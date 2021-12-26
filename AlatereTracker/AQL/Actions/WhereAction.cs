using System.Collections.Generic;

namespace AlatereTracker.AQL.Actions
{
    public delegate bool Filter(object element);

    class WhereAction : BaseAction
    {
        public WhereAction(Dictionary<string, Filter> filters) 
        {
            this.Filters = filters;
        }

        public readonly Dictionary<string, Filter> Filters;
    }
}
