using System.Collections.Generic;

namespace CrewOnDemandService.Models
{
    public class Crew
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Base { get; set; }
        public List<string> WorkDays { get; set; }

        public List<ScheduleFlight> ScheduledFlight { get; set; }
    }
}
