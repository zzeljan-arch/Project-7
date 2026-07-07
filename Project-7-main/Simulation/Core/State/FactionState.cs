using System.Collections.Generic;

namespace PG.World.Simulation
{
    public class FactionState
    {
        public FactionType Type { get; set; }
        public string Name { get; set; }
        public double Influence { get; set; }
        public double Reputation { get; set; }
        public bool HasCamp { get; set; }
    }

    public class EconomyState
    {
        public Dictionary<string, double> MarketPrices { get; set; } = new();
        public double RegionalWealth { get; set; }
    }
}
