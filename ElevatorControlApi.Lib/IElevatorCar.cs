using System.Collections.Generic;

namespace ElevatorControlApi.Lib
{

    /// <summary>
    /// configuration settings for elevator object, such as elevator floor range  
    /// 
    /// </summary>
    public interface IElevatorCarConfiguration
    {
        /// <summary>
        ///  lowest floor for the operation
        /// </summary>
        FloorInfo LowestFloor { get; }

        /// <summary>
        ///  highest floor for the operation
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
        /// 
        /// API throws ElevatorException object if floor info is invalid
        /// </summary>
        /// <param name="floor">floor where passenger asking for elevator serivce</param>
        void RequestFloorService(FloorInfo floor);

        /// <summary>
        /// A person requests that they be brought to a floor
        /// 
        /// API throws ElevatorException object if floor info is invalid
        /// </summary>
        /// <param name="request">floor where passenger need to go </param>
        void AddPassengerFloorStop (FloorInfo floor);

        /// <summary>
        /// An elevator car requests all floors that it’s current passengers are servicing
        /// </summary>
        /// <returns>a collection of floors</returns>
        IEnumerable<FloorInfo> GetAllFloorStopsPassengerRequested();

        /// <summary>
        /// An elevator car requests the next floor it needs to service
        /// </summary>
        /// <returns>next stop floor object or NULL</returns>
        FloorInfo NextStop();
    }


    /// <summary>
    ///  control elevator operation, such as move to next stop
    ///  
    /// </summary>
    public interface IElevatorController
    {
        /// <summary>
        ///  operate elevator to next stop
        /// </summary>
        void MoveToNextStop();
    }
}
