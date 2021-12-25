using AlatereTracker.Database.Converter;
using Newtonsoft.Json;
using System;

namespace AlatereTracker.Database
{
    public struct FieldDescriptor
    {
        [JsonConverter(typeof(TypeConverter))]
        public Type Type;
    }
}
