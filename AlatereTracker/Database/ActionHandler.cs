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

        public Table Handle(SelectAction action) 
        {
            EntityDescriptor entity = this.dbManager.EntityDescriptors[action.From];

            Table data = this.dbManager.GetDataFrom(entity);

            if (action.Fields.Length == data.Columns.Count()) 
            {
                return data;
            }

            IEnumerable<object[]> selectedData = data.Select(action.Fields);

            return new Table(entity, selectedData);
        }

        public Table Handle(WhereAction action, Table data)
        {
            Table table = new Table(data);

            foreach (var row in data.Rows) 
            {
                foreach (var filter in action.Filters) 
                {
                    int columnIndex = data.GetColumnIndex(filter.Key);
                    Filter filterFn = filter.Value;

                    if (filterFn(row[columnIndex])) continue;

                    table.Remove(row);
                    break;
                }
            }

            return table;
        }
    }
}
