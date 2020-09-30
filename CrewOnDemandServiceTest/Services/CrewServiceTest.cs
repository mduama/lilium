using CrewOnDemandService.Models;
using CrewOnDemandService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace CrewOnDemandServiceTest.Services
{
    public class CrewServiceTest
    {
        private readonly Mock<ILogger<CrewService>> logger;
        private readonly Mock<IWebHostEnvironment> webHostEnvironment;
        private readonly CrewService crewService;

        public CrewServiceTest()
        {
            this.logger = new Mock<ILogger<CrewService>>();
            this.webHostEnvironment = new Mock<IWebHostEnvironment>();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.Replace("CrewOnDemandServiceTest\\bin\\Debug\\netcoreapp3.1", "CrewOnDemandService"));
            this.webHostEnvironment.Setup(m => m.ContentRootPath).Returns(path);
            this.crewService = new CrewService(this.logger.Object, this.webHostEnvironment.Object);
        }

        [Fact]
        public void SchedulesANewFlightTest()
        {
            // Arrange
            var crewMember = this.crewService.GetCrewData().FirstOrDefault();
            var countFlightsBeforeSchedule = crewMember.ScheduledFlight != null ? crewMember.ScheduledFlight.Count() : 0;

            var newFlight = new ScheduleFlight
            {
                DepartureDateTime = DateTime.Now.AddHours(-1),
                ReturnDateTime = DateTime.Now.AddHours(1),
            };

            // Act
            this.crewService.ScheduleFlight(crewMember.Id, newFlight);
            var crewMemberUpdated = this.crewService.GetCrewData().FirstOrDefault();
            var countFlightsAfterSchedule = crewMemberUpdated.ScheduledFlight.Count();

            // Assert
            Assert.True(countFlightsBeforeSchedule < countFlightsAfterSchedule);
        }
    }
}
