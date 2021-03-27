using System;
using System.Collections;
using System.Collections.Generic;

namespace ElevatorControlApi.Lib
{

    /// <summary>
    /// configuration settings for elevator object, such as elevator operation range  
    /// </summary>
    public interface IElevatorCarConfiguration
    {
        /// <summary>
        ///  lowest floor for the operation floor range
        /// </summary>
        FloorInfo LowestFloor { get; }

        /// <summary>
        ///  highest floor for the operation floor range
        /// </summary>
        FloorInfo HighestFloor { get; }
    }

    /// <summary>
    ///  Interface for controlling IElevatorCar
    /// </summary>
    public interface IElevatorCar
    {
        /// <summary>
        /// A person requests an elevator be sent to their current floor
        /// </summary>
        /// <param name="floor">request elevtor provides service from floor</param>
        void RequestFloorService(FloorInfo floor);

        /// <summary>
        /// A person requests that they be brought to a floor
        /// </summary>
        /// <param name="request">request elevtor goes to floor</param>
        void AddPassengerFloorStop (FloorInfo floor);

        /// <summary>
        /// An elevator car requests all floors that it’s current passengers are servicing
        /// </summary>
        /// <returns>a collection of floors</returns>
        IEnumerable<FloorInfo> GetAllFloorStopPassengerRequests();

        /// <summary>
        /// An elevator car requests the next floor it needs to service
        /// </summary>
        /// <returns>next floor to stop</returns>
        FloorInfo NextStop();
    }


    /// <summary>
    ///  control elevation operation
    /// </summary>
    public interface IElevatorController
    {
        /// <summary>
        ///  operate elevator to next stop
        /// </summary>
        void MoveToNextStop();
    }
}
