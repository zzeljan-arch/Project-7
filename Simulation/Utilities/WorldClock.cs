using System;

namespace PG.World.Simulation
{
    public class WorldClock
    {
        public int Day { get; private set; } = 1;
        public int Hour { get; private set; } = 8;
        public int Minute { get; private set; } = 0;

        public void AdvanceByMinutes(int minutes)
        {
            int totalMinutes = Day * 1440 + Hour * 60 + Minute + minutes;
            Day = totalMinutes / 1440;
            Hour = (totalMinutes % 1440) / 60;
            Minute = totalMinutes % 60;
        }

        public string CurrentTime => $"Day {Day}, {Hour:D2}:{Minute:D2}";
    }
}
