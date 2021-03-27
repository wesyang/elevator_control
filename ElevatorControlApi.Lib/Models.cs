using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorControlApi.Lib
{
    public class FloorInfo
    {
        public uint FloorNumber { get; set;}
    }

    public enum ElevatorDirection
    {
        Up = 1,
        Down = 2,
    }

    public enum FloorStopType
    {
        RequestByFloor = 1,
        RequestByPassenger = 2,
        RequestByBoth = RequestByFloor | RequestByPassenger,
    }

    public class FloorStopRequest
    {
        public FloorStopType StopType { get; set; }
        public FloorInfo Floor { get; set; }
    }
}
