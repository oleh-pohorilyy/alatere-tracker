using System;

namespace AlatereTracker.AQL.Actions
{
    class SelectAction : BaseAction
    {
        public readonly string From;
        public readonly string[] Fields;
        public readonly MapIntent[] MapActions;

        public SelectAction(string from, string[] fields, MapIntent[] mapActions)
        {
            this.From = from;
            this.Fields = fields;
            this.MapActions = mapActions;
        }

        public SelectAction(string from, string[] fields)
            : this(from, fields, Array.Empty<MapIntent>()) { }
    }
}
