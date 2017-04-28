using System;
using System.Collections.Generic;
using System.Linq;
using C5;

namespace TimeAdvanceSimulation
{
    /// <summary>
    /// Class in charge of simulating a system running jobs.
    /// </summary>
    public class Simulator
    {
        /// <summary>
        /// Shared instance of Random
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Configuration for the simulator
        /// </summary>
        private readonly SimulationConfiguration _config;

        /// <summary>
        /// Current clock.
        /// </summary>
        private double _clock;

        /// <summary>
        /// Time of last event.
        /// </summary>
        private double _lastEvent;

        /// <summary>
        /// Priority queue in which events reside.
        /// </summary>
        private IntervalHeap<Event> _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulator"/> class.
        /// </summary>
        /// <param name="config">Configurations for the simulator.</param>
        public Simulator(SimulationConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Runs a simulation given its configuration.
        /// </summary>
        /// <returns>SimulationResults for the simulation.</returns>
        public List<SimulationResult> RunSimulation()
        {
            Initialize();

            while (_clock < _config.ClockEnd)
            {
                // Get next event
                var @event = _queue.FindMin();
                _queue.DeleteMin();

                // Advance clock.
                _clock = @event.ScheduledTime;

                // Take statistics before event
                var delta = _clock - _lastEvent;
                foreach (var system in _config.TrackedSystems)
                {
                    //  Update summation
                    system.Summation += system.Jobs * delta;
                }

                // Execute event
                Execute(@event);

                // Take statistics after event.
                _lastEvent = _clock;
            }

            // Calculate and return results.
            var observationTime = _clock - _config.ClockStart;
            return _config.TrackedSystems.Select(system => new SimulationResult
                {
                    System = system,
                    N = system.Summation / observationTime,
                    U = system.BusyTime / observationTime / system.Processors,
                    R = system.Summation / system.CompletedJobs,
                    X = system.CompletedJobs / observationTime
                }).ToList();
        }

        public void Execute(Event e)
        {
            switch (e.Type)
            {
                case EventType.Arrival:
                    ExecuteArrival(e);
                    break;
                case EventType.Departure:
                    ExecuteDeparture(e);
                    break;
                default:
                    throw new InvalidOperationException("This event type is not supported yet.");
            }
        }

        /// <summary>
        /// Executes an arrival event
        /// </summary>
        /// <param name="arrival">The event</param>
        public void ExecuteArrival(Event arrival)
        {
            if (arrival.System.Jobs < arrival.System.MaxJobs)
            {
                arrival.System.Jobs++;

                if (_config.Verbose)
                {
                    Console.WriteLine($"Arrival\t\tSystem:{arrival.System.Name}\t\t\t\tClock:{_clock}\tJobs:{arrival.System.Jobs}");
                }

                // Only schedule next arrival if its the entry point
                if (arrival.System == _config.EntryPoint)
                {
                    ScheduleNextArrival();
                }

                // Check if there are free processors
                if (arrival.System.Jobs <= arrival.System.Processors)
                {
                    ScheduleNextDeparture(arrival.System);
                }
            }
            else
            {
                if (_config.Verbose)
                {
                    Console.WriteLine($"Arrival rejected by system {arrival.System.Name}");
                }

                if (arrival.System == _config.EntryPoint)
                {
                    ScheduleNextArrival();
                }
            }
        }

        /// <summary>
        /// Executes a departure event.
        /// </summary>
        /// <param name="departure">The event</param>
        public void ExecuteDeparture(Event departure)
        {
            departure.System.Jobs--;
            departure.System.CompletedJobs++;

            // Find processor that finished
            var processor = -1;
            for (var proc = 0; proc < departure.System.Processors; proc++)
            {
                if (departure.System.Departures[proc] != _clock) continue;
                processor = proc;
                break;
            }

            // Deactivate processor
            departure.System.Departures[processor] = double.MaxValue;

            if (_config.Verbose)
            {
                Console.WriteLine($"Departure\tSystem:{departure.System.Name}\t\tProcessor:{processor}\tClock:{_clock}\tJobs:{departure.System.Jobs}");
            }

            // Pass on to next system.
            if (departure.System.FollowupSystems.Count > 0)
            {
                // Choose followup system.
                System followupSystem = null;
                double restProb = 1;
                foreach (var tuple in departure.System.FollowupSystems)
                {
                    if (Random.NextDouble()*restProb < tuple.Item2)
                    {
                        followupSystem = tuple.Item1;
                        break;
                    }
                    restProb -= tuple.Item2;
                }
                if (followupSystem == null)
                {
                    followupSystem = departure.System.FollowupSystems.Last().Item1;
                }

                if (_config.Verbose)
                    Console.WriteLine($"\tPassed to {followupSystem.Name}, {_clock}");

                _queue.Add(new Event
                {
                    ScheduledTime = _clock,
                    Type = EventType.Arrival,
                    System = followupSystem
                });
            }

            // Check if there are pending jobs.
            if (departure.System.Jobs >= departure.System.Processors)
            {
                ScheduleNextDeparture(departure.System);
            }
        }

        /// <summary>
        /// Sets up variables to start a simulation.
        /// </summary>
        private void Initialize()
        {
            // Initialize event queue.
            _queue = new IntervalHeap<Event>();

            // Initialize clock and schedule next arrival.
            _clock = _config.ClockStart;
            _queue.Add(new Event {ScheduledTime = _clock, Type = EventType.Arrival, System = _config.EntryPoint });

            _lastEvent = 0.0;
        }

        /// <summary>
        /// Schedules the next departure in the system.
        /// </summary>
        private void ScheduleNextDeparture(System system)
        {
            // Find available processor
            var processor = -1;
            for (var proc = 0; proc < system.Processors; proc++)
            {
                if (system.Departures[proc] == double.MaxValue)
                {
                    processor = proc;
                    break;
                }
            }

            // Schedule the departure
            system.Departures[processor] = _clock - system.ServiceTime * Math.Log(Random.NextDouble());
            _queue.Add(new Event { ScheduledTime = system.Departures[processor], Type = EventType.Departure, System = system});

            if (_config.Verbose)
            {
                Console.WriteLine($"Start\t\tSystem:{system.Name}\t\tProcessor:{processor}\tClock:{_clock}\tEnd:{system.Departures[processor]}");
            }

            // Take statistics
            system.BusyTime += system.Departures[processor] - _clock;
        }

        /// <summary>
        /// Schedules the next arrival in the system.
        /// </summary>
        private void ScheduleNextArrival()
        {
            _queue.Add(new Event
            {
                ScheduledTime = _clock - _config.ArrivalRate * Math.Log(Random.NextDouble()),
                Type = EventType.Arrival,
                System = _config.EntryPoint
            });
        }
    }
}
