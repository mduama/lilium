using System;
using System.Collections.Generic;
using CrewOnDemandService.Models;
using CrewOnDemandService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CrewOnDemandService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private readonly ILogger<CrewController> logger;

        public ICrewService CrewService { get; }

        public CrewController(ILogger<CrewController> logger, ICrewService crewService)
        {
            this.logger = logger;
            CrewService = crewService;
        }

        [HttpGet]
        [Route("getCrewData")]
        public IEnumerable<Crew> Get()
        {
            return CrewService.GetCrewData();
        }

        [HttpGet]
        [Route("getAvailablePilotsByDatesAndLocation")]
        public IEnumerable<Crew> GetAvailablePilotsByDatesAndLocation([FromQuery] string Location, [FromQuery] DateTime DepartureDateTime, [FromQuery] DateTime ReturnDateTime)
        {
            return CrewService.GetAvailablePilotsByDatesAndLocation(Location, DepartureDateTime, ReturnDateTime);
        }

        [HttpPost]
        [Route("scheduleFlight")]
        public ActionResult ScheduleFlight([FromQuery] int pilotId, [FromBody] ScheduleFlight schedule)
        {
            CrewService.ScheduleFlight(pilotId, schedule);

            return Ok();
        }
    }
}
