using System;
using System.Collections.Generic;
using PG.World.Simulation;

namespace PG.World.Simulation
{
    /// <summary>
    /// Main entry point for running campaign simulations.
    /// </summary>
    public class SimulationRunner
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Procedural World Evolution Simulator ===\n");

            // Run a single detailed campaign
            Console.WriteLine("--- Single Campaign (Detailed) ---");
            RunSingleCampaign(42);

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // Run batch of campaigns and analyze novelty
            Console.WriteLine("--- Batch Campaign Analysis (20 runs) ---");
            RunBatchAnalysis(20, startingSeed: 1000);
        }

        /// <summary>
        /// Run a single campaign with detailed output.
        /// </summary>
        private static void RunSingleCampaign(ulong seed)
        {
            var campaign = new WorldCampaign(seed, RegionId.NorthernRealm);
            Console.WriteLine($"Initial World State:\n{campaign}\n");

            // Simulate player actions in a few regions
            Console.WriteLine("Simulating player actions...\n");
            campaign.PerformPlayerAction(RegionId.NorthernRealm, PlayerAction.Defend);
            Console.WriteLine($"After action in Northern Realm:\n{campaign}\n");

            campaign.PerformPlayerAction(RegionId.DarkForest, PlayerAction.Explore);
            Console.WriteLine($"After action in Dark Forest:\n{campaign}\n");

            Console.WriteLine("Event Log:");
            foreach (var evt in campaign.EventLog)
            {
                Console.WriteLine($"  {evt}");
            }
        }

        /// <summary>
        /// Run multiple campaigns and analyze novelty metrics.
        /// </summary>
        private static void RunBatchAnalysis(int numCampaigns, ulong startingSeed)
        {
            var tracker = new NoveltyTracker();

            Console.WriteLine($"Running {numCampaigns} campaigns...\n");

            for (int i = 0; i < numCampaigns; i++)
            {
                ulong seed = startingSeed + (ulong)i;

                // Randomly choose starting region
                var rng = new DeterministicRandom(seed);
                int regionChoice = rng.Next(7);
                RegionId startingRegion = (RegionId)regionChoice;

                var campaign = new WorldCampaign(seed, startingRegion);

                // Simulate a few player actions
                var actions = new[] { PlayerAction.Purify, PlayerAction.Raid, PlayerAction.Research, PlayerAction.Negotiate, PlayerAction.Corrupt, PlayerAction.Explore, PlayerAction.Defend };
                for (int j = 0; j < 2; j++)
                {
                    int regionToAct = rng.Next(7);
                    var action = actions[rng.Next(actions.Length)];
                    campaign.PerformPlayerAction((RegionId)regionToAct, action);
                }

                tracker.AddCampaign(campaign);
                double novelty = tracker.CalculateNovelty(campaign);
                Console.WriteLine($"Campaign {i + 1}: Starting={startingRegion}, Novelty={novelty:P1}");
            }

            Console.WriteLine("\n" + tracker.GenerateReport());
        }
    }
}
