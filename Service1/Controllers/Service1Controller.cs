using System.Collections.Generic;
using Common.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Service1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Service1Controller : ControllerBase
    {
        private IPublishEndpoint PublishEndpoint { get; }

        public Service1Controller(IBusControl publishEndpoint)
        {
            PublishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public IActionResult Get()
        {
            PublishEndpoint.Publish(new Event1());
            
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}