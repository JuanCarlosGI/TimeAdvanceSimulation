using System;
using System.Collections.Generic;

namespace TimeAdvanceSimulation
{
    /// <summary>
    /// Class representing a system that can process jobs and then pass them along.
    /// </summary>
    public class System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="System"/> class.
        /// </summary>
        /// <param name="processors">Amount of processors that the system has.</param>
        public System(int processors)
        {
            Processors = processors;
            Departures = new double[processors];
            for (var processor = 0; processor < processors; processor++)
            {
                Departures[processor] = double.MaxValue;
            }
        }

        /// <summary>
        /// Gets or sets the average service time of jobs. Defaults at 1.
        /// </summary>
        public double ServiceTime { get; set; } = 1;

        /// <summary>
        /// Gets or sets the amount of processors that will be used. Defaults at
        /// 1.
        /// </summary>
        public int Processors { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum amount of jobs that the system can hold.
        /// </summary>
        public int MaxJobs { get; set; } = int.MaxValue;

        /// <summary>
        /// Gets or sets the list of systems that can be next to this system, and the probability that a job is passed
        /// to each one.
        /// </summary>
        public List<Tuple<System, double>> FollowupSystems { get; set; } = new List<Tuple<System, double>>();

        /// <summary>
        /// Gets or sets an array containing departures for each processor.
        /// </summary>
        public double[] Departures { get; set; }

        /// <summary>
        /// Gets or sets the name of the system
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the amount of jobs in the system.
        /// </summary>
        public int Jobs { get; set; } = 0;

        /// <summary>
        /// Gets or sets the summation of delta*jobs
        /// </summary>
        public double Summation { get; set; } = 0;

        /// <summary>
        /// Gets or sets the amount of completed jobs in system.
        /// </summary>
        public double CompletedJobs { get; set; } = 0;

        /// <summary>
        /// Gets or sets the accumulated busy time
        /// </summary>
        public double BusyTime { get; set; } = 0;

        /// <summary>
        /// Gets or sets the amount of jobs rejected by the system.
        /// </summary>
        public int RejectedJobs { get; set; } = 0;
    }
}