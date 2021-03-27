# elevator_control
coding exercise project

Build/System Requirments

-- ElevatorControlApi.Lib.proj requests .net standard 2.0
-- UnitTests.proj requests .net core 3.1
-- Need download MSTest and Mock nuget packages to run unit tests

Project / Folder structure
   - ElevatorControlApi.Lib  (folder contains souce code of elevator control project)
	 - IElevatorCar.cs			(Interface for integration)
	 - Models.cs				(models for integration)
	 - ElevatorCard.cs			(prototype elevator implementation)

   - UnitTests				 (folder contains unit tests)
	 - UninTests.cs				(testcases)