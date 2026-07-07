namespace PG.World.Simulation
{
    public static class SimulationConfig
    {
        public const int CampaignRegionCount = 4;
        public const double StartingTier2Chance = 0.40;
        public const int ExploreMinutes = 120;
        public const double EncounterChance = 0.35;
        public const double EliteEncounterChance = 0.12;
        public const double RareEncounterChance = 0.05;
        public const int WorldUpdateTicksPerAction = 1;
        public const int EvolutionCheckFrequency = 3;
        public const double ProgressionDecayRate = 0.08;
        public const double RestHealPerHour = 6.0;
        public const int RestHours = 8;
        public const double DefaultStartingGold = 80;
        public const double MarketVolatility = 0.12;
        public const int FastSimulationDefaultCampaigns = 1000;
        public const int FastSimulationDefaultActions = 200;
    }
}
