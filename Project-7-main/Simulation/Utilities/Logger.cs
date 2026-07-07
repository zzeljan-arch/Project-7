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

        public static void Log(string message, LogCategory category = LogCategory.Simulation, ConsoleColor? color = null)
        {
            lock (LockObj)
            {
                var prefix = category switch
                {
                    LogCategory.Player => "[EVENT]",
                    LogCategory.Simulation => "[SIM]",
                    _ => "[LOG]"
                };

                var originalColor = Console.ForegroundColor;
                if (color.HasValue)
                    Console.ForegroundColor = color.Value;

                Console.WriteLine($"{prefix} {message}");

                if (color.HasValue)
                    Console.ForegroundColor = originalColor;
            }
        }
    }
}
