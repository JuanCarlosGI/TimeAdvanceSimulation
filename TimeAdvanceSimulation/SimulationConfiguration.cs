using System.Collections.Generic;

namespace TimeAdvanceSimulation
{
    /// <summary>
    /// Class representing the configuration that a simulator might have.
    /// </summary>
    public class SimulationConfiguration
    {
        /// <summary>
        /// Gets or sets the time when the simulation starts. Defaults at 0.
        /// </summary>
        public double ClockStart { get; set; } = 0;

        /// <summary>
        /// Gets or sets the time when the simulation ends. Defaults at 1000.
        /// </summary>
        public double ClockEnd { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the arrival rate of jobs. Defaults at 1.
        /// </summary>
        public double ArrivalRate { get; set; } = 1;
        
        /// <summary>
        /// Gets or sets a value indicating whether the simulation will be
        /// printed in console. Defaults to false.
        /// </summary>
        public bool Verbose { get; set; } = false;

        /// <summary>
        /// Gets or sets a list of systems that will be tracked throughout the simulation.
        /// </summary>
        public List<System> TrackedSystems { get; set; }

        /// <summary>
        /// Gets or sets the entry point of a network of systems, where simulated arrivals will start.
        /// </summary>
        public System EntryPoint { get; set; }
    }
}
