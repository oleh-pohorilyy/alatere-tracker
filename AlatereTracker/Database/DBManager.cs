using AlatereTracker.AQL;
using AlatereTracker.AQL.Actions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AlatereTracker.Database
{
    class DBManager
    {
        public DBManager(DBConfig config) 
        {
            this.config = config;
        }

        public Dictionary<string, EntityDescriptor> EntityDescriptors { get; private set; }
        private DBConfig config;
        private AQLInterpretator qlInterpretator;
        private ActionHandler actionHandler;

        private Dictionary<string, EntityDescriptor> GetEntityDescriptors(IEnumerable<string> entities) 
        {
            Dictionary<string, EntityDescriptor> entityDescriptors = new Dictionary<string, EntityDescriptor>();

            foreach (string entityName in entities) 
            {
                string path = Path.Combine(config.EntitiesPath, entityName + DBConfig.ENTITY_EXTENSION);
                using (StreamReader sr = new StreamReader(path))
                {
                    string jsonDescriptor = sr.ReadToEnd();
                    EntityDescriptor descriptor = 
                        JsonConvert.DeserializeObject<EntityDescriptor>(jsonDescriptor);

                    entityDescriptors.Add(descriptor.Name, descriptor);
                }
            }

            return entityDescriptors;
        }

        public void Initialize() 
        {
            EntityDescriptors = this.GetEntityDescriptors(config.Entities);
            qlInterpretator = new AQLInterpretator(EntityDescriptors);
            actionHandler = new ActionHandler(this);
        }

        public Table GetDataFrom(EntityDescriptor descriptor)
        {
            string path = Path.Combine(this.config.DataPath, descriptor.Name + DBConfig.DATA_EXTENSION);

            using (StreamReader sr = new StreamReader(path))
            {
                var data = JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, string>>>(sr.ReadToEnd());
                return new Table(descriptor, data);
            }
        }

        public Task<Table> Query(string query) 
        {
            return Task.Run(() => {
                IEnumerable<BaseAction> actions = this.qlInterpretator.Interpretate(query);

                Table result = null;

                foreach (BaseAction action in actions)
                {
                    if (action is SelectAction) result = actionHandler.Handle(action as SelectAction);
                    else if(action is WhereAction) result = actionHandler.Handle(action as WhereAction, result);
                }

                return result;
            });
        }

        
    }
}
