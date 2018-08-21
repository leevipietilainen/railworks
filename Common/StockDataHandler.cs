using System;
using MongoDB.Bson;
using MongoDB.Driver;
using RailWorks.Common.Repository;
using RailWorks.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace RailWorks.Common
{
    public class StockDataHandler
    {
        private readonly ConfigHandler _config = null;
        //private readonly MongoClient _client = null;
        private IStockDataRepository _repository = null;

        public StockDataHandler()
        {
            _config = new ConfigHandler();
            //_client = _config.Client;
            _repository = new StockDataRepository(_config.Client);
        }

        public StockDataHandler(IStockDataRepository Repository)
        {
            _repository = Repository;
        }

        public StockSymbol GetStockData(String Symbol)
        {
            FilterDefinition<StockSymbol> filter = Builders<StockSymbol>.Filter.Eq("Symbol", Symbol);
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
