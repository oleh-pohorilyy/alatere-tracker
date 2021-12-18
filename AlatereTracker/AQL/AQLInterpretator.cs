using AlatereTracker.AQL.Actions;
using AlatereTracker.Database;
using AlatereTracker.QLI;
using System;
using System.Collections.Generic;

namespace AlatereTracker.AQL
{
    class AQLInterpretator : IQLInterpretator<BaseAction>
    {
        public AQLInterpretator(IEnumerable<EntityDescriptor> entities) 
        {
            this.entities = entities;
        }

        private IEnumerable<EntityDescriptor> entities;

        public IEnumerable<BaseAction> Interpretate(string query) 
        {
            return new BaseAction[] { new SelectAction("mood", new string[] { "value" }) };
        }
    }
}
