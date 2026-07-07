using System;
using System.Collections.Generic;
using System.Linq;

namespace PG.World.Simulation
{
    public static class EncounterRegistry
    {
        public static readonly List<EncounterDefinition> AllEncounters = new()
        {
            new EncounterDefinition
            {
                Id = "BanditAmbush",
                Name = "Bandit Ambush",
                Summary = "A group of bandits jumps out from cover and demands your valuables.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => world.Regions[world.Player.CurrentRegion].Definition.Biome != BiomeType.Arcane,
                Weight = world => 1.0,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Fight the bandits.", Execute = world => { var region = world.Regions[world.Player.CurrentRegion]; region.EventBars["BanditPatrol"].AddProgress(12); if (region.ZoneEvolutionBars.TryGetValue("AgeOfBandits", out var ageOfBandits)) ageOfBandits.ModifyValue(7, "Bandit Ambush - Fight"); return new EncounterResult { Message = "Combat ensues. (Bandit Patrol +12)", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Pay them off.", Execute = world => { var region = world.Regions[world.Player.CurrentRegion]; world.Player.SpendGold(12); region.EventBars["BanditPatrol"].AddProgress(3); if (region.ZoneEvolutionBars.TryGetValue("AgeOfBandits", out var ageOfBandits)) ageOfBandits.ModifyValue(2, "Bandit Ambush - Bribe"); return new EncounterResult { Message = "You escape by paying tribute. (Bandit Patrol +3)", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Attempt to flee.", Execute = world => new EncounterResult { Message = "You escape the ambush.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "StrandedMerchant",
                Name = "Stranded Merchant",
                Summary = "A trader is stranded after a broken wagon and needs aid.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => true,
                Weight = world => 0.9,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Repair the wagon and help.", Execute = world => { var region = world.Regions[world.Player.CurrentRegion]; region.EventBars["MerchantCaravan"].AddProgress(16); if (world.Regions.TryGetValue(RegionId.DesertKingdom, out var desertRegion) && desertRegion.ZoneEvolutionBars.TryGetValue("MerchantsRepublic", out var merchantsRepublic)) merchantsRepublic.ModifyValue(10, "Stranded Merchant - Repair"); return new EncounterResult { Message = "You repair the wagon! Merchant Caravan activity +16", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Offer to escort them.", Execute = world => { var region = world.Regions[world.Player.CurrentRegion]; region.EventBars["MerchantCaravan"].AddProgress(10); if (world.Regions.TryGetValue(RegionId.DesertKingdom, out var desertRegion) && desertRegion.ZoneEvolutionBars.TryGetValue("MerchantsRepublic", out var merchantsRepublic)) merchantsRepublic.ModifyValue(6, "Stranded Merchant - Escort"); return new EncounterResult { Message = "You guide them safely. Merchant Caravan activity +10.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Take advantage and steal.", Execute = world => new EncounterResult { Message = "You grab some cargo.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "FrozenShrine",
                Name = "Frozen Shrine",
                Summary = "A frost-covered shrine hums with ancient power in the Northern Realm.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.85,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Relight the sacred braziers.", Execute = world => { var region = world.Regions[RegionId.NorthernRealm]; region.EventBars["TempleActivity"].AddProgress(6); if (region.ZoneEvolutionBars.TryGetValue("EternalWinter", out var eternalWinter)) eternalWinter.ModifyValue(-8, "Frozen Shrine - Relight Braziers"); if (region.ZoneEvolutionBars.TryGetValue("DivineOrder", out var divineOrder)) divineOrder.ModifyValue(5, "Frozen Shrine - Relight Braziers"); return new EncounterResult { Message = "Warm light pushes back the cold. (Temple Activity +6)", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Study the frost runes.", Execute = world => { var region = world.Regions[RegionId.NorthernRealm]; if (region.ZoneEvolutionBars.TryGetValue("EternalWinter", out var eternalWinter)) eternalWinter.ModifyValue(7, "Frozen Shrine - Study Runes"); return new EncounterResult { Message = "The runes answer with a colder wind.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Leave offerings for the pilgrims.", Execute = world => { var region = world.Regions[RegionId.NorthernRealm]; if (region.ZoneEvolutionBars.TryGetValue("DivineOrder", out var divineOrder)) divineOrder.ModifyValue(6, "Frozen Shrine - Offerings"); return new EncounterResult { Message = "Pilgrims bless your act of faith.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "RestlessBarrow",
                Name = "Restless Barrow",
                Summary = "An old burial mound cracks open and undead stir beneath the ice.",
                Difficulty = EncounterDifficulty.Elite,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.75,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Sanctify the barrow.", Execute = world => { var region = world.Regions[RegionId.NorthernRealm]; if (region.ZoneEvolutionBars.TryGetValue("UndeadPower", out var undeadPower)) undeadPower.ModifyValue(-10, "Restless Barrow - Sanctify"); if (region.ZoneEvolutionBars.TryGetValue("DivineOrder", out var divineOrder)) divineOrder.ModifyValue(4, "Restless Barrow - Sanctify"); world.ApplyHiddenInfluence("DawnOfGods", 0.5, "Ancient rites restored"); return new EncounterResult { Message = "The dead return to silence.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Plunder the grave goods.", Execute = world => { var region = world.Regions[RegionId.NorthernRealm]; world.Player.Gold += 14; if (region.ZoneEvolutionBars.TryGetValue("UndeadPower", out var undeadPower)) undeadPower.ModifyValue(8, "Restless Barrow - Plunder"); if (region.ZoneEvolutionBars.TryGetValue("AgeOfBandits", out var ageOfBandits)) ageOfBandits.ModifyValue(4, "Restless Barrow - Plunder"); world.ApplyHiddenInfluence("AncientEvil", 2.0, "Ancient tomb looted"); return new EncounterResult { Message = "You leave richer, and the curse grows darker.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Seal it with ice magic.", Execute = world => { var region = world.Regions[RegionId.NorthernRealm]; if (region.ZoneEvolutionBars.TryGetValue("UndeadPower", out var undeadPower)) undeadPower.ModifyValue(-3, "Restless Barrow - Ice Seal"); if (region.ZoneEvolutionBars.TryGetValue("EternalWinter", out var eternalWinter)) eternalWinter.ModifyValue(5, "Restless Barrow - Ice Seal"); return new EncounterResult { Message = "The tomb closes, but winter deepens.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "AncientCryptRitual",
                Name = "Ancient Crypt Ritual",
                Summary = "You discover a broken crypt where necromantic sigils still pulse faintly.",
                Difficulty = EncounterDifficulty.Elite,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.40,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Use necromancy to question the dead.", Execute = world => { world.ApplyHiddenInfluence("AncientEvil", 3.0, "Necromancy practiced"); world.ApplyHiddenInfluence("TheVeil", 1.4, "Boundary crossed through necromancy"); return new EncounterResult { Message = "The dead whisper answers that should never be known.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Take the cursed artifact.", Execute = world => { world.Player.AddItem("AncientRelic", 1); world.ApplyHiddenInfluence("AncientEvil", 2.2, "Cursed artifact taken"); return new EncounterResult { Message = "You pocket a cursed artifact and feel watched.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Bury the remains with full rites.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.8, "Jarl buried with proper ritual"); world.ApplyHiddenInfluence("AncientEvil", -1.2, "Ritual calmed ancient malice"); return new EncounterResult { Message = "The crypt falls silent as the rites complete.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "WanderingKnight",
                Name = "The Wandering Knight",
                Summary = "A lone armored knight challenges worthy travelers to a rite of honor.",
                IsLegendary = true,
                Difficulty = EncounterDifficulty.Elite,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.30,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Duel with honor.", Execute = world => { var win = world.Rng.Probability(0.58); if (win) { world.Player.AddItem("KnightsOathRelic", 1); world.ApplyHiddenInfluence("DawnOfGods", 0.8, "Knight's oath reclaimed"); return new EncounterResult { Message = "You win the duel and claim the Knight's Oath relic.", Success = true, PlayerVisible = true }; } world.Player.TakeDamage(12); world.ApplyHiddenInfluence("CivilWar", 1.2, "Failed duel sparks local unrest"); return new EncounterResult { Message = "You are defeated in single combat.", Success = false, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Speak of old wars.", Execute = world => { world.ApplyHiddenInfluence("KingsStability", 1.0, "Knight brokered peace"); world.ApplyHiddenInfluence("OldGods", 0.8, "Ancient oaths remembered"); return new EncounterResult { Message = "He shares forgotten history and rides on.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Refuse and move on.", Execute = world => new EncounterResult { Message = "The knight nods and disappears into the snow.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "LastGiant",
                Name = "The Last Giant",
                Summary = "A giant sits in silence, weeping over a frozen battlefield.",
                IsLegendary = true,
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.25,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Hear his story.", Execute = world => { world.Player.AddItem("GiantTear", 1); world.Player.AddItem("RumorFragment", 2); world.ApplyHiddenInfluence("OldGods", 2.5, "Learned the giant's history"); world.ApplyHiddenInfluence("DawnOfGods", 0.8, "Ancient legend preserved"); return new EncounterResult { Message = "The giant tells the history of the north and leaves a giant's tear.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Hunt the giant.", Execute = world => { world.Player.TakeDamage(18); world.Player.AddItem("DragonBone", 1); world.ApplyHiddenInfluence("AncientEvil", 1.5, "Last giant slain"); world.ApplyHiddenInfluence("CivilWar", 0.8, "Clans angered by giant's death"); return new EncounterResult { Message = "You bring down the giant, but the land grows bitter.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Leave him be.", Execute = world => new EncounterResult { Message = "You leave the giant to his grief.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "WhiteWolf",
                Name = "The White Wolf",
                Summary = "A white wolf watches from a ridge, silent and unafraid.",
                IsLegendary = true,
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.0005,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Follow the wolf.", Execute = world => { var region = world.Regions[RegionId.NorthernRealm]; if (region.ZoneEvolutionBars.TryGetValue("DivineOrder", out var divineOrder)) divineOrder.ModifyValue(4, "White Wolf - Ancient shrine"); world.Player.AddItem("Runestone", 1); world.ApplyHiddenInfluence("DawnOfGods", 0.3, "White wolf leads to forgotten shrine"); return new EncounterResult { Message = "The wolf leads you to an ancient shrine and vanishes.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Attack the wolf.", Execute = world => { world.Player.AddItem("WhiteWolfPelt", 1); world.ApplyHiddenInfluence("DawnOfGods", -2.0, "White stag or wolf harmed"); world.ApplyHiddenInfluence("AncientEvil", 1.2, "Sacred beast slain"); world.ApplyHiddenInfluence("TheVeil", 1.1, "Wolf-spirit enraged"); return new EncounterResult { Message = "You strike the wolf, but the forest grows unnaturally quiet.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Observe in silence.", Execute = world => { world.ApplyHiddenInfluence("TheVeil", 0.4, "Wolf omen witnessed"); return new EncounterResult { Message = "The wolf observes you, then disappears into mist.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "BjornLastStand",
                Name = "Bjorn's Last Stand",
                Summary = "Bjorn the One-Eyed is cornered while defending a mountain pass.",
                IsLegendary = true,
                Difficulty = EncounterDifficulty.Elite,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm
                                   && world.UniqueNpcs.TryGetValue("BjornOneEyed", out var bjorn)
                                   && bjorn.IsAlive,
                Weight = world => 0.20,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Stand with Bjorn.", Execute = world => { var victory = world.Rng.Probability(0.62); if (victory) { world.ApplyHiddenInfluence("KingsStability", 1.6, "Bjorn defended the pass"); return new EncounterResult { Message = "You and Bjorn drive the raiders back.", Success = true, PlayerVisible = true }; } if (world.UniqueNpcs.TryGetValue("BjornOneEyed", out var bjornState)) bjornState.IsAlive = false; world.ApplyHiddenInfluence("CivilWar", 1.5, "Bjorn fell at the pass"); return new EncounterResult { Message = "Bjorn falls in battle; the pass is lost.", Success = false, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Retreat and preserve your life.", Execute = world => { world.ApplyHiddenInfluence("CivilWar", 1.0, "Pass abandoned"); return new EncounterResult { Message = "You retreat; Bjorn fights on without you.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "EclipseCultProcession",
                Name = "Eclipse Cult Procession",
                Summary = "Black-robed pilgrims march beneath darkened banners and chant to an unseen sky.",
                RequiredUnlockId = "EclipseCultProcession",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.45,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Break the procession.", Execute = world => { world.ApplyHiddenInfluence("AncientEvil", -4, "Cult procession broken"); return new EncounterResult { Message = "The cult scatters into the snow.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Shadow them in secret.", Execute = world => { world.Player.AddItem("AncientBook", 1); return new EncounterResult { Message = "You recover a blasphemous text from the cult's trail.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "BonefireDrake",
                Name = "Bonefire Drake",
                Summary = "A drake stitched from bone and cinder crashes over the ridge.",
                RequiredUnlockId = "BonefireDrake",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.40,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Drive it off.", Execute = world => { world.Player.AddItem("DragonBone", 1); return new EncounterResult { Message = "You shatter fragments from its burning skeleton.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Track its roost.", Execute = world => { world.ApplyHiddenInfluence("DragonAwakeningHidden", 2, "Drake roost uncovered"); return new EncounterResult { Message = "You map the creature's mountain lair.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "VeilTear",
                Name = "Veil Tear",
                Summary = "The air splits open and a shimmering wound in the world hovers ahead.",
                RequiredUnlockId = "VeilTear",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.40,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Seal the rift.", Execute = world => { world.ApplyHiddenInfluence("TheVeil", -5, "Veil tear sealed"); return new EncounterResult { Message = "The tear shrinks with a thunderclap.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Take what falls through.", Execute = world => { world.Player.AddItem("Runestone", 1); return new EncounterResult { Message = "A strange runestone tumbles out of the rift into your hands.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "NornProcession",
                Name = "Norn Procession",
                Summary = "Three shrouded seers walk the same road in different directions at once.",
                RequiredUnlockId = "NornProcession",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.35,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Ask your fate.", Execute = world => { world.Player.AddItem("LostSongScroll", 1); return new EncounterResult { Message = "The Norns answer in a song you cannot forget.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Offer a relic.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 1.0, "Norn bargain accepted"); return new EncounterResult { Message = "The seers accept your offering and rewrite a thread of fate.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "AuroraPeak",
                Name = "The Aurora",
                Summary = "A brilliant aurora crowns the sky for three nights above the northern peaks.",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.06,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Climb and witness it in silence.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.3, "Aurora witnessed from the highest mountain"); return new EncounterResult { Message = "You watch the sky burn with ancient color.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Ignore it and head back.", Execute = world => new EncounterResult { Message = "You leave the lights behind.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "WhiteStagSighting",
                Name = "White Stag Sighting",
                Summary = "Travelers whisper of a white stag crossing moonlit snowfields.",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.04,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Let it pass unharmed.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.5, "White stag allowed to live"); return new EncounterResult { Message = "The stag bows once and disappears into the pines.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Hunt it.", Execute = world => { world.Player.AddItem("WhiteWolfPelt", 1); world.ApplyHiddenInfluence("DawnOfGods", -2.0, "White stag or wolf harmed"); return new EncounterResult { Message = "The snow falls silent after the kill.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "RavenGathering",
                Name = "Raven Gathering",
                Summary = "Thousands of ravens circle an ancient battlefield.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.09,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Leave food in respect.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.2, "Ravens honored at battlefield"); return new EncounterResult { Message = "The ravens settle and watch quietly.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Drive them away.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", -0.5, "Ravens driven from battlefield"); return new EncounterResult { Message = "A harsh cry follows you for miles.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "AncientOakFalls",
                Name = "Ancient Oak Falls",
                Summary = "A sacred oak older than kingdoms has fallen at last.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.05,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Preserve and honor the tree.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.4, "Ancient oak honored with rites"); return new EncounterResult { Message = "You bind the trunk with carved prayer ribbons.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Cut it into lumber.", Execute = world => { world.Player.Gold += 20; world.ApplyHiddenInfluence("DawnOfGods", -1.0, "Ancient oak cut for lumber"); return new EncounterResult { Message = "You haul good timber from sacred ground.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "FrozenLakeOpens",
                Name = "The Frozen Lake Opens",
                Summary = "Centuries of ice crack apart to reveal a buried runestone.",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.05,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Activate the runestone.", Execute = world => { world.Player.AddItem("Runestone", 1); world.ApplyHiddenInfluence("DawnOfGods", 0.8, "Forgotten lake runestone activated"); return new EncounterResult { Message = "The stone glows and then goes still.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Leave it untouched.", Execute = world => new EncounterResult { Message = "You leave the old stone in the cold.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "SacredSpringPoisoned",
                Name = "Sacred Spring",
                Summary = "An ancient spring has been poisoned and animals lie dead nearby.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.07,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Cleanse the spring.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.5, "Sacred spring cleansed"); world.ApplyHiddenInfluence("TheVeil", -0.3, "Spring restored"); return new EncounterResult { Message = "Clear water slowly returns.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Move on.", Execute = world => new EncounterResult { Message = "You leave the spring as you found it.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "OneEyedWanderer",
                Name = "The One-Eyed Wanderer",
                Summary = "An old traveler asks to share your fire for the night.",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.02,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Welcome him to your fire.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 1.0, "One-eyed wanderer welcomed at fire"); return new EncounterResult { Message = "At dawn, only footprints in frost remain.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Refuse.", Execute = world => new EncounterResult { Message = "He nods and vanishes into the dark.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "GoldenHairedWoman",
                Name = "The Woman with Golden Hair",
                Summary = "A woman by the roadside weaves flowers into impossible patterns.",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.03,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Speak with her.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.2, "Golden-haired weaver's blessing accepted"); return new EncounterResult { Message = "She smiles, then the road is empty.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Pass quietly.", Execute = world => new EncounterResult { Message = "You keep walking without a word.", Success = true, PlayerVisible = true } }
                }
            },
            new EncounterDefinition
            {
                Id = "ChildWithWolves",
                Name = "Child Playing with Wolves",
                Summary = "A child laughs while wolves circle peacefully nearby.",
                Difficulty = EncounterDifficulty.Rare,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.04,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Protect the scene and keep distance.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.4, "Wolves and child protected without interference"); return new EncounterResult { Message = "Nothing is harmed, and that seems important.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Break it up by force.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", -0.5, "Sacred wolf omen disturbed"); return new EncounterResult { Message = "The wolves flee and the child glares in silence.", Success = true, PlayerVisible = true }; } }
                }
            },
            new EncounterDefinition
            {
                Id = "RavenAfterBattle",
                Name = "Raven on the Battlefield",
                Summary = "After battle, one raven watches the fallen from a broken spear.",
                Difficulty = EncounterDifficulty.Normal,
                CanOccur = world => world.Player.CurrentRegion == RegionId.NorthernRealm,
                Weight = world => 0.06,
                Options = new List<EncounterOption>
                {
                    new EncounterOption { Description = "Bury the dead with honor.", Execute = world => { world.ApplyHiddenInfluence("DawnOfGods", 0.3, "Fallen buried beneath raven's watch"); return new EncounterResult { Message = "Snow covers the graves before sunset.", Success = true, PlayerVisible = true }; } },
                    new EncounterOption { Description = "Loot the battlefield.", Execute = world => { world.Player.Gold += 12; world.ApplyHiddenInfluence("DawnOfGods", -0.5, "Battlefield looted over burial rites"); return new EncounterResult { Message = "You leave richer and watched.", Success = true, PlayerVisible = true }; } }
                }
            }
        };
    }

    // ===== CORE ENGINE - WorldState =====
    public class WorldState
    {
        private class EncounterUnlockState
        {
            public int UsesRemaining { get; set; }
            public int TurnsRemaining { get; set; }
        }

        public static WorldState CurrentGlobalState { get; private set; }

        public ulong Seed { get; private set; }
        public DeterministicRandom Rng { get; private set; }
        public WorldClock Clock { get; private set; }
        public Dictionary<RegionId, RegionState> Regions { get; private set; } = new();
        public PlayerState Player { get; private set; }
        public EconomyState Economy { get; private set; }
        public Dictionary<FactionType, FactionState> Factions { get; private set; } = new();
        public Dictionary<string, ProgressionBarState> ProgressionBars { get; private set; } = new();
        public List<EncounterDefinition> Encounters { get; private set; } = new();
        public List<LongTermEventDefinition> LongTermEvents { get; private set; } = new();
        public List<QuestDefinition> QuestDefinitions { get; private set; } = new();
        public Dictionary<string, EventBarState> HiddenEventBars { get; private set; } = new();
        public Dictionary<string, NpcState> UniqueNpcs { get; private set; } = new();
        public HashSet<string> UnlockedEncounterIds { get; private set; } = new();
        private Dictionary<string, EncounterUnlockState> TemporaryEncounterUnlocks { get; set; } = new();
        public List<string> SimulationHistory { get; private set; } = new();
        public bool IsFastMode { get; set; } = false;
        public bool ShowLogs { get; set; } = true;
        public bool AutoResolveEncounters { get; set; } = false;
        public int TicksSinceLastEvolution { get; private set; }

        private static readonly double[] ZoneMilestoneThresholds = { 25, 50, 75, 100 };
        private readonly Dictionary<string, int> _rumorShardCounts = new();
        private readonly Queue<string> _queuedRumorQuestIds = new();
        private readonly HashSet<string> _unlockedRumorQuestIds = new();
        private readonly HashSet<string> _completedQuestIds = new();
        private readonly HashSet<string> _completedQuestLineIds = new();
        private readonly Dictionary<string, int> _dawnSourceCooldowns = new();
        private NoveltyTracker _noveltyTracker = new();
        private QuestDefinition _autoAcceptedQuest;
        private static readonly string[] CollectorItemIds =
        {
            "AncientBook",
            "LostSongScroll",
            "Runestone",
            "DragonBone",
            "AncientRelic",
            "KnightsOathRelic",
            "ReligiousRelic",
            "WhiteWolfPelt"
        };
        private const ConsoleColor FastFeatureColor = ConsoleColor.Green;
        private const int DawnSourceCooldownTurns = 20;
        private static readonly HashSet<string> QuestLineQuestIds = new(
            QuestLineRegistry.AllQuestLines.SelectMany(ql => ql.QuestIds),
            StringComparer.OrdinalIgnoreCase);
        private static readonly HashSet<string> DawnMythicQuestIds = new(StringComparer.OrdinalIgnoreCase)
        {
            "RestoreForgottenShrine",
            "LastSkald",
            "HonorTheFallen",
            "WorldTreeSapling",
            "ForgeOfTheDwarves",
            "FindBrokenHarp",
            "FindBlindSkald",
            "RecoverLostVerseI",
            "RecoverLostVerseII",
            "RecoverLostVerseIII",
            "ClimbFrostpeak",
            "SingTheAncientSong",
            "SevenRelics"
        };
        private static readonly HashSet<string> DawnMythicQuestLineIds = new(StringComparer.OrdinalIgnoreCase)
        {
            "TheSongThatEndsWinter",
            "NineRavens",
            "CrownBeneathIce",
            "LastHunt",
            "SleepingWolf",
            "ForgeOfStars"
        };
        private static readonly string[] DawnMythicSourceKeywords =
        {
            "aurora", "white stag", "raven", "oak", "runestone", "sacred", "shrine", "skald", "norn", "odin",
            "one-eyed", "valkyrie", "dwarf", "dvergr", "yggdrasil", "wolf", "myth", "ritual", "honor", "burial",
            "temple", "mimir", "frostpeak", "song", "well", "land spirit", "landvaettir", "old gods", "forgotten", "oath"
        };

        private Dictionary<string, double> CaptureWorldDeltaSnapshot()
        {
            var snapshot = new Dictionary<string, double>();

            foreach (var hidden in HiddenEventBars)
            {
                snapshot[$"hidden:{hidden.Key}"] = hidden.Value.Value;
            }

            if (Regions.TryGetValue(RegionId.NorthernRealm, out var northern))
            {
                foreach (var zone in northern.ZoneEvolutionBars)
                {
                    snapshot[$"zone:north:{zone.Key}"] = zone.Value.Value;
                }
            }

            if (Regions.TryGetValue(RegionId.DesertKingdom, out var desert)
                && desert.ZoneEvolutionBars.TryGetValue("MerchantsRepublic", out var merchants))
            {
                snapshot["zone:desert:MerchantsRepublic"] = merchants.Value;
            }

            var settlements = Regions.Values.SelectMany(r => r.Settlements.Values).ToList();
            if (settlements.Count > 0)
            {
                snapshot["town:avg:Wealth"] = settlements.Average(s => s.Wealth);
                snapshot["town:avg:Safety"] = settlements.Average(s => s.Safety);
                snapshot["town:avg:Fear"] = settlements.Average(s => s.Fear);
                snapshot["town:avg:Hope"] = settlements.Average(s => s.Hope);
                snapshot["town:avg:Faith"] = settlements.Average(s => s.Faith);
            }

            snapshot["unlocks:temporary"] = TemporaryEncounterUnlocks.Count;
            snapshot["unlocks:permanent"] = UnlockedEncounterIds.Count;

            return snapshot;
        }

        private void LogWorldDeltaSummary(Dictionary<string, double> before, Dictionary<string, double> after, string context)
        {
            var deltas = new List<(string Key, double Before, double After, double Delta)>();
            foreach (var key in before.Keys)
            {
                if (!after.TryGetValue(key, out var afterValue))
                    continue;

                var beforeValue = before[key];
                var delta = afterValue - beforeValue;
                if (Math.Abs(delta) > 0.01)
                {
                    deltas.Add((key, beforeValue, afterValue, delta));
                }
            }

            if (deltas.Count == 0)
                return;

            Log(LogCategory.Simulation, $"[WORLD DELTA] {context}", ConsoleColor.Black);
            foreach (var delta in deltas.OrderByDescending(d => Math.Abs(d.Delta)).Take(12))
            {
                Log(LogCategory.Simulation, $"  -> {delta.Key}: {delta.Before:F2} -> {delta.After:F2} ({delta.Delta:+0.00;-0.00;0.00})", ConsoleColor.Black);
            }
        }

        public WorldState()
        {
            Clock = new WorldClock();
            Player = new PlayerState { CurrentRegion = RegionId.NorthernRealm, Health = 100, MaxHealth = 100, Gold = SimulationConfig.DefaultStartingGold };
            Economy = new EconomyState();
        }

        private void InitializeEconomy()
        {
            if (Rng == null) return;
            
            foreach (var item in ItemRegistry.AllItems)
            {
                var basePrice = item.BaseValue * (100 + Rng.Next(-20, 20)) / 100.0;
                Economy.MarketPrices[item.Id] = basePrice;
            }
        }

        private void InitializeFactions()
        {
            foreach (var faction in FactionRegistry.AllFactions)
            {
                Factions[faction.Type] = faction;
                Player.Reputation[faction.Type] = 0;
            }
        }

        private void InitializeRegistries()
        {
            Encounters = EncounterRegistry.AllEncounters;
            LongTermEvents = LongTermEventRegistry.AllEvents;
            QuestDefinitions = QuestRegistry.AllQuests;
            UnlockedEncounterIds.Clear();
            TemporaryEncounterUnlocks.Clear();

            HiddenEventBars.Clear();
            foreach (var hiddenDef in HiddenWorldBarRegistry.HiddenBarDefinitions)
            {
                HiddenEventBars[hiddenDef.Id] = new EventBarState(hiddenDef);
            }

            UniqueNpcs.Clear();
            foreach (var npc in UniqueNpcRegistry.AllNpcs)
            {
                UniqueNpcs[npc.Id] = new NpcState(npc);
            }
        }

        public static WorldState CreateCampaign(ulong seed, RegionId startingRegion)
        {
            var world = new WorldState
            {
                Seed = seed,
                Rng = new DeterministicRandom(seed)
            };

            CurrentGlobalState = world;
            world.Log(LogCategory.Simulation, $"Campaign seeded with {seed} starting at {startingRegion}");

            // Initialize all 7 regions
            var regionIds = new[] { RegionId.NorthernRealm, RegionId.DarkForest, RegionId.DesertKingdom, RegionId.Swamp, RegionId.Highlands, RegionId.InfernalLands, RegionId.ArcaneEmpire };
            foreach (var regionId in regionIds)
            {
                var tier = world.Rng.Probability(SimulationConfig.StartingTier2Chance) ? WorldTier.Tier2 : WorldTier.Tier1;
                var definition = WorldDefinitions.GetDefinition(regionId);
                var regionState = new RegionState(definition, tier, EvolutionPath.None);

                // Initialize EventBars from EventBarRegistry
                foreach (var barDef in EventBarRegistry.AllEventBarDefinitions)
                {
                    regionState.EventBars[barDef.Id] = new EventBarState(barDef);
                }

                // Initialize ZoneEvolutionBars
                if (ZoneEvolutionBarRegistry.BarsByRegion.TryGetValue(regionId, out var zoneBarDefs))
                {
                    foreach (var zoneBarDef in zoneBarDefs)
                    {
                        regionState.ZoneEvolutionBars[zoneBarDef.Id] = new ZoneEvolutionBarState(zoneBarDef);
                    }
                }

                // Initialize Settlements
                foreach (var settlementDef in SettlementRegistry.AllSettlements)
                {
                    if (settlementDef.Region == regionId)
                    {
                        regionState.Settlements[settlementDef.Id] = new SettlementState(settlementDef);
                    }
                }

                world.Regions[regionId] = regionState;
            }

            // Initialize progression bars
            foreach (var barDef in ProgressionBarRegistry.GlobalBarDefinitions)
            {
                world.ProgressionBars[barDef.Id] = new ProgressionBarState(barDef);
            }

            // Initialize economy, factions, and registries
            world.InitializeEconomy();
            world.InitializeFactions();
            world.InitializeRegistries();

            world.Player.CurrentRegion = startingRegion;
            world.RefreshQuestLineAvailability();
            return world;

        }

        public void Log(LogCategory category, string message)
        {
            if (ShowLogs)
                SimulationLog.Log(message, category);
            SimulationHistory.Add(message);
        }

        public void Log(LogCategory category, string message, ConsoleColor color)
        {
            if (ShowLogs)
                SimulationLog.Log(message, category, color);
            SimulationHistory.Add(message);
        }

        private void LogFastFeature(LogCategory category, string message)
        {
            if (!IsFastMode)
                return;

            Log(category, message, FastFeatureColor);
        }

        public void AdvanceWorld(int minutes)
        {
            Clock.AdvanceByMinutes(minutes);
            UpdateProgression();
            UpdateMarketPrices();
        }

        private void UpdateProgression()
        {
            TicksSinceLastEvolution++;
            RefreshQuestLineAvailability();

            var dawnCooldownSources = _dawnSourceCooldowns.Keys.ToList();
            foreach (var source in dawnCooldownSources)
            {
                _dawnSourceCooldowns[source]--;
                if (_dawnSourceCooldowns[source] <= 0)
                {
                    _dawnSourceCooldowns.Remove(source);
                }
            }

            foreach (var unlock in TemporaryEncounterUnlocks.Values)
            {
                unlock.TurnsRemaining--;
            }

            var expiredUnlocks = TemporaryEncounterUnlocks.Where(kvp => kvp.Value.TurnsRemaining <= 0 || kvp.Value.UsesRemaining <= 0).Select(kvp => kvp.Key).ToList();
            foreach (var unlockId in expiredUnlocks)
            {
                TemporaryEncounterUnlocks.Remove(unlockId);
                LogFastFeature(LogCategory.Simulation, $"[ENCOUNTER UNLOCK EXPIRED] {unlockId}");
            }

            foreach (var region in Regions.Values)
            {
                foreach (var bar in region.EventBars.Values)
                {
                    bar.TickCooldown();
                    bar.ApplyDecay();
                    
                    bar.CheckCompletion();
                    if (bar.NeedsCompletionProcessing())
                    {
                        ProcessEventBarCompletion(bar);
                        bar.MarkCompletionProcessed();
                    }
                }
                
                foreach (var zoneBar in region.ZoneEvolutionBars.Values)
                {
                    zoneBar.ModificationHistory.Clear();
                }
            }

            foreach (var hiddenBar in HiddenEventBars.Values)
            {
                hiddenBar.TickCooldown();
                hiddenBar.ApplyDecay();
                hiddenBar.CheckCompletion();
                if (hiddenBar.NeedsCompletionProcessing())
                {
                    ProcessHiddenEventBarCompletion(hiddenBar);
                    if (hiddenBar.Definition.Repeatable)
                    {
                        hiddenBar.Reset();
                        LogFastFeature(LogCategory.Simulation, $"[HIDDEN RESET] {hiddenBar.Definition.Name} reset to 0 after consequence resolution.");
                    }
                    else
                    {
                        hiddenBar.MarkCompletionProcessed();
                    }
                }
            }

            UpdateUniqueNpcs();
            UpdateSettlementPersonalityFromHiddenBars();
        }

        private void ProcessEventBarCompletion(EventBarState bar)
        {
            var definition = bar.Definition;
            Log(LogCategory.Simulation, $"[EVENT COMPLETION] {definition.Name} reached threshold ({bar.Value:F0}/{definition.Threshold})", ConsoleColor.Green);

            switch (definition.Id)
            {
                case "MerchantCaravan":
                    ApplyHiddenInfluence("KingsStability", 1.5, "Merchant caravans secured");
                    break;
                case "BanditPatrol":
                    ApplyHiddenInfluence("CivilWar", 2.0, "Bandit influence surged");
                    break;
                case "TempleActivity":
                    ApplyHiddenInfluence("OldGods", 0.8, "Temple revival");
                    break;
                case "WildlifeEncroach":
                    ApplyHiddenInfluence("TheVeil", 1.0, "Wilderness pressure");
                    break;
            }
            
            foreach (var encounterId in definition.OnCompletion.UnlockedEncounterIds)
            {
                Log(LogCategory.Simulation, $"  → Unlocks encounter: {encounterId}");
            }

            foreach (var questLineId in definition.OnCompletion.UnlockedQuestLineIds)
            {
                Log(LogCategory.Simulation, $"  → Unlocks questline: {questLineId}");
                LogQuestlineAvailability(questLineId, definition.Name);
            }

            foreach (var (settlementId, effectType, amount) in definition.OnCompletion.SettlementModifications)
            {
                var region = Regions.Values.FirstOrDefault(r => r.Settlements.ContainsKey(settlementId));
                if (region?.Settlements.TryGetValue(settlementId, out var settlement) == true)
                {
                    Log(LogCategory.Simulation, $"  → {settlement.Definition.Name} {effectType}: +{amount}");
                    ApplySettlementEffect(settlement, effectType, amount);
                }
            }
        }

        private void ProcessHiddenEventBarCompletion(EventBarState hiddenBar)
        {
            var definition = hiddenBar.Definition;
            LogFastFeature(LogCategory.Simulation, $"[HIDDEN EVENT COMPLETION] {definition.Name} reached threshold ({hiddenBar.Value:F0}/{definition.Threshold})");

            definition.OnThresholdReached?.Invoke(this);

            foreach (var worldEffect in definition.OnCompletion.WorldEffects)
            {
                Log(LogCategory.Simulation, $"  -> World: {worldEffect}", ConsoleColor.White);
            }

            foreach (var questId in definition.OnCompletion.UnlockedQuestIds)
            {
                _queuedRumorQuestIds.Enqueue(questId);
                Log(LogCategory.Simulation, $"  -> Unlocks quest: {questId}", ConsoleColor.White);
            }

            foreach (var questLineId in definition.OnCompletion.UnlockedQuestLineIds)
            {
                Log(LogCategory.Simulation, $"  -> Unlocks questline: {questLineId}", ConsoleColor.White);
                LogQuestlineAvailability(questLineId, definition.Name);
            }

            foreach (var encounterId in definition.OnCompletion.UnlockedEncounterIds)
            {
                UnlockConsequenceEncounter(encounterId, uses: 1, turns: 45);
                Log(LogCategory.Simulation, $"  -> Unlocks encounter: {encounterId}", ConsoleColor.White);
            }

            foreach (var (settlementId, effectType, amount) in definition.OnCompletion.SettlementModifications)
            {
                var region = Regions.Values.FirstOrDefault(r => r.Settlements.ContainsKey(settlementId));
                if (region?.Settlements.TryGetValue(settlementId, out var settlement) != true)
                    continue;

                ApplySettlementEffect(settlement, effectType, amount);
                Log(LogCategory.Simulation, $"  -> Town {settlement.Definition.Name}: {effectType} {amount:+0.0;-0.0;0.0}", ConsoleColor.White);
            }

            foreach (var (zoneBarId, amount) in definition.OnCompletion.ZoneEvolutionInfluences)
            {
                if (TryResolveZoneBarTarget(zoneBarId, out var targetRegionId, out var zoneBar))
                {
                    var before = zoneBar.Value;
                    zoneBar.ModifyValue(amount, $"{definition.Name} threshold");
                    var after = zoneBar.Value;
                    Log(LogCategory.Simulation, $"  -> Zone {zoneBar.Definition.Name}: {before:F1} -> {after:F1}", ConsoleColor.White);
                    LogZoneProgressChange(targetRegionId, zoneBar, before, after);
                }
            }

            if (definition.PossibleOutcomes.Count > 0)
            {
                var outcome = SelectHiddenOutcome(definition.PossibleOutcomes);
                if (outcome != null)
                {
                    ApplyHiddenOutcome(definition, outcome);
                }
            }
        }

        private HiddenEventOutcome SelectHiddenOutcome(List<HiddenEventOutcome> outcomes)
        {
            if (outcomes == null || outcomes.Count == 0)
                return null;

            var totalWeight = outcomes.Sum(o => Math.Max(0.01, o.Weight));
            var roll = Rng.NextDouble() * totalWeight;
            double accumulated = 0;
            foreach (var outcome in outcomes)
            {
                accumulated += Math.Max(0.01, outcome.Weight);
                if (roll <= accumulated)
                    return outcome;
            }

            return outcomes[^1];
        }

        private void UnlockConsequenceEncounter(string encounterId, int uses = 1, int turns = 30)
        {
            if (TemporaryEncounterUnlocks.TryGetValue(encounterId, out var state))
            {
                state.UsesRemaining = Math.Max(state.UsesRemaining, uses);
                state.TurnsRemaining = Math.Max(state.TurnsRemaining, turns);
            }
            else
            {
                TemporaryEncounterUnlocks[encounterId] = new EncounterUnlockState
                {
                    UsesRemaining = uses,
                    TurnsRemaining = turns
                };
            }

            LogFastFeature(LogCategory.Simulation, $"[ENCOUNTER UNLOCKED] {encounterId} (uses={uses}, ttl={turns})");
        }

        private void ApplyHiddenOutcome(EventBarDefinition sourceBar, HiddenEventOutcome outcome)
        {
            var beforeSnapshot = CaptureWorldDeltaSnapshot();

            LogFastFeature(LogCategory.Simulation, $"[HIDDEN OUTCOME] {sourceBar.Name} -> {outcome.Name}");
            Log(LogCategory.Simulation, $"  -> Outcome: {outcome.Description}", ConsoleColor.White);

            foreach (var encounterId in outcome.UnlockedEncounterIds)
            {
                UnlockConsequenceEncounter(encounterId);
                Log(LogCategory.Simulation, $"  -> Unlocks encounter: {encounterId}", ConsoleColor.White);
            }

            foreach (var questId in outcome.UnlockedQuestIds)
            {
                _queuedRumorQuestIds.Enqueue(questId);
                Log(LogCategory.Simulation, $"  -> Unlocks quest: {questId}", ConsoleColor.White);
            }

            foreach (var worldEffect in outcome.WorldEffects)
            {
                Log(LogCategory.Simulation, $"  -> World: {worldEffect}", ConsoleColor.White);
            }

            foreach (var (settlementId, effectType, amount) in outcome.SettlementModifications)
            {
                var region = Regions.Values.FirstOrDefault(r => r.Settlements.ContainsKey(settlementId));
                if (region?.Settlements.TryGetValue(settlementId, out var settlement) != true)
                    continue;

                ApplySettlementEffect(settlement, effectType, amount);
                Log(LogCategory.Simulation, $"  -> Town {settlement.Definition.Name}: {effectType} {amount:+0.0;-0.0;0.0}", ConsoleColor.White);
            }

            foreach (var (zoneBarId, amount) in outcome.ZoneEvolutionInfluences)
            {
                if (!TryResolveZoneBarTarget(zoneBarId, out var targetRegionId, out var zoneBar))
                    continue;

                var before = zoneBar.Value;
                zoneBar.ModifyValue(amount, outcome.Name);
                var after = zoneBar.Value;
                Log(LogCategory.Simulation, $"  -> Zone {zoneBar.Definition.Name}: {before:F1} -> {after:F1}", ConsoleColor.White);
                LogZoneProgressChange(targetRegionId, zoneBar, before, after);
            }

            foreach (var (hiddenBarId, amount) in outcome.HiddenBarInfluences)
            {
                ApplyHiddenInfluence(hiddenBarId, amount, outcome.Name);
            }

            foreach (var (regionId, eventBarId, amount) in outcome.EventBarInfluences)
            {
                ApplyEventInfluence(regionId, eventBarId, amount, outcome.Name);
                if (Regions.TryGetValue(regionId, out var region) && region.EventBars.TryGetValue(eventBarId, out var eventBar))
                {
                    Log(LogCategory.Simulation, $"  -> Event {eventBar.Definition.Name}: {eventBar.Value:F1} / {eventBar.Definition.Threshold:F0}", ConsoleColor.White);
                }
            }

            var afterSnapshot = CaptureWorldDeltaSnapshot();
            LogWorldDeltaSummary(beforeSnapshot, afterSnapshot, $"{sourceBar.Name} -> {outcome.Name}");
        }

        private bool TryResolveZoneBarTarget(string zoneBarId, out RegionId regionId, out ZoneEvolutionBarState zoneBar)
        {
            foreach (var region in Regions)
            {
                if (region.Value.ZoneEvolutionBars.TryGetValue(zoneBarId, out var foundZoneBar))
                {
                    regionId = region.Key;
                    zoneBar = foundZoneBar;
                    return true;
                }
            }

            regionId = default;
            zoneBar = null;
            return false;
        }

        private void ApplySettlementEffect(SettlementState settlement, string effectType, double amount)
        {
            if (effectType == "Wealth") settlement.ModifyWealth(amount);
            else if (effectType == "Safety") settlement.ModifySafety(amount);
            else if (effectType == "MerchantActivity") settlement.ModifyMerchantActivity(amount);
            else settlement.ModifyPersonality(effectType, amount);
        }

        private static string DescribeZoneMilestone(double threshold)
        {
            return threshold switch
            {
                25 => "Milestone I",
                50 => "Milestone II",
                75 => "Milestone III",
                100 => "Milestone IV",
                _ => "Milestone"
            };
        }

        private static bool CrossedThreshold(double previous, double current, double threshold)
        {
            return previous < threshold && current >= threshold;
        }

        private static bool ShouldLogZoneProgress(RegionId regionId, string zoneBarId)
        {
            if (regionId == RegionId.NorthernRealm)
                return true;

            return regionId == RegionId.DesertKingdom && zoneBarId == "MerchantsRepublic";
        }

        private void LogZoneProgressChange(RegionId regionId, ZoneEvolutionBarState zoneBar, double before, double after)
        {
            if (!ShouldLogZoneProgress(regionId, zoneBar.Definition.Id))
                return;

            var delta = after - before;
            if (Math.Abs(delta) <= 0.01)
                return;

            var regionName = Regions.TryGetValue(regionId, out var region) ? region.Definition.Name : regionId.ToString();
            Log(
                LogCategory.Simulation,
                $"[ZONE PROGRESS] [{regionName}] {zoneBar.Definition.Name}: {before:F1} -> {after:F1} ({delta:+0.0;-0.0;0.0})",
                ConsoleColor.Red);

            foreach (var threshold in ZoneMilestoneThresholds)
            {
                if (CrossedThreshold(before, after, threshold))
                {
                    Log(
                        LogCategory.Simulation,
                        $"[MILESTONE] [{regionName}] {zoneBar.Definition.Name} reached {DescribeZoneMilestone(threshold)} at {threshold:F0}.",
                        ConsoleColor.Blue);
                }
            }
        }

        private void LogEventBarProgressChange(RegionId regionId, EventBarState eventBar, double before, double after)
        {
            var delta = after - before;
            if (Math.Abs(delta) <= 0.01)
                return;

            var regionName = Regions.TryGetValue(regionId, out var region) ? region.Definition.Name : regionId.ToString();
            Log(
                LogCategory.Simulation,
                $"[EVENT BAR] [{regionName}] {eventBar.Definition.Name}: {before:F1} -> {after:F1} ({delta:+0.0;-0.0;0.0}, {after:F1} / {eventBar.Definition.Threshold:F0})",
                ConsoleColor.Yellow);
        }

        private bool ApplyZoneInfluence(RegionId regionId, string zoneBarId, double amount, string source)
        {
            if (!Regions.TryGetValue(regionId, out var region))
                return false;

            if (!region.ZoneEvolutionBars.TryGetValue(zoneBarId, out var zoneBar))
                return false;

            if (zoneBarId == "DawnOfGodsHidden")
            {
                var isSync = source.StartsWith("[SYNC] ", StringComparison.OrdinalIgnoreCase);
                if (isSync)
                {
                    source = source.Substring("[SYNC] ".Length);
                }
                else
                {
                    if (!TryNormalizeDawnInfluence(amount, source, out var normalizedAmount))
                        return false;

                    amount = normalizedAmount;
                }
            }

            var before = zoneBar.Value;
            zoneBar.ModifyValue(amount, source);
            var after = zoneBar.Value;
            var changed = Math.Abs(after - before) > 0.01;

            if (changed && IsFastMode)
            {
                Log(LogCategory.Player, $"[INFLUENCE] {source} changed zone evolution.", ConsoleColor.Magenta);
                LogZoneProgressChange(regionId, zoneBar, before, after);
            }

            return changed;
        }

        private bool ApplyEventInfluence(RegionId regionId, string eventBarId, double amount, string source)
        {
            if (!Regions.TryGetValue(regionId, out var region))
                return false;

            if (!region.EventBars.TryGetValue(eventBarId, out var eventBar))
                return false;

            var before = eventBar.Value;
            eventBar.AddProgress(amount);
            var after = eventBar.Value;
            var changed = Math.Abs(after - before) > 0.01;

            if (changed && IsFastMode)
            {
                Log(LogCategory.Player, $"[INFLUENCE] {source} changed world state.", ConsoleColor.Magenta);
                LogEventBarProgressChange(regionId, eventBar, before, after);
            }

            if (changed)
            {
                ApplyHiddenInfluenceFromEventBarDelta(eventBarId, after - before);
            }

            return changed;
        }

        private static double QuantizeDawnPositive(double amount)
        {
            if (amount <= 0.12) return 0.1;
            if (amount <= 0.28) return 0.2;
            if (amount <= 0.65) return 0.5;
            if (amount <= 0.90) return 0.8;
            return 1.0;
        }

        private bool IsMythicDawnSource(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            if (source.StartsWith("Quest: ", StringComparison.OrdinalIgnoreCase))
            {
                var questTitle = source.Substring("Quest: ".Length).Trim();
                var quest = QuestDefinitions.FirstOrDefault(q => string.Equals(q.Title, questTitle, StringComparison.OrdinalIgnoreCase));
                return quest != null && (DawnMythicQuestIds.Contains(quest.Id) || QuestLineRegistry.AllQuestLines.Any(ql => DawnMythicQuestLineIds.Contains(ql.Id) && ql.QuestIds.Contains(quest.Id)));
            }

            if (source.StartsWith("Questline: ", StringComparison.OrdinalIgnoreCase) || source.StartsWith("Legendary questline:", StringComparison.OrdinalIgnoreCase))
            {
                var questLineLabel = source.Contains(":")
                    ? source.Substring(source.IndexOf(":", StringComparison.Ordinal) + 1).Trim()
                    : source.Trim();

                return QuestLineRegistry.AllQuestLines.Any(questLine =>
                    DawnMythicQuestLineIds.Contains(questLine.Id)
                    && (
                        string.Equals(questLine.Id, questLineLabel, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(questLine.Title, questLineLabel, StringComparison.OrdinalIgnoreCase)));
            }

            var normalized = source.ToLowerInvariant();
            return DawnMythicSourceKeywords.Any(keyword => normalized.Contains(keyword));
        }

        private bool AreQuestLinePrerequisitesMet(QuestLineDefinition questLine)
        {
            return questLine.PrerequisiteQuestLineIds.All(id => _completedQuestLineIds.Contains(id));
        }

        private QuestLineState GetQuestLineState(string questLineId)
        {
            return Player.ActiveQuestLines.FirstOrDefault(state => string.Equals(state.Definition.Id, questLineId, StringComparison.OrdinalIgnoreCase));
        }

        private void RefreshQuestLineAvailability()
        {
            foreach (var questLine in QuestLineRegistry.AllQuestLines)
            {
                if (_completedQuestLineIds.Contains(questLine.Id))
                    continue;

                if (GetQuestLineState(questLine.Id) != null)
                    continue;

                if (!questLine.CanStart(this) || !AreQuestLinePrerequisitesMet(questLine))
                    continue;

                Player.ActiveQuestLines.Add(new QuestLineState(questLine));
                LogQuestlineAvailability(questLine.Id, "prerequisites met");
            }
        }

        private void LogQuestLineProgress(QuestLineState questLineState, string phase, QuestDefinition quest = null)
        {
            if (questLineState == null)
                return;

            var total = questLineState.Definition.QuestIds.Count;
            var currentIndex = Math.Min(questLineState.CurrentQuestIndex, total);
            var currentQuestId = questLineState.GetCurrentQuestId();
            var unmetPrereqs = questLineState.Definition.PrerequisiteQuestLineIds.Where(id => !_completedQuestLineIds.Contains(id)).ToList();
            var nextOptions = questLineState.Definition.QuestIds.Skip(currentIndex).Take(3).ToList();

            LogQuestlineEvent($"{phase}: {questLineState.Definition.Title}");
            if (quest != null)
                LogQuestlineEvent($"Trigger quest: {quest.Title}");
            LogQuestlineEvent($"Progress {currentIndex}/{total}");
            LogQuestlineEvent(unmetPrereqs.Count == 0
                ? "Prerequisites: satisfied"
                : $"Prerequisites: waiting on {string.Join(", ", unmetPrereqs)}");
            LogQuestlineEvent(nextOptions.Count == 0
                ? "Next options: complete"
                : $"Next options: {string.Join(" | ", nextOptions)}");

            if (!string.IsNullOrWhiteSpace(currentQuestId) && !questLineState.Completed)
            {
                LogQuestlineEvent($"Current quest: {currentQuestId}");
            }
        }

        private bool TryAdvanceQuestLineProgress(QuestDefinition quest)
        {
            var advancedAny = false;
            foreach (var questLineState in Player.ActiveQuestLines.Where(state => !state.Completed && state.Definition.QuestIds.Contains(quest.Id)).ToList())
            {
                var expectedQuestId = questLineState.GetCurrentQuestId();
                if (!string.Equals(expectedQuestId, quest.Id, StringComparison.OrdinalIgnoreCase))
                {
                    LogQuestlineEvent($"Skipped out-of-sequence quest in {questLineState.Definition.Title}: expected {expectedQuestId}, got {quest.Id}");
                    continue;
                }

                questLineState.CompletedQuestIds.Add(quest.Id);
                questLineState.AdvanceQuest();
                advancedAny = true;

                LogQuestLineProgress(questLineState, "Completed step", quest);

                if (DawnMythicQuestLineIds.Contains(questLineState.Definition.Id) && !questLineState.Completed)
                {
                    ApplyHiddenInfluence("DawnOfGods", 0.2, $"Questline: {questLineState.Definition.Title}");
                }

                if (questLineState.Completed)
                {
                    _completedQuestLineIds.Add(questLineState.Definition.Id);
                    LogQuestlineEvent($"Completed: {questLineState.Definition.Title}");

                    if (DawnMythicQuestLineIds.Contains(questLineState.Definition.Id))
                    {
                        ApplyHiddenInfluence("DawnOfGods", 1.0, $"Questline: {questLineState.Definition.Title}");
                    }

                    if (questLineState.Definition.FinalReward.Gold.HasValue)
                    {
                        var beforeGold = Player.Gold;
                        Player.Gold += questLineState.Definition.FinalReward.Gold.Value;
                        LogQuestlineEvent($"Final reward gold: {beforeGold:F0} -> {Player.Gold:F0}");
                    }

                    foreach (var rep in questLineState.Definition.FinalReward.ReputationChanges)
                    {
                        Player.ModifyReputation(rep.Key, rep.Value);
                        LogQuestlineEvent($"Final reputation: {rep.Key} {rep.Value:+0.0;-0.0;0.0}");
                    }

                    foreach (var unlockedId in questLineState.Definition.FinalReward.UnlockedQuestLineIds)
                    {
                        LogQuestlineEvent($"Unlocks questline: {unlockedId}");
                    }

                    questLineState.Definition.OnComplete?.Invoke(this);
                    RefreshQuestLineAvailability();
                }
            }

            return advancedAny;
        }

        private bool TryNormalizeDawnInfluence(double requestedAmount, string source, out double normalizedAmount)
        {
            normalizedAmount = 0;
            if (Math.Abs(requestedAmount) <= 0.01)
                return false;

            if (!string.IsNullOrWhiteSpace(source)
                && source.Contains("Legendary questline", StringComparison.OrdinalIgnoreCase)
                && requestedAmount > 0)
            {
                normalizedAmount = Math.Min(5.0, requestedAmount);
                return true;
            }

            if (!IsMythicDawnSource(source))
            {
                if (IsFastMode)
                {
                    Log(LogCategory.Simulation, $"[DAWN FILTER] Ignored non-mythic source: {source}", ConsoleColor.White);
                }

                return false;
            }

            if (requestedAmount > 0)
            {
                var cooldownKey = source.Trim().ToLowerInvariant();
                if (_dawnSourceCooldowns.TryGetValue(cooldownKey, out var remaining) && remaining > 0)
                {
                    if (IsFastMode)
                    {
                        Log(LogCategory.Simulation, $"[DAWN FILTER] Cooldown blocked repeat source: {source} ({remaining} turns left)", ConsoleColor.White);
                    }

                    return false;
                }

                var quantized = QuantizeDawnPositive(Math.Min(1.0, requestedAmount));
                normalizedAmount = quantized;
                _dawnSourceCooldowns[cooldownKey] = DawnSourceCooldownTurns;
                return true;
            }

            normalizedAmount = -Math.Min(2.0, Math.Abs(requestedAmount));
            return true;
        }

        private void LogQuestlineEvent(string message)
        {
            Log(LogCategory.Simulation, $"[QUESTLINE EVENT] {message}", ConsoleColor.Cyan);
        }

        private void LogQuestlineStatusForQuest(string phase, QuestDefinition quest)
        {
            if (quest == null)
                return;

            var relatedQuestlines = QuestLineRegistry.AllQuestLines.Where(ql => ql.QuestIds.Contains(quest.Id)).ToList();
            foreach (var questline in relatedQuestlines)
            {
                var completedCount = questline.QuestIds.Count(id => _completedQuestIds.Contains(id));
                var total = questline.QuestIds.Count;
                var unmetPrereqs = questline.PrerequisiteQuestLineIds.Where(id => !_completedQuestLineIds.Contains(id)).ToList();
                var nextOptions = questline.QuestIds.Where(id => !_completedQuestIds.Contains(id)).Take(3).ToList();

                LogQuestlineEvent($"{phase}: {questline.Title}");
                LogQuestlineEvent($"Progress {completedCount}/{total} | Trigger quest: {quest.Title}");
                LogQuestlineEvent(unmetPrereqs.Count == 0
                    ? "Prerequisites: satisfied"
                    : $"Prerequisites: waiting on {string.Join(", ", unmetPrereqs)}");
                LogQuestlineEvent(nextOptions.Count == 0
                    ? "Next options: complete"
                    : $"Next options: {string.Join(" | ", nextOptions)}");

                if (completedCount < total || unmetPrereqs.Count > 0 || _completedQuestLineIds.Contains(questline.Id))
                    continue;

                _completedQuestLineIds.Add(questline.Id);
                LogQuestlineEvent($"Completed: {questline.Title}");

                if (questline.FinalReward.Gold.HasValue)
                {
                    var beforeGold = Player.Gold;
                    Player.Gold += questline.FinalReward.Gold.Value;
                    LogQuestlineEvent($"Final reward gold: {beforeGold:F0} -> {Player.Gold:F0}");
                }

                foreach (var rep in questline.FinalReward.ReputationChanges)
                {
                    Player.ModifyReputation(rep.Key, rep.Value);
                    LogQuestlineEvent($"Final reputation: {rep.Key} {rep.Value:+0.0;-0.0;0.0}");
                }

                foreach (var unlockedId in questline.FinalReward.UnlockedQuestLineIds)
                {
                    LogQuestlineEvent($"Unlocks questline: {unlockedId}");
                }

                questline.OnComplete?.Invoke(this);
            }
        }

        private void LogQuestlineAvailability(string questLineId, string reason)
        {
            var questline = QuestLineRegistry.AllQuestLines.FirstOrDefault(ql => ql.Id == questLineId);
            if (questline == null)
                return;

            var unmetPrereqs = questline.PrerequisiteQuestLineIds.Where(id => !_completedQuestLineIds.Contains(id)).ToList();
            var nextOptions = questline.QuestIds.Where(id => !_completedQuestIds.Contains(id)).Take(3).ToList();
            LogQuestlineEvent($"Unlocked by {reason}: {questline.Title}");
            LogQuestlineEvent(unmetPrereqs.Count == 0
                ? "Prerequisites: satisfied"
                : $"Prerequisites: waiting on {string.Join(", ", unmetPrereqs)}");
            LogQuestlineEvent(nextOptions.Count == 0
                ? "Next options: complete"
                : $"Next options: {string.Join(" | ", nextOptions)}");
        }

        public bool ApplyHiddenInfluence(string hiddenBarId, double amount, string source)
        {
            if (!HiddenEventBars.TryGetValue(hiddenBarId, out var hiddenBar))
                return false;

            if (hiddenBarId == "DawnOfGods")
            {
                if (!TryNormalizeDawnInfluence(amount, source, out var normalizedAmount))
                    return false;

                amount = normalizedAmount;
            }

            var before = hiddenBar.Value;
            hiddenBar.AddProgress(amount);
            var after = hiddenBar.Value;
            var changed = Math.Abs(after - before) > 0.01;

            if (changed)
            {
                if (hiddenBarId == "DawnOfGods")
                {
                    ApplyZoneInfluence(RegionId.NorthernRealm, "DawnOfGodsHidden", amount, $"[SYNC] {source}");
                }

                hiddenBar.CheckCompletion();
                if (hiddenBar.NeedsCompletionProcessing())
                {
                    ProcessHiddenEventBarCompletion(hiddenBar);
                    if (hiddenBar.Definition.Repeatable)
                    {
                        hiddenBar.Reset();
                        LogFastFeature(LogCategory.Simulation, $"[HIDDEN RESET] {hiddenBar.Definition.Name} reset to 0 after consequence resolution.");
                    }
                    else
                    {
                        hiddenBar.MarkCompletionProcessed();
                    }
                }

                if (IsFastMode)
                {
                    LogFastFeature(LogCategory.Simulation, $"[HIDDEN] {hiddenBar.Definition.Name}: {before:F2} -> {after:F2} ({source})");
                }
            }

            return changed;
        }

        private void ApplyHiddenInfluenceFromEventBarDelta(string eventBarId, double delta)
        {
            if (Math.Abs(delta) <= 0.01)
                return;

            switch (eventBarId)
            {
                case "MerchantCaravan":
                    if (delta > 0)
                    {
                        ApplyHiddenInfluence("KingsStability", delta * 0.05, "Trade growth stabilizes the crown");
                    }
                    else
                    {
                        ApplyHiddenInfluence("CivilWar", Math.Abs(delta) * 0.20, "Merchant network decayed");
                        ApplyHiddenInfluence("KingsStability", -Math.Abs(delta) * 0.10, "Trade collapse weakens the crown");
                    }
                    break;
                case "BanditPatrol":
                    if (delta > 0)
                    {
                        ApplyHiddenInfluence("CivilWar", delta * 0.20, "Bandit pressure destabilizes the realm");
                        ApplyHiddenInfluence("KingsStability", -delta * 0.08, "Law and order weakened");
                    }
                    else
                    {
                        ApplyHiddenInfluence("KingsStability", Math.Abs(delta) * 0.05, "Road safety restored");
                    }
                    break;
                case "TempleActivity":
                    if (delta > 0)
                    {
                        ApplyHiddenInfluence("OldGods", delta * 0.03, "Ancient prayers remembered");
                    }
                    break;
                case "RangerActivity":
                    if (delta > 0)
                    {
                        ApplyHiddenInfluence("KingsStability", delta * 0.03, "Ranger patrols calm borderlands");
                    }
                    break;
                case "WildlifeEncroach":
                    if (delta > 0)
                    {
                        ApplyHiddenInfluence("TheVeil", delta * 0.04, "Strange beasts gather near settlements");
                    }
                    break;
            }
        }

        private void UpdateUniqueNpcs()
        {
            if (UniqueNpcs.TryGetValue("BjornOneEyed", out var bjorn)
                && !bjorn.IsAlive
                && !string.IsNullOrWhiteSpace(bjorn.Definition.SuccessorId)
                && UniqueNpcs.TryGetValue(bjorn.Definition.SuccessorId, out var successor)
                && !successor.IsAlive)
            {
                successor.IsAlive = true;
                successor.CurrentRole = "Ranger";
                LogFastFeature(LogCategory.Simulation, "[NPC] Leif Bjornson has taken his father's place as ranger.");
            }

            if (UniqueNpcs.TryGetValue("SisterElara", out var elara) && elara.IsAlive)
            {
                var divine = Regions.TryGetValue(RegionId.NorthernRealm, out var northern)
                             && northern.ZoneEvolutionBars.TryGetValue("DivineOrder", out var divineBar)
                    ? divineBar.Value
                    : 0;
                var ancient = HiddenEventBars.TryGetValue("AncientEvil", out var ancientBar)
                    ? ancientBar.Value
                    : 0;

                if (divine >= 60 && elara.CurrentRole == "Priestess")
                {
                    elara.CurrentRole = "High Priestess";
                    LogFastFeature(LogCategory.Simulation, "[NPC] Sister Elara rises as High Priestess.");
                }
                else if (ancient >= 45 && elara.CurrentRole != "Cult Leader")
                {
                    elara.CurrentRole = "Cult Leader";
                    LogFastFeature(LogCategory.Simulation, "[NPC] Sister Elara has embraced a hidden cult and now leads it.");
                }
            }
        }

        private void UpdateSettlementPersonalityFromHiddenBars()
        {
            var civilWar = HiddenEventBars.TryGetValue("CivilWar", out var civilWarBar) ? civilWarBar.Value : 0;
            var dawn = HiddenEventBars.TryGetValue("DawnOfGods", out var dawnBar) ? dawnBar.Value : 0;
            var ancient = HiddenEventBars.TryGetValue("AncientEvil", out var ancientBar) ? ancientBar.Value : 0;

            foreach (var settlement in Regions.Values.SelectMany(r => r.Settlements.Values))
            {
                settlement.ModifyPersonality("Fear", civilWar * 0.0008 + ancient * 0.0008);
                settlement.ModifyPersonality("Crime", civilWar * 0.0009);
                settlement.ModifyPersonality("Hope", -civilWar * 0.0008 + dawn * 0.0006);
                settlement.ModifyPersonality("Faith", dawn * 0.0007 - ancient * 0.0004);
                settlement.ModifyPersonality("Loyalty", -civilWar * 0.0006 + dawn * 0.0004);
                settlement.ModifyPersonality("Influence", dawn * 0.0003);
                settlement.ModifyPersonality("Military", civilWar * 0.0005);
            }
        }

        private void UpdateMarketPrices()
        {
            foreach (var itemId in Economy.MarketPrices.Keys.ToList())
            {
                var volatility = Rng.Next(-10, 10) / 100.0;
                Economy.MarketPrices[itemId] *= (1.0 + volatility * SimulationConfig.MarketVolatility);
            }
        }

        private int GetInventoryCount(string itemId)
        {
            return Player.Inventory.TryGetValue(itemId, out var count) ? count : 0;
        }

        private (int DistinctTypes, int TotalItems) GetCollectorProgress()
        {
            var distinct = 0;
            var total = 0;

            foreach (var collectorItemId in CollectorItemIds)
            {
                var count = GetInventoryCount(collectorItemId);
                if (count > 0)
                {
                    distinct++;
                    total += count;
                }
            }

            return (distinct, total);
        }

        private void LogCollectorProgressChange((int DistinctTypes, int TotalItems) before, string reason)
        {
            var after = GetCollectorProgress();
            if (after.DistinctTypes == before.DistinctTypes && after.TotalItems == before.TotalItems)
                return;

            LogFastFeature(
                LogCategory.Simulation,
                $"[COLLECTORS] {reason}: unique {before.DistinctTypes} -> {after.DistinctTypes} / {CollectorItemIds.Length}, total {before.TotalItems} -> {after.TotalItems}");
        }

        private void MaybeShareRumor()
        {
            if (WorldRumorRegistry.Rumors.Count == 0)
                return;

            if (!Rng.Probability(0.55))
                return;

            var rumor = WorldRumorRegistry.Rumors[Rng.Next(WorldRumorRegistry.Rumors.Count)];
            if (IsFastMode)
                LogFastFeature(LogCategory.Player, $"[RUMOR] People say... \"{rumor.Text}\"");
            else
                Log(LogCategory.Player, $"People say... \"{rumor.Text}\"");

            var isTrue = Rng.Probability(rumor.TruthChance);
            if (!isTrue)
                return;

            Player.AddItem("RumorFragment", 1);
            var key = string.IsNullOrWhiteSpace(rumor.RumorGroupId) ? rumor.Id : rumor.RumorGroupId;
            if (_rumorShardCounts.TryGetValue(key, out var current))
            {
                _rumorShardCounts[key] = current + 1;
            }
            else
            {
                _rumorShardCounts[key] = 1;
            }

            LogFastFeature(LogCategory.Player, $"[RUMOR] Shard progress for {key}: {_rumorShardCounts[key]} / {rumor.ShardsRequired}");

            if (!string.IsNullOrWhiteSpace(rumor.QuestUnlockId)
                && _rumorShardCounts[key] >= rumor.ShardsRequired
                && !_unlockedRumorQuestIds.Contains(rumor.QuestUnlockId))
            {
                _unlockedRumorQuestIds.Add(rumor.QuestUnlockId);
                _queuedRumorQuestIds.Enqueue(rumor.QuestUnlockId);
                LogFastFeature(LogCategory.Player, $"[RUMOR] Clues aligned and unlocked: {rumor.QuestUnlockId}");
            }

            if (_autoAcceptedQuest == null && _queuedRumorQuestIds.Count > 0)
            {
                var rumorQuestId = _queuedRumorQuestIds.Dequeue();
                var rumorQuest = QuestDefinitions.FirstOrDefault(q => q.Id == rumorQuestId);
                if (rumorQuest != null)
                {
                    _autoAcceptedQuest = rumorQuest;
                    LogFastFeature(LogCategory.Player, $"[RUMOR] Rumor shards reveal a lead: {_autoAcceptedQuest.Title}");
                }
            }
        }

        public void Explore()
        {
            Log(LogCategory.Player, "You venture forth into the surrounding land.");
            AdvanceWorld(SimulationConfig.ExploreMinutes);
            var encounterRoll = Rng.Probability(SimulationConfig.EncounterChance);
            if (encounterRoll)
            {
                var encounter = RollEncounter();
                if (encounter != null)
                {
                    Log(LogCategory.Player, $"Encounter: {encounter.Name} - {encounter.Summary}");
                    PresentEncounter(encounter);
                    return;
                }
            }

            // Try to discover a settlement by RNG
            var region = Regions[Player.CurrentRegion];
            var undiscoveredSettlements = region.Settlements.Values.Where(s => !s.Discovered).ToList();
            if (undiscoveredSettlements.Count > 0 && Rng.Probability(0.30))
            {
                var discoveredSettlement = undiscoveredSettlements[Rng.Next(undiscoveredSettlements.Count)];
                discoveredSettlement.Discover();
                Log(LogCategory.Player, $"Through your exploration, you discover the settlement of {discoveredSettlement.Definition.Name}! It has been added to your fast travel locations.");
                return;
            }

            var discoveryMessage = DiscoverLandmark();
            Log(LogCategory.Player, discoveryMessage);
        }

        private string DiscoverLandmark()
        {
            var region = Regions[Player.CurrentRegion];
            if (region.Definition.Biome == BiomeType.Forest && Rng.Probability(0.25))
                return "You discover an overgrown shrine hidden beneath the trees.";
            if (region.Definition.Biome == BiomeType.Mountain && Rng.Probability(0.20))
                return "You find a narrow pass with carvings of an old dragon cult.";
            if (region.Definition.Biome == BiomeType.Swamp && Rng.Probability(0.20))
                return "You stumble upon a flooded ruin with strange lights.";
            if (Rng.Probability(0.15))
                return "Your exploration reveals a small treasure cache of useful supplies.";
            return "The landscape shifts subtly as you move; nothing remarkable appears yet.";
        }

        private EncounterDefinition RollEncounter()
        {
            var region = Regions[Player.CurrentRegion];
            var validEncounters = Encounters.Where(e =>
                (string.IsNullOrWhiteSpace(e.RequiredUnlockId)
                 || UnlockedEncounterIds.Contains(e.RequiredUnlockId)
                 || (TemporaryEncounterUnlocks.TryGetValue(e.RequiredUnlockId, out var unlockState)
                     && unlockState.UsesRemaining > 0
                     && unlockState.TurnsRemaining > 0))
                && e.CanOccur(this)).ToList();
            if (validEncounters.Count == 0) return null;

            var totalWeight = validEncounters.Sum(e => e.Weight(this));
            var roll = Rng.NextDouble() * totalWeight;
            double accumulated = 0;

            foreach (var encounter in validEncounters)
            {
                accumulated += encounter.Weight(this);
                if (roll <= accumulated) return encounter;
            }

            return validEncounters.LastOrDefault();
        }

        private void PresentEncounter(EncounterDefinition encounter)
        {
            if (!ShowLogs || AutoResolveEncounters)
            {
                var autoChoice = encounter.Options[Rng.Next(encounter.Options.Count)];
                ResolveEncounter(encounter, autoChoice);
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"--- {encounter.Name} ---");
            Console.WriteLine(encounter.Summary);
            for (int i = 0; i < encounter.Options.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {encounter.Options[i].Description}");
            }

            while (true)
            {
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out var selection) && selection >= 1 && selection <= encounter.Options.Count)
                {
                    ResolveEncounter(encounter, encounter.Options[selection - 1]);
                    break;
                }

                Console.WriteLine("Invalid selection. Please enter the number of your choice.");
            }

            Console.WriteLine();
        }

        private void ResolveEncounter(EncounterDefinition encounter, EncounterOption option)
        {
            var regionForHidden = Regions[Player.CurrentRegion];
            var beforeEventValues = regionForHidden.EventBars.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);

            if (!IsFastMode)
            {
                var standardResult = option.Execute(this);
                Log(LogCategory.Player, standardResult.Message);

                if (!string.IsNullOrWhiteSpace(encounter.RequiredUnlockId)
                    && TemporaryEncounterUnlocks.TryGetValue(encounter.RequiredUnlockId, out var nonFastUnlock))
                {
                    nonFastUnlock.UsesRemaining--;
                    if (nonFastUnlock.UsesRemaining <= 0)
                    {
                        TemporaryEncounterUnlocks.Remove(encounter.RequiredUnlockId);
                        LogFastFeature(LogCategory.Simulation, $"[ENCOUNTER UNLOCK CONSUMED] {encounter.RequiredUnlockId}");
                    }
                }

                foreach (var kvp in regionForHidden.EventBars)
                {
                    var before = beforeEventValues.TryGetValue(kvp.Key, out var valueBefore) ? valueBefore : kvp.Value.Value;
                    var after = kvp.Value.Value;
                    if (Math.Abs(after - before) > 0.01)
                    {
                        ApplyHiddenInfluenceFromEventBarDelta(kvp.Key, after - before);
                    }
                }

                return;
            }

            var region = Regions[Player.CurrentRegion];
            var selectedRegionId = Player.CurrentRegion;
            var beforeZoneValues = region.ZoneEvolutionBars.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);
            var beforeMerchantsRepublic = Regions.TryGetValue(RegionId.DesertKingdom, out var desertBeforeState) && desertBeforeState.ZoneEvolutionBars.TryGetValue("MerchantsRepublic", out var merchantsBeforeBar)
                ? merchantsBeforeBar.Value
                : (double?)null;
            var collectorProgressBefore = GetCollectorProgress();
            var goldBefore = Player.Gold;
            var healthBefore = Player.Health;

            if (encounter.IsLegendary)
            {
                Log(LogCategory.Player, $"[LEGENDARY] {encounter.Name}", ConsoleColor.Yellow);
            }

            Log(LogCategory.Player, $"[ACTION] {encounter.Name}", ConsoleColor.Magenta);
            Log(LogCategory.Player, $"[OPTIONS] {string.Join(" | ", encounter.Options.Select(o => o.Description))}", ConsoleColor.Magenta);
            Log(LogCategory.Player, $"[CHOICE] {option.Description}", ConsoleColor.Magenta);

            var result = option.Execute(this);
            Log(LogCategory.Player, result.Message);

            var influenceLogged = false;

            foreach (var kvp in region.EventBars)
            {
                var before = beforeEventValues.TryGetValue(kvp.Key, out var valueBefore) ? valueBefore : kvp.Value.Value;
                var after = kvp.Value.Value;
                var delta = after - before;
                if (Math.Abs(delta) > 0.01)
                {
                    if (!influenceLogged)
                    {
                        Log(LogCategory.Player, "[INFLUENCE] Player choice changed world state.", ConsoleColor.Magenta);
                        influenceLogged = true;
                    }

                    LogEventBarProgressChange(selectedRegionId, kvp.Value, before, after);
                    ApplyHiddenInfluenceFromEventBarDelta(kvp.Key, delta);
                }
            }

            foreach (var kvp in region.ZoneEvolutionBars)
            {
                var zoneBarId = kvp.Key;
                if (!ShouldLogZoneProgress(selectedRegionId, zoneBarId))
                    continue;

                var before = beforeZoneValues.TryGetValue(zoneBarId, out var valueBefore) ? valueBefore : kvp.Value.Value;
                var after = kvp.Value.Value;
                var delta = after - before;
                if (Math.Abs(delta) <= 0.01)
                    continue;

                if (!influenceLogged)
                {
                    Log(LogCategory.Player, "[INFLUENCE] Player choice changed world state.", ConsoleColor.Magenta);
                    influenceLogged = true;
                }

                LogZoneProgressChange(selectedRegionId, kvp.Value, before, after);
            }

            if (Regions.TryGetValue(RegionId.DesertKingdom, out var desertAfterState)
                && desertAfterState.ZoneEvolutionBars.TryGetValue("MerchantsRepublic", out var merchantsRepublicAfter)
                && beforeMerchantsRepublic.HasValue)
            {
                var delta = merchantsRepublicAfter.Value - beforeMerchantsRepublic.Value;
                if (Math.Abs(delta) > 0.01)
                {
                    if (!influenceLogged)
                    {
                        Log(LogCategory.Player, "[INFLUENCE] Player choice changed world state.", ConsoleColor.Magenta);
                        influenceLogged = true;
                    }

                    LogZoneProgressChange(RegionId.DesertKingdom, merchantsRepublicAfter, beforeMerchantsRepublic.Value, merchantsRepublicAfter.Value);
                }
            }

            if (Regions.TryGetValue(RegionId.NorthernRealm, out var northernRealm))
            {
                foreach (var northernZoneBar in northernRealm.ZoneEvolutionBars.Values.OrderBy(z => z.Definition.Name))
                {
                    var label = northernZoneBar.Definition.Id == "DawnOfGodsHidden"
                        ? "[NORTHERN PATH HIDDEN]"
                        : "[NORTHERN PATH]";
                    Log(
                        LogCategory.Simulation,
                        $"{label} {northernZoneBar.Definition.Name}: {northernZoneBar.Value:F1} / {northernZoneBar.Definition.MaxValue:F0}",
                        ConsoleColor.Red);
                }
            }

            if (Regions.TryGetValue(RegionId.DesertKingdom, out var desertRealm)
                && desertRealm.ZoneEvolutionBars.TryGetValue("MerchantsRepublic", out var merchantPath))
            {
                Log(
                    LogCategory.Simulation,
                    $"[MERCHANT PATH] {merchantPath.Definition.Name}: {merchantPath.Value:F1} / {merchantPath.Definition.MaxValue:F0}",
                    ConsoleColor.Red);
            }

            if (Math.Abs(Player.Gold - goldBefore) > 0.01)
            {
                if (!influenceLogged)
                {
                    Log(LogCategory.Player, "[INFLUENCE] Player choice changed world state.", ConsoleColor.Magenta);
                    influenceLogged = true;
                }

                Log(LogCategory.Player, $"[PLAYER EFFECT] Gold: {goldBefore:F0} -> {Player.Gold:F0} ({Player.Gold - goldBefore:+0;-0;0})", ConsoleColor.Magenta);
            }

            if (Math.Abs(Player.Health - healthBefore) > 0.01)
            {
                if (!influenceLogged)
                {
                    Log(LogCategory.Player, "[INFLUENCE] Player choice changed world state.", ConsoleColor.Magenta);
                    influenceLogged = true;
                }

                Log(LogCategory.Player, $"[PLAYER EFFECT] Health: {healthBefore:F0} -> {Player.Health:F0} ({Player.Health - healthBefore:+0;-0;0})", ConsoleColor.Magenta);
            }

            if (!influenceLogged)
            {
                Log(LogCategory.Player, "[CONSEQUENCE] not implemented", ConsoleColor.Magenta);
            }

            if (!string.IsNullOrWhiteSpace(encounter.RequiredUnlockId)
                && TemporaryEncounterUnlocks.TryGetValue(encounter.RequiredUnlockId, out var unlock))
            {
                unlock.UsesRemaining--;
                if (unlock.UsesRemaining <= 0)
                {
                    TemporaryEncounterUnlocks.Remove(encounter.RequiredUnlockId);
                    LogFastFeature(LogCategory.Simulation, $"[ENCOUNTER UNLOCK CONSUMED] {encounter.RequiredUnlockId}");
                }
            }

            LogCollectorProgressChange(collectorProgressBefore, $"after {encounter.Name}");
        }

        public void Rest()
        {
            var region = Regions[Player.CurrentRegion];
            var activities = GetAvailableRestActivities(region);
            Log(LogCategory.Player, "You take time to rest and recover.");
            foreach (var activity in activities)
            {
                Log(LogCategory.Player, $"- {activity}");
            }

            Player.Heal(SimulationConfig.RestHours * SimulationConfig.RestHealPerHour);
            Log(LogCategory.Player, $"You restore health to {Player.Health:F0}/{Player.MaxHealth}.");
            AdvanceWorld(SimulationConfig.RestHours * 60);
        }

        private List<string> GetAvailableRestActivities(RegionState region)
        {
            var activities = new List<string> { "Sleep", "Meditate", "Sharpen weapons" };
            if (region.Definition.HasWater)
                activities.Add("Fish");
            if (region.Definition.HasTown)
                activities.Add("Visit the tavern");
            activities.Add("Read a book");
            return activities;
        }

        public bool Travel(RegionId destination)
        {
            if (!Regions.ContainsKey(destination))
            {
                Log(LogCategory.Player, "That destination is not part of your current campaign.");
                return false;
            }

            if (destination == Player.CurrentRegion)
            {
                Log(LogCategory.Player, "You are already in that region.");
                return false;
            }

            var region = Regions[Player.CurrentRegion];
            Log(LogCategory.Player, $"You prepare to leave {region.Definition.Name}.");
            Log(LogCategory.Player, "You are about to leave this region. Returning will no longer be possible.");

            string input;
            if (AutoResolveEncounters)
            {
                input = "y";
            }
            else
            {
                Console.Write("Confirm travel? (y/n): ");
                input = Console.ReadLine()?.Trim().ToLower();
            }

            if (input != "y")
            {
                Log(LogCategory.Player, "You decide to stay for now.");
                return false;
            }

            region.IsLocked = true;
            Player.CurrentRegion = destination;
            Log(LogCategory.Player, $"You travel to {Regions[destination].Definition.Name}. It will continue evolving while you were away.");
            AdvanceWorld(SimulationConfig.ExploreMinutes);
            return true;
        }

        public void VisitTown()
        {
            var region = Regions[Player.CurrentRegion];
            var discoveredSettlements = region.Settlements.Values.Where(s => s.Discovered).ToList();
            
            if (discoveredSettlements.Count == 0)
            {
                Log(LogCategory.Player, "You have not yet discovered any settlements in this region. Explore to find towns!");
                return;
            }

            SettlementState selectedSettlement;
            if (discoveredSettlements.Count == 1)
            {
                selectedSettlement = discoveredSettlements[0];
                Log(LogCategory.Player, $"You fast travel to {selectedSettlement.Definition.Name}.");
            }
            else
            {
                if (!ShowLogs || AutoResolveEncounters)
                {
                    selectedSettlement = discoveredSettlements[Rng.Next(discoveredSettlements.Count)];
                    Log(LogCategory.Player, $"You fast travel to {selectedSettlement.Definition.Name}.");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Discovered Settlements:");
                    for (int i = 0; i < discoveredSettlements.Count; i++)
                    {
                        Console.WriteLine($"[{i + 1}] {discoveredSettlements[i].Definition.Name}");
                    }

                    while (true)
                    {
                        Console.Write("Choose destination: ");
                        var input = Console.ReadLine();
                        if (int.TryParse(input, out var selection) && selection >= 1 && selection <= discoveredSettlements.Count)
                        {
                            selectedSettlement = discoveredSettlements[selection - 1];
                            Log(LogCategory.Player, $"You fast travel to {selectedSettlement.Definition.Name}.");
                            break;
                        }

                        Console.WriteLine("Invalid selection. Please enter a valid settlement number.");
                    }

                    Console.WriteLine();
                }
            }

            Log(LogCategory.Player, "You arrive in town and listen for rumors.");
            AdvanceWorld(60);
            Log(LogCategory.Player, "Merchants haggle openly and faction agents watch for trouble.");
            Player.AddItem("RumorFragment", 1);
            MaybeShareRumor();

            if (ShowLogs && !AutoResolveEncounters)
                OfferTownQuests();
            else
                AutoAcceptTownQuests();

            HandleTurnInRequests();
            CompleteQuestsInTown();
        }

        private void HandleTurnInRequests() { }
        private void OfferTownQuests() => Log(LogCategory.Player, "The town is quiet today. No new opportunities present themselves.");
        private void AutoAcceptTownQuests()
        {
            if (_autoAcceptedQuest != null)
                return;

            RefreshQuestLineAvailability();

            if (_queuedRumorQuestIds.Count > 0)
            {
                var queuedQuestId = _queuedRumorQuestIds.Dequeue();
                var queuedQuest = QuestDefinitions.FirstOrDefault(q => q.Id == queuedQuestId);
                if (queuedQuest != null)
                {
                    _autoAcceptedQuest = queuedQuest;
                    LogFastFeature(LogCategory.Player, $"[RUMOR] Hidden quest accepted: {_autoAcceptedQuest.Title}");
                    var rumourQuestLineState = Player.ActiveQuestLines.FirstOrDefault(state => state.Definition.QuestIds.Contains(_autoAcceptedQuest.Id));
                    if (rumourQuestLineState != null)
                        LogQuestLineProgress(rumourQuestLineState, "Accepted step", _autoAcceptedQuest);
                    return;
                }
            }

            var activeQuestLineState = Player.ActiveQuestLines
                .Where(state => !state.Completed)
                .OrderByDescending(state => state.CurrentQuestIndex)
                .ThenBy(state => state.Definition.Id)
                .FirstOrDefault(state => !string.IsNullOrWhiteSpace(state.GetCurrentQuestId()));

            if (activeQuestLineState != null)
            {
                var questId = activeQuestLineState.GetCurrentQuestId();
                var questFromLine = QuestDefinitions.FirstOrDefault(q => q.Id == questId);
                if (questFromLine != null && questFromLine.CanStart(this))
                {
                    _autoAcceptedQuest = questFromLine;
                    Log(LogCategory.Player, $"[QUEST] Accepted: {_autoAcceptedQuest.Title}", ConsoleColor.Magenta);
                    LogQuestLineProgress(activeQuestLineState, "Accepted step", _autoAcceptedQuest);
                    return;
                }
            }

            var available = QuestDefinitions.Where(q => q.CanStart(this) && !QuestLineQuestIds.Contains(q.Id)).ToList();
            if (available.Count == 0)
                return;

            _autoAcceptedQuest = available[Rng.Next(available.Count)];
            Log(LogCategory.Player, $"[QUEST] Accepted: {_autoAcceptedQuest.Title}", ConsoleColor.Magenta);
        }

        public void VisitMarketplace()
        {
            var region = Regions[Player.CurrentRegion];
            var discoveredSettlements = region.Settlements.Values.Where(s => s.Discovered).ToList();
            
            if (discoveredSettlements.Count == 0)
            {
                Log(LogCategory.Player, "There is no marketplace here. You must discover a settlement first by exploring.");
                return;
            }

            var settlement = discoveredSettlements.Count == 1 
                ? discoveredSettlements[0]
                : discoveredSettlements[Rng.Next(discoveredSettlements.Count)];

            Log(LogCategory.Player, $"You browse the marketplace in {settlement.Definition.Name}.");
            AdvanceWorld(45);
            var item = ItemRegistry.AllItems[Rng.Next(ItemRegistry.AllItems.Count)];
            var price = Economy.MarketPrices[item.Id];
            Log(LogCategory.Player, $"A merchant offers {item.Name} for {price:F0} gold.");
            if (Player.Gold >= price)
            {
                Player.SpendGold(price);
                Player.AddItem(item.Id, 1);
                Log(LogCategory.Player, $"You purchase {item.Name}.");
                ApplyEventInfluence(Player.CurrentRegion, "MerchantCaravan", 4, "Marketplace trade");
                ApplyZoneInfluence(RegionId.DesertKingdom, "MerchantsRepublic", 4, "Marketplace trade");
                ApplyHiddenInfluence("KingsStability", 0.6, "Marketplace prosperity");

                if (item.Id == "DragonBone")
                    ApplyHiddenInfluence("DragonAwakeningHidden", 1.5, "Dragon bone traded");
                if (item.Id == "AncientRelic")
                    ApplyHiddenInfluence("AncientEvil", 1.0, "Ancient relic circulated");
            }
            else
            {
                Log(LogCategory.Player, "You do not have enough gold.");
            }
        }

        public void InventorySummary()
        {
            Log(LogCategory.Player, $"Gold: {Player.Gold:F0}");
            if (Player.Inventory.Count == 0)
            {
                Log(LogCategory.Player, "Inventory is empty.");
                return;
            }

            foreach (var kvp in Player.Inventory)
            {
                var item = ItemRegistry.GetItem(kvp.Key);
                Log(LogCategory.Player, $"{item.Name}: {kvp.Value}");
            }
        }

        public void CharacterSummary()
        {
            Log(LogCategory.Player, $"Health: {Player.Health:F0}/{Player.MaxHealth}");
            Log(LogCategory.Player, "Reputation:");
            foreach (var rep in Player.Reputation)
            {
                Log(LogCategory.Player, $"  {rep.Key}: {rep.Value:F1}");
            }
        }

        public void QuestLogSummary() => Log(LogCategory.Player, _autoAcceptedQuest == null ? "No active quests." : $"Active quest: {_autoAcceptedQuest.Title}");

        public void ReadBook()
        {
            Log(LogCategory.Player, "You study an ancient tome and gain wisdom.");
            if (Player.CurrentRegion == RegionId.NorthernRealm)
            {
                ApplyZoneInfluence(RegionId.NorthernRealm, "DivineOrder", 4, "Reading sacred chronicles");
                ApplyZoneInfluence(RegionId.NorthernRealm, "EternalWinter", -2, "Reading sacred chronicles");
                ApplyHiddenInfluence("DawnOfGods", 0.2, "Forgotten rune activated");
            }
        }

        public void Fish()
        {
            Log(LogCategory.Player, "You fish but catch nothing of value.");
            if (Player.CurrentRegion == RegionId.NorthernRealm)
            {
                ApplyZoneInfluence(RegionId.NorthernRealm, "EternalWinter", 2, "Icy fishing expedition");
                ApplyHiddenInfluence("TheVeil", 0.2, "Sacred spring disturbed");
            }
        }

        public void Camp()
        {
            Log(LogCategory.Player, "You camp under the stars and recover strength.");
            if (Player.CurrentRegion == RegionId.NorthernRealm)
            {
                ApplyZoneInfluence(RegionId.NorthernRealm, "UndeadPower", 2, "Night camp near old graves");
                ApplyHiddenInfluence("AncientEvil", 0.8, "Forbidden site camped near old tombs");
            }
        }

        public void Craft()
        {
            Log(LogCategory.Player, "You reinforce gear and craft protective charms.");
            if (Player.CurrentRegion == RegionId.NorthernRealm)
            {
                ApplyZoneInfluence(RegionId.NorthernRealm, "DivineOrder", 2, "Crafted warding charms");
                ApplyZoneInfluence(RegionId.NorthernRealm, "UndeadPower", -1, "Crafted warding charms");
                ApplyHiddenInfluence("DawnOfGods", 0.1, "Relic forging ritual");
            }
        }

        public bool TurnInQuest() => false;
        public bool AcceptQuest() => false;

        private void CompleteQuestsInTown()
        {
            if (_autoAcceptedQuest == null)
                return;

            var quest = _autoAcceptedQuest;
            _autoAcceptedQuest = null;
            Log(LogCategory.Player, $"[QUEST] Completed: {quest.Title}", ConsoleColor.Magenta);
            _completedQuestIds.Add(quest.Id);
            var relatedQuestlines = QuestLineRegistry.AllQuestLines.Where(ql => ql.QuestIds.Contains(quest.Id)).ToList();
            var collectorProgressBefore = GetCollectorProgress();

            if (quest.Reward.Gold.HasValue)
            {
                var beforeGold = Player.Gold;
                Player.Gold += quest.Reward.Gold.Value;
                if (IsFastMode)
                    Log(LogCategory.Player, $"[PLAYER EFFECT] Gold: {beforeGold:F0} -> {Player.Gold:F0} (+{quest.Reward.Gold.Value:F0})", ConsoleColor.Magenta);
            }

            foreach (var repChange in quest.Reward.ReputationChanges)
            {
                Player.ModifyReputation(repChange.Key, repChange.Value);
            }

            foreach (var itemStack in quest.Reward.Items)
            {
                if (itemStack?.Definition == null || itemStack.Quantity <= 0)
                    continue;

                Player.AddItem(itemStack.Definition.Id, itemStack.Quantity);
                LogFastFeature(LogCategory.Player, $"[REWARD] {itemStack.Definition.Name} x{itemStack.Quantity}");
            }

            foreach (var effect in quest.EventBarEffects)
            {
                var targetRegion = effect.TargetRegion ?? Player.CurrentRegion;
                EventBarState eventBar = null;
                RegionState eventRegion = null;
                var hadBar = false;
                if (Regions.TryGetValue(targetRegion, out eventRegion))
                {
                    hadBar = eventRegion.EventBars.TryGetValue(effect.BarId, out eventBar);
                }
                var before = hadBar ? eventBar.Value : 0;
                var changed = ApplyEventInfluence(targetRegion, effect.BarId, effect.Amount, $"Quest: {quest.Title}");

                if (changed && relatedQuestlines.Count > 0 && hadBar)
                {
                    var after = eventBar.Value;
                    var delta = after - before;
                    var regionName = eventRegion.Definition.Name;
                    foreach (var questline in relatedQuestlines)
                    {
                        LogQuestlineEvent(
                            $"Influence [{questline.Title}] Event Bar [{regionName}] {eventBar.Definition.Name}: {before:F1} -> {after:F1} ({delta:+0.0;-0.0;0.0}, {after:F1} / {eventBar.Definition.Threshold:F0})");
                    }
                }
            }

            foreach (var (zoneBarId, amount) in quest.ZoneEvolutionEffects)
            {
                var targetRegion = Player.CurrentRegion;
                if (!Regions[targetRegion].ZoneEvolutionBars.ContainsKey(zoneBarId))
                {
                    if (Regions[RegionId.NorthernRealm].ZoneEvolutionBars.ContainsKey(zoneBarId))
                        targetRegion = RegionId.NorthernRealm;
                    else if (Regions[RegionId.DesertKingdom].ZoneEvolutionBars.ContainsKey(zoneBarId))
                        targetRegion = RegionId.DesertKingdom;
                }

                ZoneEvolutionBarState zoneBar = null;
                RegionState zoneRegion = null;
                var hadZoneBar = false;
                if (Regions.TryGetValue(targetRegion, out zoneRegion))
                {
                    hadZoneBar = zoneRegion.ZoneEvolutionBars.TryGetValue(zoneBarId, out zoneBar);
                }
                var before = hadZoneBar ? zoneBar.Value : 0;
                var changed = ApplyZoneInfluence(targetRegion, zoneBarId, amount, $"Quest: {quest.Title}");

                if (changed && relatedQuestlines.Count > 0 && hadZoneBar)
                {
                    var after = zoneBar.Value;
                    var delta = after - before;
                    var regionName = zoneRegion.Definition.Name;
                    foreach (var questline in relatedQuestlines)
                    {
                        LogQuestlineEvent(
                            $"Influence [{questline.Title}] Zone Bar [{regionName}] {zoneBar.Definition.Name}: {before:F1} -> {after:F1} ({delta:+0.0;-0.0;0.0}, {after:F1} / {zoneBar.Definition.MaxValue:F0})");
                    }
                }
            }

            foreach (var (hiddenBarId, amount) in quest.HiddenBarEffects)
            {
                if (hiddenBarId == "DawnOfGods" && !DawnMythicQuestIds.Contains(quest.Id))
                {
                    LogQuestlineEvent($"Dawn gated: {quest.Title} is not mythic enough to progress Dawn of the Gods.");
                    continue;
                }

                ApplyHiddenInfluence(hiddenBarId, amount, $"Quest: {quest.Title}");
            }

            TryAdvanceQuestLineProgress(quest);
            LogCollectorProgressChange(collectorProgressBefore, $"after quest {quest.Title}");
        }

        public void PerformAutomaticAction(bool questMode)
        {
            var region = Regions[Player.CurrentRegion];
            var discoveredSettlements = region.Settlements.Values.Where(s => s.Discovered).ToList();
            var undiscoveredSettlements = region.Settlements.Values.Where(s => !s.Discovered).ToList();
            
            // Intelligent action selection based on game state - STAY IN REGION
            if (Player.Health < Player.MaxHealth * 0.4)
            {
                // Low health: prioritize resting
                Rest();
            }
            else if (undiscoveredSettlements.Count > 0 && Rng.Probability(0.4))
            {
                // Explore to find settlements (lower weight than before)
                Explore();
            }
            else if (discoveredSettlements.Count > 0 && Rng.Probability(0.3))
            {
                // Visit discovered towns occasionally
                VisitTown();
            }
            else if (discoveredSettlements.Count > 0 && Rng.Probability(0.15))
            {
                VisitMarketplace();
            }
            else if (Rng.Probability(0.10))
            {
                ReadBook();
            }
            else if (Rng.Probability(0.08))
            {
                Camp();
            }
            else if (Rng.Probability(0.06))
            {
                Craft();
            }
            else
            {
                // Default: explore to progress EventBars and trigger encounters
                Explore();
            }
        }

        public override string ToString()
        {
            var region = Regions[Player.CurrentRegion];
            var sb = new System.Text.StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"Time: {Clock.CurrentTime}");
            sb.AppendLine($"Player Region: {region.Definition.Name}");
            sb.AppendLine($"Health: {Player.Health:F0}/{Player.MaxHealth}, Gold: {Player.Gold:F0}");
            sb.AppendLine("Regions:");
            foreach (var r in Regions.Values)
            {
                sb.AppendLine($"  {r.Definition.Name}: {r.CurrentPath} (Tier {(int)r.CurrentTier})");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }

    public class WorldCampaign
    {
        private readonly WorldState _world;
        public int ActionCount { get; private set; }
        public WorldState World => _world;
        public Dictionary<RegionId, RegionState> Regions => _world.Regions;
        public List<string> EventLog => _world.SimulationHistory;

        public WorldCampaign(WorldState world)
        {
            _world = world;
            ActionCount = 0;
        }

        public void ExecuteAction(PlayerAction action)
        {
            ActionCount++;
        }
    }

    public class NoveltyTracker
    {
        public List<WorldState> Campaigns { get; private set; } = new();
        public Dictionary<string, int> PathOccurrences { get; private set; } = new();

        public void AddCampaign(WorldState world)
        {
            Campaigns.Add(world);
        }

        public double CalculateNovelty(WorldState world = null)
        {
            return Campaigns.Count > 0 ? 1.0 / Campaigns.Count : 1.0;
        }

        public string GenerateReport()
        {
            return $"Tracked {Campaigns.Count} campaigns";
        }
    }
}
