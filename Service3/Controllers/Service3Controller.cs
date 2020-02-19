using System.Collections.Generic;
using Common.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Service3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Service3Controller : ControllerBase
    {
        private IPublishEndpoint PublishEndpoint { get; }

        public Service3Controller(IBusControl publishEndpoint)
        {
            PublishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public IActionResult Get()
        {
            PublishEndpoint.Publish(new Event1());
            
            return Ok();
        }

        [HttpPost]
        public IActionResult Post()
        {
            PublishEndpoint.Publish(new Event2());
            
            return Ok();
        }

        [HttpPut]
        public IActionResult Put()
        {
            PublishEndpoint.Publish(new Event3());
            
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}