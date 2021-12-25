using AlatereTracker.AQL;
using AlatereTracker.AQL.Actions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public IEnumerable<Dictionary<string, string>> GetDataFrom(EntityDescriptor descriptor) 
        {
            string path = Path.Combine(this.config.DataPath, descriptor.Name + DBConfig.DATA_EXTENSION);

            using (StreamReader sr = new StreamReader(path)) 
                return JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, string>>>(sr.ReadToEnd());
        }

        public Dictionary<string, List<object>> GetDataByColumns(IEnumerable<Dictionary<string, string>> data) 
        {
            if (data.Count() == 0) return default;

            Dictionary<string, List<object>> newData = new Dictionary<string, List<object>>();

            foreach(string column in data.First().Keys) 
            {
                newData[column] = new List<object>();
            }

            foreach (Dictionary<string, string> row in data) 
            {
                foreach (string column in row.Keys) 
                {
                    newData[column].Add(row[column]);
                }
            }

            return newData;
        }

        public Task<Dictionary<string, ColumnDescriptor>> Query(string query) 
        {
            return Task.Run(() => {
                IEnumerable<BaseAction> actions = this.qlInterpretator.Interpretate(query);

                Dictionary<string, ColumnDescriptor> result = new Dictionary<string, ColumnDescriptor>();

                foreach (BaseAction action in actions)
                {
                    if (action is SelectAction) result = actionHandler.Handle(action as SelectAction);
                    
                }

                return result;
            });
        }

        
    }
}
