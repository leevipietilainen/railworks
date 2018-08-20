using System;
using MongoDB.Bson;
using MongoDB.Driver;
using RailWorks.Common.Models;

namespace RailWorks.Common.Repository
{
    public interface IStockDataRepository
    {
        StockSymbol GetStockSymbolData(FilterDefinition<BsonDocument> Filter);

        void AddStock(StockSymbol Symbol);
    }
}