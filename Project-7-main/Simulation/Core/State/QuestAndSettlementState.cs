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
        public double Loyalty { get; set; }
        public double Faith { get; set; }
        public double Fear { get; set; }
        public double Culture { get; set; }
        public double Education { get; set; }
        public double Crime { get; set; }
        public double Hope { get; set; }
        public double Influence { get; set; }
        public double Military { get; set; }
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
            Loyalty = 50;
            Faith = 50;
            Fear = 50;
            Culture = 50;
            Education = 50;
            Crime = 50;
            Hope = 50;
            Influence = 50;
            Military = 50;

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

        public void ModifyPersonality(string trait, double amount)
        {
            switch (trait)
            {
                case "Loyalty":
                    Loyalty = System.Math.Max(0, System.Math.Min(100, Loyalty + amount));
                    break;
                case "Faith":
                    Faith = System.Math.Max(0, System.Math.Min(100, Faith + amount));
                    break;
                case "Fear":
                    Fear = System.Math.Max(0, System.Math.Min(100, Fear + amount));
                    break;
                case "Culture":
                    Culture = System.Math.Max(0, System.Math.Min(100, Culture + amount));
                    break;
                case "Education":
                    Education = System.Math.Max(0, System.Math.Min(100, Education + amount));
                    break;
                case "Crime":
                    Crime = System.Math.Max(0, System.Math.Min(100, Crime + amount));
                    break;
                case "Hope":
                    Hope = System.Math.Max(0, System.Math.Min(100, Hope + amount));
                    break;
                case "Influence":
                    Influence = System.Math.Max(0, System.Math.Min(100, Influence + amount));
                    break;
                case "Military":
                    Military = System.Math.Max(0, System.Math.Min(100, Military + amount));
                    break;
            }
        }
    }
}
