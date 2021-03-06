# Lilium Crew On Demand Service

## Implementation Notes.

I have developed the requested test case using:
 - .net core 3.1 with asp.net core for the API.
 - React.js with TypeScript for the UI.
 
API:
 - Backend application uses swagger (https://swagger.io/) to document the endpoints:
	- Start the application using Visual Studio 2019 importing solution file: CrewOnDemandService/CrewOnDemandService.sln.
	- API url: https://localhost:44399/swagger/index.html
	- Implemented endpoints:
		- /api/Crew/getCrewData:
			Returns all crew data from the underline database that in this case is a json file (CrewOnDemandService/Models/crew.json)
		- /api/Crew/getAvailablePilotsByDatesAndLocation:
			Filters crew data by dates and location. To implement the specification that requests should be distributed evenly the endpoint returns
			available crew members sorted by number of scheduled flights ascending.
			This way the crew members with less scheduled flights will show at the top of the list on the service UI.		
		- /api/Crew/scheduleFlight:
			Schedules a flight for the selected pilot Id on the dates proposed and saves the information to the database (json file).
	- Tests project CrewOnDemandServiceTest.
		- Run the tests using Visual Studio 2019.

 - Improvements/extension points:
	- Add extra tests like testAvailablePilotsByDatesAndLocation.
	- Use EntityFramework (https://docs.microsoft.com/en-us/ef/core/) and save Crew data to a SQLServer database.

UI:
 - To start the UI run the next commands inside crewondemandapp folder:
	- npm i
	- npm start
    - UI will be started on localhost:3000

 - Improvements/extension points:
	- Use a dropdown on location input. A call can be done to /api/Crew/getCrewData and extract the list of locations or a new endpoint
to get the list of locations can be implemented.
	- Improve design/UX. I have used basic bootstrap implementation (https://getbootstrap.com/).
	- Use react-datepicker (https://www.npmjs.com/package/react-datepicker) to have Datepicker component instead of adding Departure and Return date manually.
	- Add extra information on crew data list. More information can be added to the list like WorkDays and ScheduledFlights.
	- Extract api url to an appsettings configuration file.
	- Show errors and messages in toast notifications (https://getbootstrap.com/docs/4.3/components/toasts/)
	- Update crew data list after a new flight is scheduled instead of resetting the form.
	- Add tests to react components using Jest (https://jestjs.io/).
