using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RailWorks.Common.Models;

namespace RailWorks.Common.Repository
{
    public interface IStockDataRepository
    {
        StockSymbol GetStock(FilterDefinition<StockSymbol> Filter);

        void AddStock(StockSymbol Symbol);
        Task UpdateStockAsync(StockSymbol Symbol);
        Task<StockSymbol> GetStockSymbolDataAsync(FilterDefinition<StockSymbol> Filter, FilterDefinition<StockValue> DataFilter);
        Task AddStockSymbolDataAsync(StockValue DataPoint);
    }
}