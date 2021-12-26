using AlatereTracker.AQL.Actions;
using AlatereTracker.Database;
using AlatereTracker.QLI;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AlatereTracker.AQL
{
    class AQLInterpretator : IQLInterpretator<BaseAction>
    {
        public AQLInterpretator(Dictionary<string, EntityDescriptor> entities) 
        {
            this.entities = entities;
        }

        private Dictionary<string, Regex> operations = new Dictionary<string, Regex>() 
        {
            { nameof(SelectAction), new Regex(@"select (?<columns>.+(:from)??) from (?<table>\w+)", RegexOptions.IgnoreCase) }
        };

        private Dictionary<string, EntityDescriptor> entities;

        public IEnumerable<BaseAction> Interpretate(string query) 
        {
            List<BaseAction> actions = new List<BaseAction>();

            foreach (var pair in this.operations) 
            {
                Match match = pair.Value.Match(query);
                GroupCollection groups = match.Groups;

                if (match.Success == false) continue;

                switch (pair.Key) 
                {
                    case nameof(SelectAction):
                        string table = groups["table"].Value;
                        string[] columns = groups["columns"].Value.Replace(" ", "").Split(',');

                        if (columns.Contains("*"))
                        {
                            columns = entities[table].Fields.Keys.ToArray();
                        }

                        actions.Add(new SelectAction(table, columns));
                        break;
                    case nameof(WhereAction):
                        break;
                    default: break;
                }
            }

            return actions;
        }
    }
}
