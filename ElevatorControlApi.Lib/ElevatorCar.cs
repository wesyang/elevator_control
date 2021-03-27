using System;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorControlApi.Lib
{
    /// <summary>
    /// 
    /// </summary>
    public class ElevatorCar : IElevatorCar, IElevatorController
    {
        /// <summary>
        ///  Create ElevatorCar object 
        ///  
        ///  Notes:
        ///  this is not Thread Safe Object.
        ///  (todo) Update implementation to Thread Safe if it's required
        ///  
        /// </summary>
        /// <param name="configuraiton">inject configuration</param>
        public ElevatorCar (IElevatorCarConfiguration configuraiton)
        {
            if (configuraiton == null)
            {
                throw new ArgumentNullException("configuration is required");
            }

            this.Configuration = configuraiton;
            this.Direction = ElevatorDirection.Up;
            this.CurrentFloor = configuraiton.LowestFloor;
            this.FloorStopRequests = new List<FloorStopRequest>();
        }

        /// <summary>
        /// Elevator direction (up or down)
        /// </summary>
        private ElevatorDirection Direction { get;  set; }

        /// <summary>
        /// Collection of all floor-stop request objects
        /// 
        /// Note: maintain this list in an sorted order
        /// </summary>
        private List<FloorStopRequest> FloorStopRequests { get; }

        /// <summary>
        /// Elevator current floor
        /// </summary>
        private FloorInfo CurrentFloor { get; set; }

        /// <summary>
        /// Elevator configuration
        /// </summary>
        private IElevatorCarConfiguration Configuration { get; }


        /// <summary>
        /// A person requests an elevator be sent to their current floor
        /// 
        /// API throws ElevatorException object if floor info is invalid
        /// </summary>
        /// <param name="floor">floor infomration such as floor number</param>
        public void RequestFloorService(FloorInfo floor)
        {
            AddStopRequest(floor, FloorStopType.RequestByFloor);
        }

        /// <summary>
        /// A person requests that they be brought to a floor
        /// 
        /// API throws ElevatorException object if floor info is invalid
        /// </summary>
        /// <param name="floor">floor infomration such as floor number</param>
        public void AddPassengerFloorStop(FloorInfo floor)
        {
            AddStopRequest(floor, FloorStopType.RequestByPassenger);
        }

        /// <summary>
        /// An elevator car requests all floors that it’s current passengers are servicin
        /// </summary>
        /// <returns>collection of FloorInfo abject</returns>
        public IEnumerable<FloorInfo> GetAllFloorStopsPassengerRequested()
        {
            var result = from c in this.FloorStopRequests
                         where (c.StopType & FloorStopType.RequestByPassenger) == FloorStopType.RequestByPassenger
                         select c.Floor;
            return result;
        }

        /// <summary>
        /// An elevator car requests the next floor it needs to service
        /// </summary>
        /// <returns>next stop floor object or NULL</returns>
        public FloorInfo NextStop()
        {
            if (!this.FloorStopRequests.Any())
            {
                return null;
            }

            var upFloors = from c in this.FloorStopRequests
                           where c.Floor.FloorNumber > this.CurrentFloor.FloorNumber
                           orderby c.Floor.FloorNumber ascending
                           select c.Floor;
                           
            var downFloors = from c in this.FloorStopRequests 
                             where c.Floor.FloorNumber < this.CurrentFloor.FloorNumber
                             orderby c.Floor.FloorNumber descending
                             select c.Floor;


            if (this.Direction == ElevatorDirection.Down && !downFloors.Any())
            {
                // change direct since there is no more lower floor need to stop
                this.Direction = ElevatorDirection.Up;
            }

            if (this.Direction == ElevatorDirection.Up && !upFloors.Any())
            {
                // change direct since there is no more higer floor need to stop
                this.Direction = ElevatorDirection.Down;
            }

            return (this.Direction == ElevatorDirection.Down) ? downFloors.FirstOrDefault() : upFloors.FirstOrDefault();
        }

        /// <summary>
        ///  move elevator to next stop
        /// </summary>
        public void MoveToNextStop()
        {
            var floor = this.NextStop();
            if (floor == null)
            {
                return;
            }

            int count = this.FloorStopRequests.Count;
            for (int idx = 0; idx < count; idx++)
            {
                if (this.FloorStopRequests[idx].Floor.FloorNumber == floor.FloorNumber)
                {
                    this.CurrentFloor = this.FloorStopRequests[idx].Floor;

                    this.FloorStopRequests.RemoveAt(idx);
                    return;
                }
            }
        }

        /// <summary>
        /// private helper function to add floor stop in the floor-stop request list
        /// 
        /// It maintains the floor-stop request list in sorted order
        /// It throws ElevatorException expection if the request is invalid 
        /// </summary>
        /// <param name="floor">floor info such as floor number</param>
        /// <param name="type">request type (either request from the floor or by passenger inside the car)</param>
        private void AddStopRequest (FloorInfo floor, FloorStopType type)
        {
            if (floor == null || floor.FloorNumber < Configuration.LowestFloor.FloorNumber || floor.FloorNumber > Configuration.HighestFloor.FloorNumber)
            {
                throw new ElevatorException("Invalid Request");
            }

            // no need to add request if elevator currently on the same floor
            if (CurrentFloor.FloorNumber == floor.FloorNumber)
            {
                return;
            }

            // insert or update request list with sorted order
            int count = this.FloorStopRequests.Count;
            for (int idx  =0; idx< count; idx++)
            {
                if (this.FloorStopRequests[idx].Floor.FloorNumber == floor.FloorNumber)
                {
                    // just need update the stop type
                    this.FloorStopRequests[idx].StopType |= type;
                    return;
                }
                if (this.FloorStopRequests[idx].Floor.FloorNumber > floor.FloorNumber)
                {
                    // insert new item
                    this.FloorStopRequests.Insert(idx, new FloorStopRequest() { Floor = floor, StopType = type });
                    return;
                }
            }

            // append the new request at end
            this.FloorStopRequests.Add(new FloorStopRequest() { Floor = floor, StopType = type });
        }
    }
}
