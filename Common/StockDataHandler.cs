using System;
using MongoDB.Bson;
using MongoDB.Driver;
using RailWorks.Common.Repository;
using RailWorks.Common.Models;

namespace RailWorks.Common
{
    public class StockDataHandler
    {
        private MongoClient _client = null;
        private IStockDataRepository _repository = null;

        public StockDataHandler()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _repository = new StockDataRepository(_client);
        }

        public StockDataHandler(IStockDataRepository Repository)
        {
            _repository = Repository;
        }

        public StockSymbol GetStockData(String Symbol)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("Symbol", Symbol);
            return _repository.GetStockSymbolData(filter);
        }

        public void UpsertStockData(String Symbol)
        {
            if(GetStockData(Symbol) == null)
            {
                StockSymbol symbol = new StockSymbol()
                {
                    Symbol = Symbol
                };
                _repository.AddStock(symbol);
            }
        }
    }
}
