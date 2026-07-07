namespace PG.World.Simulation
{
    /// <summary>World progression tiers - higher tiers mean more challenging/advanced regions</summary>
    public enum WorldTier
    {
        Tier1 = 1,
        Tier2 = 2,
        Tier3 = 3,
        Tier4 = 4,
        Tier5 = 5
    }

    /// <summary>The 7 major regions player can explore</summary>
    public enum RegionId
    {
        NorthernRealm = 0,
        DarkForest = 1,
        DesertKingdom = 2,
        Swamp = 3,
        Highlands = 4,
        InfernalLands = 5,
        ArcaneEmpire = 6
    }

    /// <summary>Region evolution paths - unique ways each region can change over time</summary>
    public enum EvolutionPath
    {
        None = -1,
        AgeOfRaiders = 0,
        EternalWinter = 1,
        Ragnarok = 2,
        UndeadNorth = 3,
        PrimordialJungle = 4,
        CorruptedAbyss = 5,
        ElvenEnclave = 6,
        CursedWildlands = 7,
        AgeOfMerchants = 8,
        AgeOfPharaohs = 9,
        ElementalCatastrophe = 10,
        ShadowAndSecrets = 11,
        PrimordialBog = 12,
        PlaguePestilence = 13,
        MonsterDominion = 14,
        MysticalAwakening = 15,
        DwarvenProsperity = 16,
        AsceticHermitage = 17,
        DragonRoost = 18,
        CorruptionBlight = 19,
        DemonicDominion = 20,
        VolcanicCatastrophe = 21,
        DemonicElementalFusion = 22,
        EternalForge = 23,
        ArcaneAscendance = 24,
        KnowledgeHoarding = 25,
        TechnoMagicalSynthesis = 26,
        CorruptionDecay = 27
    }

    public enum PlayerAction
    {
        Explore,
        Purify,
        Raid,
        Research,
        Negotiate,
        Corrupt,
        Defend,
        Rest,
        Travel,
        VisitTown,
        VisitMarketplace,
        Inventory,
        Character,
        QuestLog,
        ReadBook,
        Fish,
        Camp,
        Craft,
        Trade,
        Help,
        Investigate,
        Steal,
        Fight,
        Leave
    }

    public enum PathTrait
    {
        War,
        Magic,
        Corruption,
        Nature,
        Trade,
        Order,
        Knowledge
    }

    public enum BiomeType
    {
        Plains,
        Forest,
        Desert,
        Swamp,
        Mountain,
        Arcane,
        Infernal
    }

    public enum FactionType
    {
        Merchants,
        Rangers,
        Bandits,
        Mages,
        Miners,
        Clergy,
        Nobles
    }

    public enum QuestStatus
    {
        Available,
        OnQuest,
        Completed,
        Failed
    }

    public enum EncounterDifficulty
    {
        Normal,
        Elite,
        Rare
    }

    public enum ZoneEvolutionMilestone
    {
        None = 0,
        Milestone1 = 1,
        Milestone2 = 2,
        Milestone3 = 3,
        Milestone4 = 4
    }
}
