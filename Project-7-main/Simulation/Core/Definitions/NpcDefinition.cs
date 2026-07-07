using System.Collections.Generic;

namespace PG.World.Simulation
{
    public class NpcDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public List<string> Traits { get; set; } = new();
        public List<string> HiddenTraits { get; set; } = new();
        public bool StartsAlive { get; set; } = true;
        public string SuccessorId { get; set; }
    }

    public class NpcState
    {
        public NpcDefinition Definition { get; set; }
        public bool IsAlive { get; set; }
        public string CurrentRole { get; set; }

        public NpcState(NpcDefinition definition)
        {
            Definition = definition;
            IsAlive = definition.StartsAlive;
            CurrentRole = definition.Role;
        }
    }
}
