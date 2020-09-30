using CrewOnDemandService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrewOnDemandService.Services
{
    public interface ICrewService
    {
        public IEnumerable<Crew> GetCrewData();
        public void ScheduleFlight(int PilotId, ScheduleFlight scheduleFlight);
        public IEnumerable<Crew> GetAvailablePilotsByDatesAndLocation(string Location, DateTime DepartureDateTime, DateTime ReturnDateTime);
    }

    public class CrewService: ICrewService
    {
        private readonly ILogger<CrewService> logger;

        public CrewService(ILogger<CrewService> logger, IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonCrew
        {
            get { return Path.Combine(WebHostEnvironment.ContentRootPath, "Models", "crew.json"); }
        }

        public IEnumerable<Crew> GetCrewData()
        {
            using var jsonFileReader = File.OpenText(JsonCrew);
            return JsonSerializer.Deserialize<Crew[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public void ScheduleFlight(int PilotId, ScheduleFlight scheduleFlight) {
            var crew = GetCrewData();

            if (crew.First(x => x.Id == PilotId).ScheduledFlight == null)
            {
                var schedule = new List<ScheduleFlight>
                {
                    scheduleFlight
                };
                crew.First(x => x.Id == PilotId).ScheduledFlight = schedule;
            }
            else
            {
                var schedule = crew.First(x => x.Id == PilotId).ScheduledFlight;
                schedule.Add(scheduleFlight);
                crew.First(x => x.Id == PilotId).ScheduledFlight = schedule;
            }

            using var outputStream = File.OpenWrite(JsonCrew);
            JsonSerializer.Serialize<IEnumerable<Crew>>(
                new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    SkipValidation = true,
                    Indented = true
                }),
                crew
            );
        }

        public IEnumerable<Crew> GetAvailablePilotsByDatesAndLocation(string Location, DateTime DepartureDateTime, DateTime ReturnDateTime)
        {
            var crew = GetCrewData();
            var availableCrew = new List<Crew>();

            var inLocationCrew = from c in crew
                                 where c.Base == Location &&
                                 c.WorkDays.Contains(DepartureDateTime.DayOfWeek.ToString())
                                 select c;

            var crewWithoutSchedule = from ilc in inLocationCrew.Where(x => x.ScheduledFlight == null)
                                      select ilc;
            availableCrew.AddRange(crewWithoutSchedule);

            var crewWithSchedule = (from ilc in inLocationCrew.Where(x => x.ScheduledFlight != null && x.ScheduledFlight.Any())
                                   from sf in ilc.ScheduledFlight
                                   where ((DepartureDateTime > sf.ReturnDateTime || ReturnDateTime < sf.DepartureDateTime))
                                   orderby ilc.ScheduledFlight.Count() ascending
                                   select ilc).Distinct();
            availableCrew.AddRange(crewWithSchedule);
            
            return availableCrew;
        }
    }
}
