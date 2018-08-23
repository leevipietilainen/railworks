﻿using System;
using MongoDB.Bson;
using MongoDB.Driver;
using RailWorks.Common.Repository;
using RailWorks.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Threading.Tasks;

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

        public async Task<StockSymbol> UpsertStockDataAsync(String Symbol)
        {
            StockSymbol symbol = GetStockData(Symbol);
            if(symbol == null)
            {
                symbol = new StockSymbol()
                {
                    Symbol = Symbol
                };
                _repository.AddStock(symbol);
            }
            StockDataUpdater updater = new StockDataUpdater();
            return await updater.UpdateStockSymbolDataAsync(symbol);
        }
    }
}
