using System.Collections.Generic;

namespace AlatereTracker.Database
{
    public struct EntityDescriptor
    {
        public string Name;
        public Dictionary<string, FieldDescriptor> Fields;
    }
}
