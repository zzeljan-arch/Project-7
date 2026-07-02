using System;
using PG.Systems;

namespace PG
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var seed = args.Length > 0 && int.TryParse(args[0], out var parsed) ? parsed : 12345;
            var manager = new PrototypeGameManager(seed);
            manager.StartSession();

            Console.WriteLine("=== Minimal Prototype Run ===");
            Console.WriteLine($"Seed: {seed}");
            Console.WriteLine($"Region: {manager.CurrentState.CurrentRegion.RegionId}");
            Console.WriteLine($"Path: {manager.CurrentState.CurrentRegion.CurrentPath}");
            Console.WriteLine($"Tier: {manager.CurrentState.CurrentRegion.CurrentTier}");
            Console.WriteLine($"Enemies spawned: {manager.CurrentState.ActiveEnemies.Count}");
            Console.WriteLine($"Active quests: {manager.CurrentState.ActiveQuests.Count}");
            Console.WriteLine();

            while (manager.CurrentState.ActiveEnemies.Count > 0)
            {
                var enemy = manager.CurrentState.ActiveEnemies[0];
                Console.WriteLine($"Attacking enemy: {enemy.Archetype.ArchetypeName} (HP: {enemy.CurrentHealth})");
                manager.DamageEnemy(enemy.Id, enemy.CurrentHealth + 1);
                Console.WriteLine("Enemy killed.");
                Console.WriteLine($"Loot generated: {manager.CurrentState.ActiveLoot.Count}");
                Console.WriteLine();
            }

            manager.Tick(0.016f);

            Console.WriteLine("Final prototype state:");
            Console.WriteLine($"Remaining enemies: {manager.CurrentState.ActiveEnemies.Count}");
            Console.WriteLine($"Total loot items: {manager.CurrentState.ActiveLoot.Count}");
            Console.WriteLine($"Play time: {manager.CurrentState.PlayTimeSeconds:F3}s");

            foreach (var quest in manager.CurrentState.ActiveQuests)
            {
                Console.WriteLine($"Quest: {quest.Title} - State={quest.State}");
                if (quest.Objective != null)
                {
                    Console.WriteLine($"  Progress: {quest.Objective.CurrentCount}/{quest.Objective.TargetCount}");
                }
            }

            if (manager.CurrentState.ActiveLoot.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Loot details:");
                foreach (var item in manager.CurrentState.ActiveLoot)
                {
                    Console.WriteLine($" - {item.DisplayName} ({item.Rarity}) Value={item.Value}");
                }
            }
        }
    }
}
