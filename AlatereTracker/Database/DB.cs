using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlatereTracker.Database
{
    class DB
    {
        public DB(string configPath) 
        {
            FileInfo configInfo = new FileInfo(configPath);

            if (configInfo.Extension != DBConfig.CONFIG_EXTENSION)
                throw new NotSupportedException($"Only \"{DBConfig.CONFIG_EXTENSION}\" extension is valid!");

            this.ConfigPathInfo = configInfo;
        }

        public DBManager Manager { get; private set; }

        public readonly FileInfo ConfigPathInfo;
        public DBConfig Config { get; private set; }

        private void WriteConfig(DBConfig config) 
        {
            Config = config;

            JsonSerializer serializer = new JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Include
            };

            using (StreamWriter sw = new StreamWriter(this.ConfigPathInfo.FullName))
                using (JsonWriter jw = new JsonTextWriter(sw)) 
                {
                    jw.Formatting = Formatting.Indented;
                    serializer.Serialize(jw, config);
                }
        }

        private DBConfig ReadConfig() 
        {
            using (StreamReader sr = new StreamReader(this.ConfigPathInfo.FullName))
                return JsonConvert.DeserializeObject<DBConfig>(sr.ReadToEnd());
        }

        private IEnumerable<string> FindAllEntities(string path) 
        {
            if (Directory.Exists(path) == false) return Array.Empty<string>();

            return Directory
                .GetFiles(path, $"*{DBConfig.ENTITY_EXTENSION}")
                .Select(f => Path.GetFileNameWithoutExtension(f));
        }

        public Task Connect()
        {
            return Task.Run(() => {
                if (this.ConfigPathInfo.Directory.Exists == false)
                    Directory.CreateDirectory(this.ConfigPathInfo.DirectoryName);

                IEnumerable<string> entities = 
                    this.FindAllEntities(Path.Combine(this.ConfigPathInfo.DirectoryName, "entities"));

                DBConfig config;

                if (this.ConfigPathInfo.Exists == false)
                {
                    config = new DBConfig(entities, this.ConfigPathInfo.Name, this.ConfigPathInfo.DirectoryName);
                    this.WriteConfig(config);
                } else 
                {
                    config = this.ReadConfig();
                    config = new DBConfig(entities, config.Name, config.Path);
                    this.WriteConfig(config);
                }

                this.Manager = new DBManager(config);
                this.Manager.Initialize();
            });
        }
    }
}
