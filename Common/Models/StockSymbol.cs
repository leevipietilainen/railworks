using System;
using System.Collections.Generic;

namespace RailWorks.Common.Models
{
    public class StockSymbol
    {
        public String Symbol { get; set; }
        public DateTime Added { get; set; }
        public List<StockValue> Values { get; set; }
    }
}