using AlatereTracker.AQL;
using AlatereTracker.AQL.Actions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;

namespace AlatereTracker.Database
{
    class DBManager
    {
        public DBManager(DBConfig config) 
        {
            this.config = config;
        }

        private DBConfig config;
        private AQLInterpretator qlInterpretator;

        private IEnumerable<EntityDescriptor> GetEntityDescriptors(IEnumerable<string> entities) 
        {
            List<EntityDescriptor> entityDescriptors = new List<EntityDescriptor>();

            foreach (string entityName in entities) 
            {
                string path = Path.Combine(config.EntitiesPath, entityName + DBConfig.ENTITY_EXTENSION);
                using (StreamReader sr = new StreamReader(path))
                {
                    string jsonDescriptor = sr.ReadToEnd();
                    EntityDescriptor descriptor = 
                        JsonConvert.DeserializeObject<EntityDescriptor>(jsonDescriptor);

                    entityDescriptors.Add(descriptor);
                }
            }

            return entityDescriptors;
        }

        public void Initialize() 
        {
            IEnumerable<EntityDescriptor> entityDescriptors = this.GetEntityDescriptors(config.Entities);

            qlInterpretator = new AQLInterpretator(entityDescriptors);
        }

        public object Query(string query) 
        {
            IEnumerable<BaseAction> actions = this.qlInterpretator.Interpretate(query);

            object result = new object();

            foreach (BaseAction action in actions) 
            {                
                if (action is SelectAction) 
                {
                    
                }
            }

            return result;
        }

        
    }
}
