using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    public class ItemDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int BaseValue { get; set; }
    }

    public class ItemStack
    {
        public ItemDefinition Definition { get; set; }
        public int Quantity { get; set; }

        public ItemStack(ItemDefinition definition, int quantity)
        {
            Definition = definition;
            Quantity = quantity;
        }
    }
}
