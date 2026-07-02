using System.Collections.Generic;
using PG.Gameplay.Combat;
using PG.World.Simulation;

namespace PG.Systems
{
    public class QuestManager
    {
        public Quest AssignInitialQuest(RegionState region)
        {
            return new KillQuest
            {
                QuestId = "kill_bandits_1",
                Title = "Clear the Raider Outpost",
                Description = $"Eliminate 5 bandits in {region.RegionId}.",
                State = QuestState.Active,
                Objective = new QuestObjective { Description = "Defeat 5 bandits", TargetCount = 5, CurrentCount = 0 },
                RewardXP = 250,
                RewardGold = 150
            };
        }

        public void UpdateQuests(GameState gameState, float deltaTime)
        {
            foreach (var quest in gameState.ActiveQuests)
            {
                if (quest.State != QuestState.Active)
                    continue;

                if (quest.IsComplete())
                    quest.State = QuestState.Completed;
            }
        }

        public void RecordEnemyKill(GameState gameState, EnemyInstance enemy)
        {
            foreach (var quest in gameState.ActiveQuests)
            {
                if (quest.State != QuestState.Active)
                    continue;

                if (quest is KillQuest killQuest)
                {
                    killQuest.Objective.CurrentCount++;
                    if (killQuest.Objective.CurrentCount >= killQuest.Objective.TargetCount)
                    {
                        killQuest.State = QuestState.Completed;
                    }
                }
            }
        }
    }

    public abstract class Quest
    {
        public string QuestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public QuestState State { get; set; }
        public int RewardXP { get; set; }
        public int RewardGold { get; set; }
        public QuestObjective Objective { get; set; }

        public abstract bool IsComplete();
    }

    public class KillQuest : Quest
    {
        public override bool IsComplete()
        {
            return Objective != null && Objective.CurrentCount >= Objective.TargetCount;
        }
    }

    public class QuestObjective
    {
        public string Description { get; set; }
        public int TargetCount { get; set; }
        public int CurrentCount { get; set; }
    }

    public enum QuestState
    {
        Available,
        Active,
        Completed,
        Abandoned,
        Failed
    }
}
