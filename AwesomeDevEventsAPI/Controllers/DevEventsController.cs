﻿using AwesomeDevEventsAPI.Entities;
using AwesomeDevEventsAPI.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeDevEventsAPI.Controllers
{
    [Route("api/dev-events")]
    [ApiController] // não precisa do [fromBoddy]
    public class DevEventsController : ControllerBase
    {
        private DevEventsDbContext _context;

        public DevEventsController(DevEventsDbContext context)
        {
            _context = context;
        }
        // api/dev-events GET
        /// <summary>
        /// Rota GET para todo conteúdo
        /// </summary>
        /// <returns>Retorna todas as informações cadastradas</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var devEvent = _context.DevEvents.Where(d => !d.isdeleted).ToList();
            return Ok(devEvent);
        }
        // api/dev-events/12345 GET
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
            {
                return NotFound();
            }
            return Ok(devEvent);
        }
        // api/dev-events/12345 POST
        [HttpPost]
        public IActionResult Post(DevEvent devEvent)
        {
            _context.DevEvents.Add(devEvent);
            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);
        }
        // api/dev-events/12345 PUT
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
            {
                return NotFound();
            }
            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);
            return NoContent();
        }
        // api/dev-events/12345 DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
            {
                return NotFound();
            }
            devEvent.Delete();
            return NoContent();
        }

        // api/dev-events/12345/speakers
        [HttpPost("{id}/speakers")]
        public IActionResult PostSpeaker(Guid id, DevEventSpeaker speaker)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }
            devEvent.Speakers.Add(speaker);

            return NoContent();
        }

    }
}
