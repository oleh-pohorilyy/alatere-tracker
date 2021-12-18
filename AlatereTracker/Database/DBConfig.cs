using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlatereTracker.Database
{
    struct DBConfig
    {
        public readonly IEnumerable<string> Entities;
        public readonly string Name;
        public readonly string Path;
        [JsonIgnore]
        public readonly string EntitiesPath;
        [JsonIgnore]
        public readonly string DataPath;

        public const string CONFIG_EXTENSION = ".dbc";
        public const string ENTITY_EXTENSION = ".ent";
        public const string DATA_EXTENSION = ".tbl";

        public DBConfig(IEnumerable<string> entities, string name, string path)
        {
            this.Name = name;
            this.Entities = entities;
            this.Path = path;
            this.EntitiesPath = System.IO.Path.Combine(path, "entities");
            this.DataPath = System.IO.Path.Combine(path, "data");
        }

        public override string ToString()
        {
            IEnumerable<string> forattedEntities = this.Entities.Select(e => "  ■ " + e);

            return $"■ DB Name: {this.Name}\n" +
                   $"■ DB Path: {this.Path}\n" +
                   $"■ DB Entities:\n{string.Join("\n", forattedEntities)}";
        }
    }
}
