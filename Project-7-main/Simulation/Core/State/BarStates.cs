using System;
using System.Collections.Generic;

namespace PG.World.Simulation
{
    /// <summary>Runtime state of a global progression bar (e.g., WorldStability, MagicalAwareness)</summary>
    public class ProgressionBarState
    {
        public ProgressionBarDefinition Definition { get; set; }
        public double Value { get; private set; }
        public bool Completed { get; private set; }
        public bool HasBeenTouched { get; internal set; }

        public ProgressionBarState(ProgressionBarDefinition definition)
        {
            Definition = definition;
            Value = definition.MinValue;
            HasBeenTouched = false;
        }

        public void AddProgress(double amount)
        {
            Value = System.Math.Max(Definition.MinValue, System.Math.Min(Definition.MaxValue, Value + amount));
            HasBeenTouched = true;
        }

        public void ApplyDecay()
        {
            Value = System.Math.Max(Definition.MinValue, Value - Definition.DecayAmount);
        }

        public bool CheckCompletion()
        {
            if (!Completed && Value >= Definition.Threshold)
            {
                Completed = true;
                return true;
            }
            return false;
        }
    }

    /// <summary>Runtime state of zone evolution bar - tracks region tier progression</summary>
    public class ZoneEvolutionBarState
    {
        public ZoneEvolutionBarDefinition Definition { get; set; }
        public double Value { get; private set; }
        public ZoneEvolutionMilestone CurrentMilestone { get; private set; }
        public bool HasReachedMax { get; private set; }
        public List<string> ModificationHistory { get; private set; } = new();

        public ZoneEvolutionBarState(ZoneEvolutionBarDefinition definition)
        {
            Definition = definition;
            Value = definition.StartingValue;
            CurrentMilestone = ZoneEvolutionMilestone.None;
            HasReachedMax = false;
        }

        public void ModifyValue(double amount, string source = "Unknown")
        {
            Value = System.Math.Max(Definition.MinValue, System.Math.Min(Definition.MaxValue, Value + amount));
            ModificationHistory.Add($"{source}: {amount:+0.00;-0.00;0} -> {Value:F0}");

            CheckMilestones();
        }

        public void ApplyDecay()
        {
            ModifyValue(-Definition.DecayAmount, "Decay");
        }

        public void CheckMilestones()
        {
            if (HasReachedMax)
                return;

            if (Value >= Definition.MaxValue)
            {
                HasReachedMax = true;
                CurrentMilestone = ZoneEvolutionMilestone.Milestone4;
            }
            else if (Value >= Definition.MaxValue * 0.75)
            {
                CurrentMilestone = ZoneEvolutionMilestone.Milestone4;
            }
            else if (Value >= Definition.MaxValue * 0.5)
            {
                CurrentMilestone = ZoneEvolutionMilestone.Milestone3;
            }
            else if (Value >= Definition.MaxValue * 0.25)
            {
                CurrentMilestone = ZoneEvolutionMilestone.Milestone2;
            }
        }
    }

    /// <summary>Runtime state of a regional EventBar - progression metric for settlements and encounters</summary>
    public class EventBarState
    {
        public EventBarDefinition Definition { get; set; }
        public double Value { get; private set; }
        public bool Completed { get; private set; }
        public int TurnsSinceCompletion { get; private set; }
        public int CooldownRemaining { get; private set; }
        public bool HasBeenTouched { get; internal set; }
        private bool _completionProcessed = false;

        public EventBarState(EventBarDefinition definition)
        {
            Definition = definition;
            Value = definition.StartingValue;
            HasBeenTouched = false;
        }

        public void AddProgress(double amount)
        {
            if (IsOnCooldown)
                return;

            Value = System.Math.Max(Definition.MinValue, System.Math.Min(Definition.MaxValue, Value + amount));
            HasBeenTouched = true;
        }

        public void ApplyDecay()
        {
            Value = System.Math.Max(Definition.MinValue, Value - Definition.DecayAmount);
        }

        public bool CheckCompletion()
        {
            if (!Completed && Value >= Definition.Threshold)
            {
                Completed = true;
                _completionProcessed = false;
                return true;
            }
            return false;
        }

        public bool NeedsCompletionProcessing()
        {
            return Completed && !_completionProcessed;
        }

        public bool IsOnCooldown => CooldownRemaining > 0;

        public void TickCooldown()
        {
            if (CooldownRemaining > 0)
                CooldownRemaining--;
        }

        public void MarkCompletionProcessed()
        {
            _completionProcessed = true;
        }

        public void Reset()
        {
            if (Definition.Repeatable)
            {
                Value = Definition.MinValue;
                Completed = false;
                _completionProcessed = false;
                TurnsSinceCompletion = 0;
                CooldownRemaining = System.Math.Max(0, Definition.CooldownTurns);
            }
        }
    }
}
