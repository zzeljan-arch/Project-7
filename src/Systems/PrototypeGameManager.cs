using System;
using System.Collections.Generic;
using PG.Gameplay.Combat;
using PG.World.Simulation;

namespace PG.Systems
{
    public class PrototypeGameManager
    {
        private readonly DeterministicRandom _rng;
        private readonly RegionManager _regionManager;
        private readonly EnemyDatabase _enemyDatabase;
        private readonly LootDatabase _lootDatabase;
        private readonly QuestManager _questManager;

        public GameState CurrentState { get; private set; }

        public PrototypeGameManager(int seed)
        {
            _rng = new DeterministicRandom((ulong)seed);
            _regionManager = new RegionManager(seed);
            _enemyDatabase = new EnemyDatabase();
            _lootDatabase = new LootDatabase();
            _questManager = new QuestManager();

            CurrentState = new GameState
            {
                Seed = seed,
                PlayTimeSeconds = 0,
                CurrentRegion = _regionManager.GetInitialRegion(),
                ActiveEnemies = new List<EnemyInstance>(),
                ActiveLoot = new List<ItemInstance>(),
                ActiveQuests = new List<Quest>()
            };
        }

        public void StartSession()
        {
            CurrentState.ActiveQuests.Add(_questManager.AssignInitialQuest(CurrentState.CurrentRegion));
            SpawnEncounter(3);
        }

        public void Tick(float deltaTime)
        {
            CurrentState.PlayTimeSeconds += deltaTime;
            UpdateEnemies(deltaTime);
            _questManager.UpdateQuests(CurrentState, deltaTime);

            if (CurrentState.ActiveEnemies.Count == 0)
            {
                SpawnEncounter(1);
            }
        }

        private void SpawnEncounter(int count)
        {
            var archetype = _enemyDatabase.GetArchetypeForRegion(CurrentState.CurrentRegion.RegionId, CurrentState.CurrentRegion.CurrentPath, CurrentState.CurrentRegion.CurrentTier);
            if (archetype == null)
                return;

            for (var i = 0; i < count; i++)
            {
                var tierData = archetype.TierProgression[CurrentState.CurrentRegion.CurrentTier];
                CurrentState.ActiveEnemies.Add(new EnemyInstance
                {
                    Id = Guid.NewGuid().ToString(),
                    Archetype = archetype,
                    TierData = tierData,
                    CurrentHealth = tierData.BaseHealth
                });
            }
        }

        private void UpdateEnemies(float deltaTime)
        {
            for (int i = CurrentState.ActiveEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = CurrentState.ActiveEnemies[i];
                if (enemy.CurrentHealth <= 0)
                {
                    HandleEnemyDeath(enemy);
                    CurrentState.ActiveEnemies.RemoveAt(i);
                }
            }
        }

        public void DamageEnemy(string enemyId, float damageAmount)
        {
            var enemy = CurrentState.ActiveEnemies.Find(e => e.Id == enemyId);
            if (enemy == null)
                return;

            enemy.CurrentHealth -= damageAmount;
            if (enemy.CurrentHealth <= 0)
            {
                CurrentState.ActiveEnemies.Remove(enemy);
                HandleEnemyDeath(enemy);
            }
        }

        private void HandleEnemyDeath(EnemyInstance enemy)
        {
            var loot = _lootDatabase.GenerateLoot(CurrentState.CurrentRegion, enemy.Archetype, CurrentState.CurrentRegion.CurrentTier);
            CurrentState.ActiveLoot.AddRange(loot);
            _questManager.RecordEnemyKill(CurrentState, enemy);
        }
    }

    public class GameState
    {
        public int Seed { get; set; }
        public float PlayTimeSeconds { get; set; }
        public RegionState CurrentRegion { get; set; } = null!;
        public List<EnemyInstance> ActiveEnemies { get; set; } = new();
        public List<ItemInstance> ActiveLoot { get; set; } = new();
        public List<Quest> ActiveQuests { get; set; } = new();
    }

    public class EnemyInstance
    {
        public string Id { get; set; } = string.Empty;
        public EnemyArchetype Archetype { get; set; } = null!;
        public EnemyTierData TierData { get; set; } = null!;
        public float CurrentHealth { get; set; }
    }

    public class ItemInstance
    {
        public string ItemId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public ItemRarityType Rarity { get; set; }
        public float Value { get; set; }
    }
}
