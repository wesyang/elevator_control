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
        IElevatorCarConfiguration GetConfiguration ()
        {
            var config = new Mock<IElevatorCarConfiguration>();
            config.SetupGet(x => x.LowestFloor).Returns(new FloorInfo() { FloorNumber = 1 });
            config.SetupGet(x => x.HighestFloor).Returns(new FloorInfo() { FloorNumber = 10 });
            return config.Object;
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException), "expect ArgumentNullException")]
        public void TestCreateElevatorWithNullConfiguration()
        {
            IElevatorCar elevator = new ElevatorCar(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ElevatorException), "expect ElevatorException for invalid input")]
        public void TestAddInvalidFloorStopRequest()
        {
            IElevatorCar elevator = new ElevatorCar(GetConfiguration());
            elevator.RequestFloorService(new FloorInfo() { FloorNumber = 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ElevatorException), "expect ElevatorException for invalid input")]
        public void TestAddInvalidPassangerStopRequest()
        {
            IElevatorCar elevator = new ElevatorCar(GetConfiguration());
            elevator.AddPassengerFloorStop(new FloorInfo() { FloorNumber = 20 });
        }


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

            Assert.IsTrue(!elevator.GetAllFloorStopPassengerRequests().Any(), 
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
        }
    }
}
