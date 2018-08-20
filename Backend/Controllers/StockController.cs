using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RailWorks.Common;
using RailWorks.Common.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            StockDataHandler handler = new StockDataHandler();
            handler.UpsertStockData("MSFT");
            StockSymbol symbol = handler.GetStockData("MSFT");
            if(symbol == null)
                return new string[] { "null", "value2" };
            return new string[] { symbol.Symbol, "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
