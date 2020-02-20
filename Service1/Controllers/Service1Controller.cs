using Common.MassTransit.Services;
using Common.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Service1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Service1Controller : ControllerBase
    {
        private readonly IMassTransitService _massTransitService;
        
        public Service1Controller(IMassTransitService massTransitService)
        {
            _massTransitService = massTransitService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _massTransitService.PublishMessage(new Event1());
            return Ok();
        }

        [HttpPost]
        public IActionResult Post()
        {
            _massTransitService.PublishMessage(new Event2());
            return Ok();
        }

        [HttpPut]
        public IActionResult Put()
        {
            _massTransitService.PublishMessage(new Event3());
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            _massTransitService.SendMessage(new Command1());
            return Ok();
        }
    }
}