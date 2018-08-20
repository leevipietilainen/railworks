using System;
using MongoDB.Bson;
using MongoDB.Driver;
using RailWorks.Common.Repository;
using RailWorks.Common.Models;

namespace RailWorks.Common
{
    public class StockDataHandler
    {
        public StockSymbol GetStockData(String Symbol)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            StockDataRepository repository = new StockDataRepository(client);

            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("Symbol", Symbol);
            return repository.GetStockSymbolData(filter);
        }
    }
}
