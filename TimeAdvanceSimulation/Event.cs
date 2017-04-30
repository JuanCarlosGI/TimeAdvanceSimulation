using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace TimeAdvanceSimulation
{
    public enum EventType
    {
        Arrival,
        Departure
    }

    /// <summary>
    /// Class representing an event happening in the system.
    /// </summary>
    public class Event : IComparable
    {
        /// <summary>
        /// Gets or sets the scheduled time of execution.
        /// </summary>
        public double ScheduledTime { get; set; }

        /// <summary>
        /// Compares the event to another instance of an event.
        /// </summary>
        /// <param name="obj">The other event</param>
        /// <returns>A value indicating whether it is lower, equal or bigger.
        /// </returns>
        public int CompareTo(object obj)
        {
            return ScheduledTime < ((Event)obj).ScheduledTime ? -1 : 1;
        }

        /// <summary>
        /// Gets or sets the type of event.
        /// </summary>
        public EventType Type { get; set; }

        /// <summary>
        /// Gets or sets the system that will process this event.
        /// </summary>
        public System System { get; set; }

        /// <summary>
        /// Gets or sets the Attribute dictionary, in case an event will have
        /// additional properties.
        /// </summary>
        public Dictionary<string, object> Attributes { get; set; } = 
            new Dictionary<string, object>();
    }
}
