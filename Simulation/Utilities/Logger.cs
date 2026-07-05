using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    public enum LogCategory
    {
        Player,
        Simulation
    }

    public static class SimulationLog
    {
        private static readonly object LockObj = new object();

        public static void Log(string message, LogCategory category = LogCategory.Simulation)
        {
            lock (LockObj)
            {
                var prefix = category switch
                {
                    LogCategory.Player => "[EVENT]",
                    LogCategory.Simulation => "[SIM]",
                    _ => "[LOG]"
                };
                Console.WriteLine($"{prefix} {message}");
            }
        }
    }
}
