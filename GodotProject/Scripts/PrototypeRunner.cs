using Godot;
using System.Text;
using PG.Systems;

public partial class PrototypeRunner : Control
{
    private Label _output = null!;
    private ProgressBar _enemyBar = null!;
    private Label _summaryLabel = null!;
    private PrototypeGameManager? _manager;

    public override void _Ready()
    {
        _output = GetNode<Label>("Output");
        BuildHud();
        RunPrototype();
    }

    public override void _Draw()
    {
        var rect = new Rect2(0, 0, Size.X, Size.Y);
        DrawRect(rect, new Color(0.08f, 0.12f, 0.18f));
        DrawRect(new Rect2(20, 20, Size.X - 40, 140), new Color(0.18f, 0.25f, 0.35f));
        DrawRect(new Rect2(40, 210, Size.X - 80, 170), new Color(0.14f, 0.20f, 0.28f));

        var count = _manager?.CurrentState.ActiveEnemies.Count ?? 3;
        for (var i = 0; i < Mathf.Min(count, 5); i++)
        {
            var x = 60 + (i * 70);
            DrawCircle(new Vector2(x, 280), 24, new Color(0.85f, 0.25f, 0.25f));
        }
    }

    private void BuildHud()
    {
        var bg = new ColorRect
        {
            Name = "Backdrop",
            AnchorsPreset = LayoutPreset.FullRect,
            Color = new Color(0.07f, 0.09f, 0.13f),
            ZIndex = -10
        };
        AddChild(bg);
        bg.MoveToFront();

        var panel = new PanelContainer
        {
            Name = "Panel",
            AnchorLeft = 0.03f,
            AnchorTop = 0.03f,
            AnchorRight = 0.97f,
            AnchorBottom = 0.97f,
            OffsetLeft = 0,
            OffsetTop = 0,
            OffsetRight = 0,
            OffsetBottom = 0
        };
        AddChild(panel);

        var vbox = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
        panel.AddChild(vbox);

        var title = new Label
        {
            Text = "ProceduralGame Prototype",
            HorizontalAlignment = HorizontalAlignment.Center,
            ThemeTypeVariation = "HeaderLarge",
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(title);

        _summaryLabel = new Label
        {
            Text = "Waiting for the prototype run...",
            AutowrapMode = TextServer.AutowrapMode.Word,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(_summaryLabel);

        _enemyBar = new ProgressBar
        {
            MinValue = 0,
            MaxValue = 5,
            Value = 0,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(_enemyBar);

        var infoLabel = new Label
        {
            Text = "Enemy pressure and loot feedback",
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(infoLabel);

        var scenePanel = new ColorRect
        {
            CustomMinimumSize = new Vector2(0, 220),
            Color = new Color(0.16f, 0.22f, 0.30f),
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(scenePanel);

        var sceneLabel = new Label
        {
            Text = "World view: region, path, loot, and quest state",
            AnchorsPreset = LayoutPreset.TopWide,
            OffsetLeft = 16,
            OffsetTop = 16,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        scenePanel.AddChild(sceneLabel);

        _output = new Label
        {
            Text = "Starting prototype...",
            AutowrapMode = TextServer.AutowrapMode.Word,
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            CustomMinimumSize = new Vector2(0, 180)
        };
        vbox.AddChild(_output);
    }

    private void RunPrototype()
    {
        _manager = new PrototypeGameManager(12345);
        _manager.StartSession();

        var sb = new StringBuilder();
        sb.AppendLine("=== Minimal Godot Prototype ===");
        sb.AppendLine($"Region: {_manager.CurrentState.CurrentRegion.RegionId}");
        sb.AppendLine($"Path: {_manager.CurrentState.CurrentRegion.CurrentPath}");
        sb.AppendLine($"Tier: {_manager.CurrentState.CurrentRegion.CurrentTier}");
        sb.AppendLine($"Enemies: {_manager.CurrentState.ActiveEnemies.Count}");
        sb.AppendLine($"Quests: {_manager.CurrentState.ActiveQuests.Count}");
        sb.AppendLine();

        while (_manager.CurrentState.ActiveEnemies.Count > 0)
        {
            var enemy = _manager.CurrentState.ActiveEnemies[0];
            sb.AppendLine($"Attacking enemy: {enemy.Archetype.ArchetypeName} (HP: {enemy.CurrentHealth})");
            _manager.DamageEnemy(enemy.Id, enemy.CurrentHealth + 1);
            sb.AppendLine("Enemy killed.");
            sb.AppendLine($"Loot generated: {_manager.CurrentState.ActiveLoot.Count}");
            sb.AppendLine();
        }

        _manager.Tick(0.016f);

        sb.AppendLine($"Play time: {_manager.CurrentState.PlayTimeSeconds:F3}s");
        sb.AppendLine($"Remaining enemies: {_manager.CurrentState.ActiveEnemies.Count}");

        foreach (var quest in _manager.CurrentState.ActiveQuests)
        {
            sb.AppendLine($"Quest: {quest.Title} - {quest.State} ({quest.Objective?.CurrentCount}/{quest.Objective?.TargetCount})");
        }

        if (_manager.CurrentState.ActiveLoot.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("Loot:");
            foreach (var item in _manager.CurrentState.ActiveLoot)
            {
                sb.AppendLine($" - {item.DisplayName} [{item.Rarity}]");
            }
        }

        _output.Text = sb.ToString();
        _summaryLabel.Text = $"Region {_manager.CurrentState.CurrentRegion.RegionId} • Path {_manager.CurrentState.CurrentRegion.CurrentPath} • Loot {_manager.CurrentState.ActiveLoot.Count}";
        _enemyBar.MaxValue = 5;
        _enemyBar.Value = _manager.CurrentState.ActiveEnemies.Count;
        QueueRedraw();
    }
}
