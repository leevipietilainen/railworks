using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using RailWorks.Common.Models;

namespace RailWorks.Common.Repository
{
    public class StockDataRepository : IStockDataRepository
    {
        private IMongoDatabase _database = null; 
        private const String _stockCollectionName = "Stock";

        public StockDataRepository(MongoClient Client, String DatabaseName = "RailWorks")
        {
            _database = Client.GetDatabase(DatabaseName);
        }

        public StockSymbol GetStockSymbolData(FilterDefinition<BsonDocument> Filter)
        {
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(_stockCollectionName);

            BsonDocument result = collection.Find(Filter).FirstOrDefault();
            if(result == null)
                return null;

            StockSymbol item = new StockSymbol()
            {
                Symbol = result.GetValue("Symbol").AsString,
                Added = result.GetValue("Added").ToUniversalTime()
            };

            return item;
        }

        public void AddStock(StockSymbol Symbol)
        {
            BsonDocument document = new BsonDocument()
            {
                { "Symbol", Symbol.Symbol },
                { "Added", DateTime.UtcNow }
            };

            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(_stockCollectionName);
            collection.InsertOne(document);
        }
    }
}
