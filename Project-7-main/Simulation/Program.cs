using System;
using System.Collections.Generic;
using System.Linq;
using PG.World.Simulation;

namespace PG.World.Simulation
{
    public class SimulationRunner
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Procedural World Evolution Simulator ===\n");

            if (args.Length > 0 && args[0].Equals("fast", StringComparison.OrdinalIgnoreCase))
            {
                int campaigns = SimulationConfig.FastSimulationDefaultCampaigns;
                int actions = SimulationConfig.FastSimulationDefaultActions;
                bool questMode = false;

                if (args.Length > 1 && int.TryParse(args[1], out var parsedCampaigns))
                    campaigns = Math.Max(1, parsedCampaigns);
                if (args.Length > 2 && int.TryParse(args[2], out var parsedActions))
                    actions = Math.Max(1, parsedActions);
                if (args.Length > 3 && args[3].Equals("quest", StringComparison.OrdinalIgnoreCase))
                    questMode = true;

                RunFastSimulation(campaigns, actions, startingSeed: 1000, questMode: questMode);
                return;
            }

            RunInteractiveCampaign(seed: 42, startingRegion: RegionId.NorthernRealm);
        }

        private static void RunInteractiveCampaign(ulong seed, RegionId startingRegion)
        {
            var world = WorldState.CreateCampaign(seed, startingRegion);
            Console.WriteLine("Starting interactive campaign. Type 'help' for commands.\n");

            while (true)
            {
                Console.WriteLine(world);
                Console.Write("Action> ");
                var input = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (string.IsNullOrEmpty(input))
                    continue;

                if (input == "quit" || input == "exit")
                {
                    Console.WriteLine("Exiting interactive campaign.");
                    break;
                }

                if (input == "help")
                {
                    PrintHelp();
                    continue;
                }

                if (input == "explore")
                {
                    world.Explore();
                }
                else if (input == "rest")
                {
                    world.Rest();
                }
                else if (input == "town")
                {
                    world.VisitTown();
                }
                else if (input == "market")
                {
                    world.VisitMarketplace();
                }
                else if (input == "inventory")
                {
                    world.InventorySummary();
                }
                else if (input == "character")
                {
                    world.CharacterSummary();
                }
                else if (input == "quests")
                {
                    world.QuestLogSummary();
                }
                else if (input == "turnin")
                {
                    if (world.TurnInQuest())
                    {
                        Console.WriteLine("Quest turned in.");
                    }
                    else
                    {
                        Console.WriteLine("No completed quest found to turn in.");
                    }
                }
                else if (input == "accept")
                {
                    if (world.AcceptQuest())
                    {
                        Console.WriteLine("Quest accepted.");
                    }
                    else
                    {
                        Console.WriteLine("No available quest found.");
                    }
                }
                else if (input == "read")
                {
                    world.ReadBook();
                }
                else if (input == "fish")
                {
                    world.Fish();
                }
                else if (input == "camp")
                {
                    world.Camp();
                }
                else if (input == "craft")
                {
                    world.Craft();
                }
                else if (input == "travel")
                {
                    HandleTravel(world);
                }
                else if (input == "status")
                {
                    world.CharacterSummary();
                }
                else if (input == "history")
                {
                    PrintHistory(world);
                }
                else
                {
                    Console.WriteLine("Unknown command. Type 'help' for a list of supported commands.");
                }

                if (world.Player.Health <= 0)
                {
                    Console.WriteLine("You have fallen in the wilds. Campaign over.");
                    break;
                }
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  explore    - Venture into the region and potentially discover settlements or trigger encounters");
            Console.WriteLine("  rest       - Recover health and advance time");
            Console.WriteLine("  town       - Fast travel to a discovered settlement (must explore to discover)");
            Console.WriteLine("  market     - Browse market prices and buy available items");
            Console.WriteLine("  inventory  - Show your current inventory and gold");
            Console.WriteLine("  character  - Show player health and reputation");
            Console.WriteLine("  quests     - Show active and available quests");
            Console.WriteLine("  accept     - Accept a quest from the current town");
            Console.WriteLine("  turnin     - Turn in a completed quest for rewards");
            Console.WriteLine("  read       - Study a book to gain arcane reputation");
            Console.WriteLine("  fish       - Fish if water is available in your region");
            Console.WriteLine("  camp       - Camp for the night to restore health");
            Console.WriteLine("  craft      - Attempt to craft using inventory materials");
            Console.WriteLine("  travel     - Move to another available region");
            Console.WriteLine("  history    - Print the world event history");
            Console.WriteLine("  help       - Show this help text");
            Console.WriteLine("  quit       - Exit the interactive campaign");
        }

        private static void HandleTravel(WorldState world)
        {
            var available = world.Regions.Keys.ToList();
            Console.WriteLine("Available regions:");
            for (int i = 0; i < available.Count; i++)
            {
                Console.WriteLine($"  {i + 1}: {available[i]}");
            }

            Console.Write("Destination number> ");
            var selection = Console.ReadLine();
            if (!int.TryParse(selection, out var index) || index < 1 || index > available.Count)
            {
                Console.WriteLine("Invalid region selection.");
                return;
            }

            var destination = available[index - 1];
            world.Travel(destination);
        }

        private static void PrintHistory(WorldState world)
        {
            Console.WriteLine("-- World Event History --");
            foreach (var entry in world.SimulationHistory)
            {
                Console.WriteLine(entry);
            }
        }

        private static void RunFastSimulation(int numCampaigns, int actionsPerCampaign, ulong startingSeed, bool questMode = false)
        {
            var tracker = new NoveltyTracker();
            var modeLabel = questMode ? "quest-focused" : "standard";
            Console.WriteLine($"Running fast simulation: {numCampaigns} campaigns with {actionsPerCampaign} actions each ({modeLabel} mode)...\n");

            for (int i = 0; i < numCampaigns; i++)
            {
                ulong seed = startingSeed + (ulong)i;
                var world = WorldState.CreateCampaign(seed, RegionId.NorthernRealm);
                world.IsFastMode = true;
                world.ShowLogs = true;
                world.AutoResolveEncounters = !questMode;

                Console.WriteLine($"\n=== Campaign {i + 1} (Seed: {seed}) ===");
                for (int j = 0; j < actionsPerCampaign; j++)
                {
                    if (world.Player.Health <= 0)
                    {
                        Console.WriteLine($"[CAMPAIGN END] Player died after {j} actions.");
                        break;
                    }
                    world.PerformAutomaticAction(questMode);
                }

                tracker.AddCampaign(world);
                var novelty = tracker.CalculateNovelty(world);
                Console.WriteLine($"\n--- Campaign {i + 1} Summary: Gold={world.Player.Gold}, Health={world.Player.Health}/{world.Player.MaxHealth}, Novelty={novelty:P1} ---\n");
            }

            Console.WriteLine("\n" + tracker.GenerateReport());
        }
    }
}
