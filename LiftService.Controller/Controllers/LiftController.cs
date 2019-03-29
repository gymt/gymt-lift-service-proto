using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LiftService.Controller.Controllers
{
    [Route("api/lift")]
    [ApiController]
    public class LiftController : ControllerBase
    {
        // GET api/lift
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/lift/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/lift
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/lift/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/lift/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
