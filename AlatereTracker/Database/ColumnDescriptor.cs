using System;
using System.Collections.Generic;

namespace AlatereTracker.Database
{
    public class ColumnDescriptor
    {
        public readonly Type Type;
        public readonly IEnumerable<object> Data;

        public ColumnDescriptor(Type type, IEnumerable<object> data) 
        {
            this.Type = type;
            this.Data = data;
        }
    }
}
