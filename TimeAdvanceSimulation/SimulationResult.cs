namespace TimeAdvanceSimulation
{
    /// <summary>
    /// Class representing the resulting statistics of a simulation.
    /// </summary>
    public class SimulationResult
    {
        /// <summary>
        /// Gets or sets the average amount of jobs in system.
        /// </summary>
        public double N { get; set; }

        /// <summary>
        /// Gets or sets the utility of the system.
        /// </summary>
        public double U { get; set; }

        /// <summary>
        /// Gets or sets the average residence time of jobs in the system.
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// Gets or sets the average throughput of the system.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the system for which the result refer to.
        /// </summary>
        public System System { get; set; }
    }
}
