using System.Collections.Generic;

namespace PG.World.Simulation
{
    public class QuestLineState
    {
        public QuestLineDefinition Definition { get; set; }
        public int CurrentQuestIndex { get; set; }
        public bool Completed { get; set; }
        public List<string> CompletedQuestIds { get; set; } = new();

        public QuestLineState(QuestLineDefinition definition)
        {
            Definition = definition;
            CurrentQuestIndex = 0;
            Completed = false;
        }

        public string GetCurrentQuestId()
        {
            if (CurrentQuestIndex < Definition.QuestIds.Count)
                return Definition.QuestIds[CurrentQuestIndex];
            return null;
        }

        public void AdvanceQuest()
        {
            if (!Completed)
                CurrentQuestIndex++;
            if (CurrentQuestIndex >= Definition.QuestIds.Count)
                Completed = true;
        }
    }

    public class SettlementState
    {
        public string Id { get; set; }
        public SettlementDefinition Definition { get; set; }
        public bool Discovered { get; set; }
        public double Population { get; set; }
        public double Wealth { get; set; }
        public double Safety { get; set; }
        public double MerchantActivity { get; set; }
        public Dictionary<string, SettlementBuilding> Buildings { get; set; } = new();
        public List<string> AvailableQuestLineIds { get; set; } = new();
        public Dictionary<FactionType, double> Reputation { get; set; } = new();
        public List<string> ConnectedSettlementIds { get; set; } = new();

        public SettlementState(SettlementDefinition definition)
        {
            Id = definition.Id;
            Definition = definition;
            Discovered = false;
            Population = definition.PopulationStart;
            Wealth = definition.WealthStart;
            Safety = definition.SafetyStart;
            MerchantActivity = 0;

            foreach (var buildingId in definition.InitialBuildings)
            {
                Buildings[buildingId] = new SettlementBuilding { Id = buildingId, Name = buildingId, Active = true, Level = 1 };
            }

            AvailableQuestLineIds = new List<string>(definition.InitialQuestLineIds);
        }

        public void Discover()
        {
            Discovered = true;
        }

        public void ModifyWealth(double amount)
        {
            Wealth = System.Math.Max(0, System.Math.Min(100, Wealth + amount));
        }

        public void ModifySafety(double amount)
        {
            Safety = System.Math.Max(0, System.Math.Min(100, Safety + amount));
        }

        public void ModifyMerchantActivity(double amount)
        {
            MerchantActivity = System.Math.Max(0, System.Math.Min(100, MerchantActivity + amount));
        }
    }
}
