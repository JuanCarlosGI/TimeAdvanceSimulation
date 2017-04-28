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
            //Problem2();
            Problem3();
        }

        public static void Problem2()
        {
            var systemA = new System(6)
            {
                ServiceTime = 2.5,
                MaxJobs = 40
            };

            var results = new Simulator(new SimulationConfiguration
            {
                ArrivalRate = 5.5,
                ClockEnd = 10000000,
                EntryPoint = systemA,
                TrackedSystems = new List<System> { systemA },
                //Verbose = true
            }).RunSimulation();

            foreach (var result in results)
            {
                Console.WriteLine(result.System.Name);
                Console.WriteLine($"N = {result.N}");
                Console.WriteLine($"U = {result.U}");
                Console.WriteLine($"R = {result.R}");
                Console.WriteLine($"X = {result.X}\n");
            }
        }

        public static void Problem3()
        {
            var systemB = new System(1)
            {
                ServiceTime = 1 / 12.0,
                Name = "B"
            };

            var systemC = new System(1)
            {
                ServiceTime = 1 / 7.0,
                Name = "C"
            };

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

            var results = new Simulator(new SimulationConfiguration
            {
                ArrivalRate = 1 / 10.0,
                ClockEnd = 100000,
                EntryPoint = systemA,
                TrackedSystems = new List<System> { systemA, systemB, systemC },
                //Verbose = true
            }).RunSimulation();

            foreach (var result in results)
            {
                Console.WriteLine(result.System.Name);
                Console.WriteLine($"N = {result.N}");
                Console.WriteLine($"U = {result.U}");
                Console.WriteLine($"R = {result.R}");
                Console.WriteLine($"X = {result.X}\n");
            }
        }
    }
}
