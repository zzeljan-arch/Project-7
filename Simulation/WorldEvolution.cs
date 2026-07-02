using System;
using System.Collections.Generic;
using System.Linq;

namespace PG.World.Simulation
{
    /// <summary>
    /// Represents a world tier level (1-5).
    /// </summary>
    public enum WorldTier
    {
        Tier1 = 1,
        Tier2 = 2,
        Tier3 = 3,
        Tier4 = 4,
        Tier5 = 5
    }

    /// <summary>
    /// Unique identifier for a region.
    /// </summary>
    public enum RegionId
    {
        NorthernRealm = 0,
        DarkForest = 1,
        DesertKingdom = 2,
        Swamp = 3,
        Highlands = 4,
        InfernalLands = 5,
        ArcaneEmpire = 6
    }

    /// <summary>
    /// Evolution paths available per region (4 per region).
    /// </summary>
    public enum EvolutionPath
    {
        // Northern Realm
        AgeOfRaiders = 0,
        EternalWinter = 1,
        Ragnarok = 2,
        UndeadNorth = 3,

        // Dark Forest
        PrimordialJungle = 4,
        CorruptedAbyss = 5,
        ElvenEnclave = 6,
        CursedWildlands = 7,

        // Desert Kingdom
        AgeOfMerchants = 8,
        AgeOfPharaohs = 9,
        ElementalCatastrophe = 10,
        ShadowAndSecrets = 11,

        // Swamp
        PrimordialBog = 12,
        PlaguePestilence = 13,
        MonsterDominion = 14,
        MysticalAwakening = 15,

        // Highlands
        DwarvenProsperity = 16,
        AsceticHermitage = 17,
        DragonRoost = 18,
        CorruptionBlight = 19,

        // Infernal Lands
        DemonicDominion = 20,
        VolcanicCatastrophe = 21,
        DemonicElementalFusion = 22,
        EternalForge = 23,

        // Arcane Empire
        ArcaneAscendance = 24,
        KnowledgeHoarding = 25,
        TechnoMagicalSynthesis = 26,
        CorruptionDecay = 27
    }

    /// <summary>
    /// Represents a single region's state in the world.
    /// </summary>
    public class RegionState
    {
        public RegionId RegionId { get; set; }
        public WorldTier CurrentTier { get; set; }
        public EvolutionPath CurrentPath { get; set; }

        public RegionState(RegionId regionId, WorldTier tier, EvolutionPath path)
        {
            RegionId = regionId;
            CurrentTier = tier;
            CurrentPath = path;
        }

        public override string ToString()
        {
            return $"{RegionId} - {CurrentPath} (Tier {(int)CurrentTier})";
        }
    }

    /// <summary>
    /// Deterministic random number generator for reproducible world generation.
    /// Uses a simple linear congruential generator (LCG) for consistency across platforms.
    /// </summary>
    public class DeterministicRandom
    {
        private ulong _seed;
        private const ulong A = 6364136223846793005UL;
        private const ulong C = 1442695040888963407UL;

        public DeterministicRandom(ulong seed = 0)
        {
            _seed = seed != 0 ? seed : (ulong)Environment.TickCount;
        }

        /// <summary>
        /// Returns a random integer in the range [0, max).
        /// </summary>
        public int Next(int max)
        {
            if (max <= 0)
                throw new ArgumentException("max must be positive");

            _seed = A * _seed + C;
            return (int)((_seed >> 33) % (uint)max);
        }

        /// <summary>
        /// Returns a random integer in the range [min, max).
        /// </summary>
        public int Next(int min, int max)
        {
            if (min >= max)
                throw new ArgumentException("min must be less than max");
            return min + Next(max - min);
        }

        /// <summary>
        /// Returns true with probability p (0.0 to 1.0).
        /// </summary>
        public bool Probability(double p)
        {
            if (p < 0 || p > 1)
                throw new ArgumentException("Probability must be between 0 and 1");
            return Next(10000) < (p * 10000);
        }
    }

