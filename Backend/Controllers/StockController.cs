using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RailWorks.Common;
using RailWorks.Common.Models;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        // GET api/stock
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            StockDataHandler handler = new StockDataHandler();
            //handler.UpsertStockData("MSFT");
            StockSymbol symbol = handler.GetStockData("MSFT");
            if(symbol == null)
                return new string[] { "null", "value2" };
            return new string[] { symbol.Symbol, "value2" };
        }

        // GET api/stock/MSFT
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            StockDataHandler handler = new StockDataHandler();
            StockSymbol symbol = handler.GetStockData(id);
            return JsonConvert.SerializeObject(symbol);
        }

        // POST api/stock
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/stock/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
        }
    }
}
