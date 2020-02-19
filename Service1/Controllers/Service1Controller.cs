using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Constants;
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
        
        private ISendEndpointProvider SendEndpointProvider { get; }

        public Service1Controller(IBusControl busControl)
        {
            PublishEndpoint = busControl;
            SendEndpointProvider = busControl;
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
        public async Task<IActionResult> Delete()
        {
            var sendEndpoint = await SendEndpointProvider.GetSendEndpoint(new Uri(PathConstants.BUS_URL + PathConstants.COMMAND1));

            await sendEndpoint.Send(new Command1());
            
            return Ok();
        }
    }
}