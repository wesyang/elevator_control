using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElevatorControlApi.Lib;
using Moq;
using System;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        /// <summary>
        ///  mock dependency object
        /// </summary>
        /// <returns></returns>
        IElevatorCarConfiguration GetConfiguration()
        {
            var config = new Mock<IElevatorCarConfiguration>();
            config.SetupGet(x => x.LowestFloor).Returns(new FloorInfo() { FloorNumber = 1 });
            config.SetupGet(x => x.HighestFloor).Returns(new FloorInfo() { FloorNumber = 10 });
            return config.Object;
        }

        /// <summary>
        /// Verify constructor which validating dependency injection
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "expect ArgumentNullException")]
        public void TestCreateElevatorWithNullConfiguration()
        {
            IElevatorCar elevator = new ElevatorCar(null);
        }

        /// <summary>
        /// Verify RequestFloorService() which validate input request object
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ElevatorException), "expect ElevatorException for invalid input")]
        public void TestAddInvalidFloorStopRequest()
        {
            IElevatorCar elevator = new ElevatorCar(GetConfiguration());
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 0 });
        }

        /// <summary>
        /// Verify AddPassengerFloorStop() which validate input request object
        /// </summary>
        /// 
        [TestMethod]
        [ExpectedException(typeof(ElevatorException), "expect ElevatorException for invalid input")]
        public void TestAddInvalidPassangerStopRequest()
        {
            IElevatorCar elevator = new ElevatorCar(GetConfiguration());
            elevator.AddPassengerFloorStop(new FloorInfo() { FloorNumber = 20 });
        }


        /// <summary>
        /// Test Elevator object with RequestFloorService() function
        /// 
        /// Verify by calling NextStop () to see if NextStop works after adding floor stop requests
        /// </summary>
        /// 
        [TestMethod]
        public void TestElevatorFloorStopRequests()
        {
            IElevatorCar elevator = new ElevatorCar(GetConfiguration());
            IElevatorController controller = elevator as IElevatorController;
            Assert.IsTrue(controller != null, "Expect elevator object also implements IElevatorController, update test if code been refactory");

            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 6 });
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 3 });
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 8 });
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 5 });
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 1 });
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 5 });

            Assert.IsTrue(!elevator.GetAllFloorStopsPassengerRequested().Any(),
                "Expected there is no passage floor stop request");

            // currently elevator is at intial 1st floor.  
            // expect next stop is 3rd floor
            var nextStop = elevator.NextStop();
            var expected = 3;

            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");

            controller.MoveToNextStop();
            expected = 5;
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");

            controller.MoveToNextStop();
            expected = 6;
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");

            // add request from opposite elevator direction
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 1 });

            controller.MoveToNextStop();
            expected = 8;
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");


            controller.MoveToNextStop();
            expected = 1;
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");

            // add request from opposite elevator direction
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 9 });

            controller.MoveToNextStop();
            expected = 9;
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");

        }


        /// <summary>
        /// Test Elevator object with AddPassengerFloorStop()
        /// 
        /// Verify by calling GetAllFloorStopPassengerRequests () to see if query works
        /// Verify by calling NextStop () to see if NextStop works after adding passenger stop requests
        /// </summary>
        [TestMethod]
        public void TestPassengerFloorStopRequests()
        {
            IElevatorCar elevator = new ElevatorCar(GetConfiguration());
            IElevatorController controller = elevator as IElevatorController;
            Assert.IsTrue(controller != null, "Expect elevator object also implements IElevatorController, update test if code been refactory");

            elevator.AddPassengerFloorStop(new FloorInfo() { FloorNumber = 5 });
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 6 });
            var allRequests = elevator.GetAllFloorStopsPassengerRequested();
            Assert.IsTrue(allRequests.Count() == 1 && allRequests.First().FloorNumber == 5, 
                "expect only one passenger request to stop at 5th floor");

            elevator.AddPassengerFloorStop(new FloorInfo() { FloorNumber = 8 });
            allRequests = elevator.GetAllFloorStopsPassengerRequested();
            Assert.IsTrue(allRequests.Count() == 2 && allRequests.First().FloorNumber == 5,
                "expect two passenger request, fisrt request stop at 5th floor");
            Assert.IsTrue(allRequests.Last().FloorNumber == 8, "expect second request stop at 8");

            int expected = 5;
            var nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");


            controller.MoveToNextStop();
            expected = 6;
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");

            controller.MoveToNextStop();

            // add passenge floor with opposite direction
            elevator.AddPassengerFloorStop(new FloorInfo() { FloorNumber = 3 });
            Assert.IsTrue(allRequests.Count() == 2 && allRequests.First().FloorNumber == 3,
                    "expect two passenger request, fisrt request stop at 3 th floor");
            Assert.IsTrue(allRequests.Last().FloorNumber == 8, "expect second request stop at 8");

            expected = 8;
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");

            controller.MoveToNextStop();
            expected = 3;
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop.FloorNumber == expected,
                    $"Expected next elevator stop is floor {expected}");

            // no item in the request list.  expect null from calling NextStop()
            controller.MoveToNextStop();
            nextStop = elevator.NextStop();
            Assert.IsTrue(nextStop == null, $"Expected next step is NULL");

            // expect MoveToNextStop() function handles edge condition gracefully     
            controller.MoveToNextStop();
        }
    }
}