    /// <summary>
    /// Encapsulates evolution path assignment logic and tier spreading rules.
    /// </summary>
    public class EvolutionRules
    {
        /// <summary>
        /// Get the available paths for a given region.
        /// </summary>
        public static List<EvolutionPath> GetPathsForRegion(RegionId regionId)
        {
            return regionId switch
            {
                RegionId.NorthernRealm => new List<EvolutionPath>
                {
                    EvolutionPath.AgeOfRaiders,
                    EvolutionPath.EternalWinter,
                    EvolutionPath.Ragnarok,
                    EvolutionPath.UndeadNorth
                },
                RegionId.DarkForest => new List<EvolutionPath>
                {
                    EvolutionPath.PrimordialJungle,
                    EvolutionPath.CorruptedAbyss,
                    EvolutionPath.ElvenEnclave,
                    EvolutionPath.CursedWildlands
                },
                RegionId.DesertKingdom => new List<EvolutionPath>
                {
                    EvolutionPath.AgeOfMerchants,
                    EvolutionPath.AgeOfPharaohs,
                    EvolutionPath.ElementalCatastrophe,
                    EvolutionPath.ShadowAndSecrets
                },
                RegionId.Swamp => new List<EvolutionPath>
                {
                    EvolutionPath.PrimordialBog,
                    EvolutionPath.PlaguePestilence,
                    EvolutionPath.MonsterDominion,
                    EvolutionPath.MysticalAwakening
                },
                RegionId.Highlands => new List<EvolutionPath>
                {
                    EvolutionPath.DwarvenProsperity,
                    EvolutionPath.AsceticHermitage,
                    EvolutionPath.DragonRoost,
                    EvolutionPath.CorruptionBlight
                },
                RegionId.InfernalLands => new List<EvolutionPath>
                {
                    EvolutionPath.DemonicDominion,
                    EvolutionPath.VolcanicCatastrophe,
                    EvolutionPath.DemonicElementalFusion,
                    EvolutionPath.EternalForge
                },
                RegionId.ArcaneEmpire => new List<EvolutionPath>
                {
                    EvolutionPath.ArcaneAscendance,
                    EvolutionPath.KnowledgeHoarding,
                    EvolutionPath.TechnoMagicalSynthesis,
                    EvolutionPath.CorruptionDecay
                },
                _ => throw new ArgumentException("Unknown region")
            };
        }

        /// <summary>
        /// Calculate distance between two regions (grid-based).
        /// </summary>
        public static int CalculateDistance(RegionId from, RegionId to)
        {
            if (from == to)
                return 0;

            // Simple grid layout (can be customized):
            // 0=North, 1=Forest, 2=Desert, 3=Swamp, 4=Highlands, 5=Infernal, 6=Arcane
            // Arrange in a rough circle/hexagon pattern
            var adjacencies = new Dictionary<RegionId, List<(RegionId, int)>>
            {
                { RegionId.NorthernRealm, new() { (RegionId.DarkForest, 1), (RegionId.Highlands, 1), (RegionId.ArcaneEmpire, 2) } },
                { RegionId.DarkForest, new() { (RegionId.NorthernRealm, 1), (RegionId.Swamp, 1), (RegionId.ArcaneEmpire, 1) } },
                { RegionId.DesertKingdom, new() { (RegionId.Swamp, 1), (RegionId.Highlands, 2), (RegionId.InfernalLands, 2) } },
                { RegionId.Swamp, new() { (RegionId.DarkForest, 1), (RegionId.DesertKingdom, 1), (RegionId.Highlands, 1) } },
                { RegionId.Highlands, new() { (RegionId.NorthernRealm, 1), (RegionId.Swamp, 1), (RegionId.DesertKingdom, 2), (RegionId.InfernalLands, 1) } },
                { RegionId.InfernalLands, new() { (RegionId.Highlands, 1), (RegionId.DesertKingdom, 2), (RegionId.ArcaneEmpire, 2) } },
                { RegionId.ArcaneEmpire, new() { (RegionId.NorthernRealm, 2), (RegionId.DarkForest, 1), (RegionId.InfernalLands, 2) } }
            };

            // BFS to find shortest distance
            var queue = new Queue<(RegionId, int)>();
            var visited = new HashSet<RegionId> { from };
            queue.Enqueue((from, 0));

            while (queue.Count > 0)
            {
                var (current, dist) = queue.Dequeue();
                if (current == to)
                    return dist;

                if (adjacencies.ContainsKey(current))
                {
                    foreach (var (neighbor, _) in adjacencies[current])
                    {
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            queue.Enqueue((neighbor, dist + 1));
                        }
                    }
                }
            }

