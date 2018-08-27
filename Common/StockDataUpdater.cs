using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using RailWorks.Common.Repository;
using RailWorks.Common.Models;
using AlphaVantage.Net.Core;
using Newtonsoft.Json.Linq;

namespace RailWorks.Common
{
    class StockDataUpdater
    {
        private readonly ConfigHandler _config = null;
        private readonly IStockDataRepository _repository = null;

        public StockDataUpdater(IStockDataRepository Repository)
        {
            _config = new ConfigHandler();
            _repository = Repository;
        }

        public void UpdateStockSymbolData(StockSymbol Symbol)
        {
            throw new NotImplementedException();
        }

        public async Task<StockSymbol> UpdateStockSymbolDataAsync(StockSymbol Symbol)
        {
            //String baseUrl = null;
            String apiKey = null;
            try
            {
                //baseUrl = _config.Configuration["AlphavantageBaseUrl"];
                apiKey = _config.Configuration["AlphavantageApiKey"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            if(String.IsNullOrWhiteSpace(apiKey))
                throw new Exception("AlphavantageApiKey null or empty!");
 
            AlphaVantageCoreClient client = new AlphaVantageCoreClient();
            Dictionary<string, string> query = new Dictionary<string, string>()
            {
                { "symbol", Symbol.Symbol },
                { "interval", "5min" },
                { "outputsize", "full" }
            };

            JObject result = await client.RequestApiAsync(apiKey, ApiFunction.TIME_SERIES_INTRADAY, query);
            DateTime lastRefresh = Symbol.LastRefresh ?? DateTime.MinValue;
            string timeZoneString = (string)result["Meta Data"]["6. Time Zone"];
            //TODO: Move elsewhere
            if("US/Eastern".Equals(timeZoneString))
                timeZoneString = "US Eastern Standard Time";
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneString);

            Symbol.Values = new List<StockValue>();
            List<Task> waitingTasks = new List<Task>();
            foreach (JObject timeSerie in result["Time Series (5min)"].Values())
            {
                JProperty prop = (JProperty)timeSerie.Parent;
                String timestampProp = prop.Name;
                DateTime localTimeStamp = DateTime.ParseExact(timestampProp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime utcTimeStamp = TimeZoneInfo.ConvertTimeToUtc(localTimeStamp, timeZone);

                if(utcTimeStamp < lastRefresh)
                    continue;

                StockValue value = new StockValue()
                {
                    TimeStamp = utcTimeStamp,
                    Open = Decimal.Parse(timeSerie.Value<string>("1. open")),
                    High = Decimal.Parse(timeSerie.Value<string>("2. high")),
                    Low = Decimal.Parse(timeSerie.Value<string>("3. low")),
                    Close = Decimal.Parse(timeSerie.Value<string>("4. close")),
                    Volume = Decimal.Parse(timeSerie.Value<string>("5. volume")),
                    ParentId = Symbol.Id
                };

                Symbol.Values.Add(value);
                
                waitingTasks.Add(_repository.AddStockSymbolDataAsync(value));
                if(waitingTasks.Count > 4)
                {
                    int i = Task.WaitAny(waitingTasks.ToArray());
                    waitingTasks.RemoveAt(i);
                }
            }
            Task.WaitAll(waitingTasks.ToArray());
            waitingTasks.Clear();

            DateTime localRefresh = DateTime.ParseExact((string)result["Meta Data"]["3. Last Refreshed"], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Symbol.LastRefresh = TimeZoneInfo.ConvertTimeToUtc(localRefresh, timeZone);

            return Symbol;
        }
    }
}