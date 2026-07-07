using PG.World.Simulation;
using Xunit;

namespace Simulation.Tests;

public class QuestFlowTests
{
    [Fact]
    public void ResolvingRequiredEncounter_ShouldCompleteQuestAndApplyQuestProgressionEffects()
    {
        var world = WorldState.CreateCampaign(1234, RegionId.NorthernRealm);
        world.ShowLogs = false;

        world.AcceptQuest("ClearBanditCamp");

        var quest = Assert.Single(world.Player.Quests.Where(q => q.Definition.Id == "ClearBanditCamp"));
        Assert.Equal(QuestStatus.OnQuest, quest.Status);

        var encounter = EncounterRegistry.AllEncounters.Single(e => e.Id == "BanditStronghold");
        var outcome = world.ResolveEncounter(encounter, new EncounterOption
        {
            Description = "Victory",
            Execute = _ => new EncounterResult { Message = "You defeat the bandit stronghold.", Success = true, PlayerVisible = true }
        });

        Assert.True(outcome.Success);
        var completedQuest = world.Player.Quests.Single(q => q.Definition.Id == "ClearBanditCamp");
        Assert.Equal(QuestStatus.Completed, completedQuest.Status);

        var bar = world.GetRegionProgressionBar(quest.RegionId, "BanditClearance");
        Assert.NotNull(bar);
        Assert.True(bar!.Value >= 20);
    }
}