            return int.MaxValue; // No path found
        }

        /// <summary>
        /// Simulate tier spread when a region evolves.
        /// Only some adjacent regions tier up; others remain unchanged.
        /// </summary>
        public static Dictionary<RegionId, int> CalculateTierSpread(RegionId clearedRegion, DeterministicRandom rng)
        {
            var tierChanges = new Dictionary<RegionId, int>();

            // All regions except the cleared one
            foreach (RegionId region in Enum.GetValues(typeof(RegionId)))
            {
                if (region == clearedRegion)
                    continue;

                int distance = CalculateDistance(clearedRegion, region);

                // Probability decreases with distance
                double probability = distance switch
                {
                    1 => 0.60, // Adjacent: 60% chance to tier up
                    2 => 0.35, // Two steps away: 35% chance
                    3 => 0.15, // Three steps: 15% chance
                    _ => 0.05  // Far away: 5% chance
                };

                if (rng.Probability(probability))
                {
                    // Possible tier increases: +1 or +2
                    tierChanges[region] = rng.Probability(0.7) ? 1 : 2;
                }
            }

            return tierChanges;
        }
    }

    /// <summary>
    /// Represents a complete world state and history of one campaign.
    /// </summary>
    public class WorldCampaign
    {
        public ulong Seed { get; private set; }
        public RegionId StartingRegion { get; private set; }
        public List<RegionState> Regions { get; private set; } = new();
        public List<string> EventLog { get; private set; } = new();
        public Dictionary<RegionId, List<(WorldTier, EvolutionPath)>> RegionHistory { get; private set; } = new();

        /// <summary>
        /// Initialize a new campaign with a given seed and starting region.
        /// </summary>
        public WorldCampaign(ulong seed, RegionId startingRegion)
        {
            Seed = seed;
            StartingRegion = startingRegion;

            var rng = new DeterministicRandom(seed);

            // Initialize all regions
            foreach (RegionId region in Enum.GetValues(typeof(RegionId)))
            {
                WorldTier tier = region == startingRegion ? WorldTier.Tier1 : WorldTier.Tier2;
                if (region != startingRegion)
                {
                    // Non-starting regions tier up based on distance
                    int distance = EvolutionRules.CalculateDistance(startingRegion, region);
                    tier = distance switch
                    {
                        1 => WorldTier.Tier2,
                        2 => WorldTier.Tier3,
                        _ => WorldTier.Tier4
                    };
                }

                // Assign random path for this region
                var availablePaths = EvolutionRules.GetPathsForRegion(region);
                EvolutionPath path = availablePaths[rng.Next(availablePaths.Count)];

                var regionState = new RegionState(region, tier, path);
                Regions.Add(regionState);

                // Track history
                RegionHistory[region] = new List<(WorldTier, EvolutionPath)> { (tier, path) };

                EventLog.Add($"[Init] {region} initialized at {tier}, Path: {path}");
            }
        }

        /// <summary>
        /// Simulate clearing a region and advancing the world state.
        /// </summary>
        public void ClearRegion(RegionId region)
        {
            var regionState = Regions.First(r => r.RegionId == region);
            EventLog.Add($"[Clear] {region} cleared at {regionState.CurrentTier}");

            var rng = new DeterministicRandom(Seed + (ulong)Regions.Count);

            // Apply tier spread
            var tierChanges = EvolutionRules.CalculateTierSpread(region, rng);

            foreach (var (affectedRegion, tierIncrease) in tierChanges)
            {
                var affectedState = Regions.First(r => r.RegionId == affectedRegion);
                var oldTier = affectedState.CurrentTier;
                affectedState.CurrentTier = (WorldTier)Math.Min(5, (int)affectedState.CurrentTier + tierIncrease);
                EventLog.Add($"[Spread] {affectedRegion}: {oldTier} -> {affectedState.CurrentTier}");

                // Record history
                RegionHistory[affectedRegion].Add((affectedState.CurrentTier, affectedState.CurrentPath));
            }
        }

        public override string ToString()
        {
            return $"Campaign (Seed: {Seed}, Starting: {StartingRegion})\n" +
                   string.Join("\n", Regions.Select(r => $"  {r}"));
        }
    }

    /// <summary>
    /// Tracks novelty metrics across multiple campaigns to measure replayability.
    /// </summary>
    public class NoveltyTracker
    {
        public List<WorldCampaign> Campaigns { get; private set; } = new();
        public Dictionary<string, int> PathOccurrences { get; private set; } = new();
        public Dictionary<(RegionId, EvolutionPath), int> RegionPathOccurrences { get; private set; } = new();

        public void AddCampaign(WorldCampaign campaign)
        {
            Campaigns.Add(campaign);

            foreach (var region in campaign.Regions)
            {
                string pathKey = $"{region.RegionId}_{region.CurrentPath}";
                if (!PathOccurrences.ContainsKey(pathKey))
                    PathOccurrences[pathKey] = 0;
                PathOccurrences[pathKey]++;

                var key = (region.RegionId, region.CurrentPath);
                if (!RegionPathOccurrences.ContainsKey(key))
                    RegionPathOccurrences[key] = 0;
                RegionPathOccurrences[key]++;
            }
        }

        /// <summary>
        /// Calculate novelty score for a new campaign compared to previous campaigns.
        /// Returns a value 0-1 where 1 = completely novel.
        /// </summary>
        public double CalculateNovelty(WorldCampaign newCampaign)
        {
            if (Campaigns.Count == 0)
                return 1.0;

            double noveltyScore = 0;
            int comparisonCount = 0;

            foreach (var region in newCampaign.Regions)
            {
                string pathKey = $"{region.RegionId}_{region.CurrentPath}";
                int occurrences = PathOccurrences.ContainsKey(pathKey) ? PathOccurrences[pathKey] : 0;

                // Normalized novelty: regions that have appeared rarely are more novel
                double regionNovelty = 1.0 - (Math.Min(occurrences, Campaigns.Count) / (double)Campaigns.Count);
                noveltyScore += regionNovelty;
                comparisonCount++;
            }

            return comparisonCount > 0 ? noveltyScore / comparisonCount : 1.0;
        }

        /// <summary>
        /// Generate a summary report of novelty metrics.
        /// </summary>
        public string GenerateReport()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"=== Novelty Report ({Campaigns.Count} campaigns) ===\n");

            sb.AppendLine("Path Occurrence Distribution:");
            var sortedPaths = PathOccurrences.OrderByDescending(kvp => kvp.Value);
            foreach (var (pathKey, count) in sortedPaths)
            {
                double frequency = count / (double)Campaigns.Count;
                sb.AppendLine($"  {pathKey}: {count} occurrences ({frequency:P1})");
            }

            sb.AppendLine($"\nUnique Paths Generated: {PathOccurrences.Count} / 28 possible");

            // Calculate variance
            var occurrences = PathOccurrences.Values.ToList();
            if (occurrences.Count > 0)
            {
                double mean = occurrences.Average();
                double variance = occurrences.Sum(x => Math.Pow(x - mean, 2)) / occurrences.Count;
                double stdDev = Math.Sqrt(variance);
                sb.AppendLine($"Occurrence Std Dev: {stdDev:F2} (lower = more balanced)");
            }

            return sb.ToString();
        }
    }
}
