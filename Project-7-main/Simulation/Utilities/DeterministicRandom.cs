using System;

namespace PG.World.Simulation
{
    public class DeterministicRandom
    {
        private ulong _seed;
        private const ulong A = 6364136223846793005UL;
        private const ulong C = 1442695040888963407UL;

        public DeterministicRandom(ulong seed)
        {
            _seed = seed;
        }

        public int Next(int maxValue)
        {
            return (int)(Next() % (ulong)maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return minValue + (int)(Next() % (ulong)(maxValue - minValue));
        }

        public double NextDouble()
        {
            return (Next() & 0xFFFFFFF) / 268435456.0;
        }

        public bool Probability(double chance)
        {
            return NextDouble() < chance;
        }

        private ulong Next()
        {
            _seed = A * _seed + C;
            return _seed;
        }
    }
}
