using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using RailWorks.Common.Models;

namespace RailWorks.Common.Repository
{
    public class StockDataRepository : IStockDataRepository
    {
        private IMongoDatabase _database = null; 
        private const String _stockCollectionName = "Stock";

        public StockDataRepository(MongoClient Client, String DatabaseName = "RailWorks")
        {
            BsonClassMap.RegisterClassMap<StockSymbol>();
            _database = Client.GetDatabase(DatabaseName);
        }

        public StockSymbol GetStockSymbolData(FilterDefinition<StockSymbol> Filter)
        {
            IMongoCollection<StockSymbol> collection = _database.GetCollection<StockSymbol>(_stockCollectionName);

            StockSymbol result = collection.Find(Filter).FirstOrDefault();
            return result;
        }

        public void AddStock(StockSymbol Symbol)
        {
            Symbol.Added = DateTime.UtcNow;

            IMongoCollection<StockSymbol> collection = _database.GetCollection<StockSymbol>(_stockCollectionName);
            collection.InsertOne(Symbol);
        }
    }
}
