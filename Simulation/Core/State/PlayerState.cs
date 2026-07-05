using System.Collections.Generic;

namespace PG.World.Simulation
{
    public class PlayerState
    {
        public RegionId CurrentRegion { get; set; }
        public double Health { get; set; }
        public double MaxHealth { get; set; }
        public double Gold { get; set; }
        public Dictionary<string, int> Inventory { get; set; } = new();
        public Dictionary<FactionType, double> Reputation { get; set; } = new();
        public List<QuestLineState> ActiveQuestLines { get; set; } = new();
        public List<string> DiscoveredSettlementIds { get; set; } = new();

        public void Heal(double amount)
        {
            Health = System.Math.Min(Health + amount, MaxHealth);
        }

        public double TakeDamage(double damage)
        {
            Health = System.Math.Max(0, Health - damage);
            return Health;
        }

        public bool SpendGold(double amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        public void AddItem(string itemId, int quantity)
        {
            if (Inventory.TryGetValue(itemId, out var existing))
                Inventory[itemId] = existing + quantity;
            else
                Inventory[itemId] = quantity;
        }

        public bool RemoveItem(string itemId, int quantity)
        {
            if (Inventory.TryGetValue(itemId, out var existing) && existing >= quantity)
            {
                Inventory[itemId] = existing - quantity;
                if (Inventory[itemId] == 0)
                    Inventory.Remove(itemId);
                return true;
            }
            return false;
        }

        public void ModifyReputation(FactionType faction, double amount)
        {
            if (Reputation.TryGetValue(faction, out var current))
                Reputation[faction] = current + amount;
            else
                Reputation[faction] = amount;
        }
    }
}
