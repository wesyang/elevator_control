using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorControlApi.Lib
{
    /// <summary>
    /// FloorInfo model
    /// 
    /// contains information such as FloorNumber and others
    /// </summary>
    public class FloorInfo
    {
        /// <summary>
        /// Floor number: 1 means first floor
        /// </summary>
        public uint FloorNumber { get; set;}

        ///
        /// (todo) add other properties relate to floor if needs
        ///
    }

    /// <summary>
    /// ElevatorDirection
    /// 
    /// enum value of direction (Up or Down)
    /// </summary>
    public enum ElevatorDirection
    {
        /// <summary>
        /// Indicate Elevator goes up
        /// </summary>
        Up = 1,

        /// <summary>
        /// Indicate Elevator goes down
        /// </summary>
        Down = 2,
    }

    /// <summary>
    /// Elevator Floor Stop Request Type
    /// </summary>
    public enum FloorStopType
    {
        /// <summary>
        /// Indicator the request was trigger by pushing floor button.  Passenger asks for Elevator service.
        /// 
        /// </summary>
        RequestByFloor = 0x01,

        /// <summary>
        /// Indicator the request was trigger inside the elevator,  Passenger provides the destination floor 
        /// 
        /// </summary>
        RequestByPassenger =0x10,

        /// <summary>
        /// Indicator floor stop is for both passenger request and passenger destination
        /// </summary>
        RequestByBoth = RequestByFloor | RequestByPassenger,
    }

    /// <summary>
    ///  Floor stop request
    ///  
    ///  this model is used by Elevator to keep tracking where it needs to stop
    /// </summary>
    public class FloorStopRequest
    {
        /// <summary>
        /// FloorStop Type: indicate the request type 
        /// 
        /// see detail info from enum FloorStopType
        /// </summary>
        public FloorStopType StopType { get; set; }

        /// <summary>
        /// Floor Info object which contains the infomation about floor data such as FloorNumber
        /// 
        /// see detail info from class FloorInfo
        /// </summary>
        public FloorInfo Floor { get; set; }
    }
}
