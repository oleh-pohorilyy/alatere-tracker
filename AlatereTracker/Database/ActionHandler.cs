using AlatereTracker.AQL.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlatereTracker.Database
{
    class ActionHandler
    {
        public ActionHandler(DBManager manager) 
        {
            this.dbManager = manager;
        }

        private readonly DBManager dbManager;

        public Dictionary<string, ColumnDescriptor> Handle(SelectAction action) 
        {
            EntityDescriptor entity = this.dbManager.EntityDescriptors[action.From];

            var fields = action.Fields.Select(f => (name: f, descriptor: entity.Fields[f]));

            var data = this.dbManager.GetDataFrom(entity);

            var dataByColumns = this.dbManager.GetDataByColumns(data);

            var result = new Dictionary<string, ColumnDescriptor>();

            foreach (var field in fields)
            {
                result.Add(field.name, new ColumnDescriptor(field.descriptor.Type, dataByColumns[field.name]));
            }

            return result;
        }
    }
}
