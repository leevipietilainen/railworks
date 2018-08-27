using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RailWorks.Common.Models;

namespace RailWorks.Common.Repository
{
    public class StockDataRepository : IStockDataRepository
    {
        private IMongoDatabase _database = null; 
        private const String _stockCollectionName = "Stock";
        private const String _stockDataCollectionName = "StockData";

        public StockDataRepository(MongoClient Client, String DatabaseName = "RailWorks")
        {
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

        public Task UpdateStockAsync(StockSymbol Symbol)
        {
            FilterDefinition<StockSymbol> filter = Builders<StockSymbol>.Filter.Eq("Symbol", Symbol);
            IMongoCollection<StockSymbol> collection = _database.GetCollection<StockSymbol>(_stockCollectionName);

            return collection.ReplaceOneAsync(filter, Symbol);
        }

        public Task AddStockSymbolDataAsync(StockValue DataPoint)
        {
            IMongoCollection<StockValue> collection = _database.GetCollection<StockValue>(_stockDataCollectionName);
            return collection.InsertOneAsync(DataPoint);
        }
    }
}
