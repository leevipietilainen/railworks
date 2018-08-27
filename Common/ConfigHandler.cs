using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using RailWorks.Common.Models;

namespace RailWorks.Common
{
    class ConfigHandler
    {
        private IConfiguration _config = null;
        public IConfiguration Configuration
        {
            get { return _config; }
        }

        private static MongoClient _client = null;

        public MongoClient Client
        {
            get { return _client; }
        }

        public ConfigHandler()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("a59f52bb-3247-491a-8050-1d79aee5b6a6");
            _config = builder.Build();

            if(_client == null) {
                RegisterMappings();
                _client = new MongoClient(_config["ConnectionString"]);
            }
        }

        private void RegisterMappings()
        {
            BsonClassMap.RegisterClassMap<StockSymbol>(cm => {
                cm.AutoMap();
                cm.UnmapMember(c => c.Values);
                cm.MapIdMember(c => c.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
            BsonClassMap.RegisterClassMap<StockValue>(cm => {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }
    }
}