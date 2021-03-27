using System;

namespace ElevatorControlApi.Lib
{
    /// <summary>
    ///  Exception relate to invalid Elevator operation
    /// </summary>
    public class ElevatorException : Exception
    {
        /// <summary>
        ///  constructor
        /// </summary>
        /// <param name="message">error message</param>
        public ElevatorException(string message): base (message)
        {

        }
    }
}
