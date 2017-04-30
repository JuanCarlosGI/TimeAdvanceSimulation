using System;
using System.Collections.Generic;

namespace DemoProblems
{
    using TimeAdvanceSimulation;

    /// <summary>
    /// Main class running the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Runs the main program.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            //var results = Problem2();
            //var results = Problem3();
            var results = Problem4();

            foreach (var result in results)
            {
                Console.WriteLine(result.System.Name);
                Console.WriteLine($"N = {result.N}");
                Console.WriteLine($"U = {result.U}");
                Console.WriteLine($"R = {result.R}");
                Console.WriteLine($"X = {result.X}");
                Console.WriteLine($"Rejection rate: {result.RejectionRate}\n");
            }
        }

        /// <summary>
        /// Runs a simulation where a single single-processor system completes jobs.
        /// </summary>
        /// <returns>The results of the simulation</returns>
        public static List<SimulationResult> Problem2()
        {
            var systemA = new System(1) { ServiceTime = 1 / 8.0, MaxJobs = 37 };
            return new Simulator(new SimulationConfiguration
            {
                ArrivalRate = 1 / 7.0,
                ClockEnd = 10000,
                EntryPoint = systemA,
                TrackedSystems = new List<System> { systemA },
                //Verbose = true
            }).RunSimulation();
        }

        /// <summary>
        /// Runs a simulation where system A forwards jobs to either system B or system C, with probabilities of 60% 
        /// and 40%, respectively. All systems have a single processor.
        /// </summary>
        /// <returns>The results of the simulation.</returns>
        public static List<SimulationResult> Problem3()
        {
            var systemB = new System(1) {ServiceTime = 1 / 12.0, Name = "B"};
            var systemC = new System(1) {ServiceTime = 1 / 7.0, Name = "C"};
            var systemA = new System(1)
            {
                ServiceTime = 1 / 15.0,
                Name = "A",
                FollowupSystems = new List<Tuple<System, double>>
                {
                    new Tuple<System, double>(systemB, 0.6),
                    new Tuple<System, double>(systemC, 0.4)
                }
            };

            return new Simulator(new SimulationConfiguration
            {
                ArrivalRate = 1 / 10.0,
                ClockEnd = 10000,
                EntryPoint = systemA,
                TrackedSystems = new List<System> { systemA, systemB, systemC },
                //Verbose = true
            }).RunSimulation();
        }

        /// <summary>
        /// Runs a simulation where a single system with multiple processors completes jobs. 22 jobs arrive per second,
        /// and 8 jobs are completed per processor per second.
        /// </summary>
        /// <returns>The results of the simulation.</returns>
        public static List<SimulationResult> Problem4()
        {
            var systemA = new System(6) {ServiceTime = 1 / 8.0};
            return new Simulator(new SimulationConfiguration
            {
                ArrivalRate = 1 / 22.0,
                ClockEnd = 10000,
                EntryPoint = systemA,
                TrackedSystems = new List<System> { systemA },
                //Verbose = true
            }).RunSimulation();
        }
    }
}
