﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Service3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Service3Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
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