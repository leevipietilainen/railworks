using System;

namespace RailWorks.Common.Models
{
    public class StockValue
    {
        public DateTime TimeStamp { get; set; }
        public Decimal Open { get; set; }
        public Decimal High { get; set; }
        public Decimal Low { get; set; }
        public Decimal Close { get; set; }
        public Decimal Volume { get; set; }
        public String ParentId { get; set; }
    }
}