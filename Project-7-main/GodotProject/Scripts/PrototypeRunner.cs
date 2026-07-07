using Godot;
using PG.Systems;
using System.Collections.Generic;
using System.Linq;

public partial class PrototypeRunner : Node2D
{
    private const float PlayerSpeed = 220f;
    private const float AttackRange = 110f;
    private const float AttackCooldownDuration = 0.35f;

    private PrototypeGameManager _manager = null!;
    private CharacterBody2D _player = null!;
    private Node2D _world = null!;
    private Label _stateLabel = null!;
    private Label _questLabel = null!;
    private Label _lootLabel = null!;
    private Label _hintLabel = null!;
    private Dictionary<string, Node2D> _enemyNodes = new();

    private float _attackCooldown = 0f;
    private float _worldTime = 0f;

    public override void _Ready()
    {
        _player = GetNode<CharacterBody2D>("Player");
        _world = GetNode<Node2D>("World");

        BuildHud();
        _manager = new PrototypeGameManager(12345);
        _manager.StartSession();

        var camera = _player.GetNode<Camera2D>("Camera2D");
        camera.MakeCurrent();

        SyncWorldToManager();
        UpdateHud();
    }

    public override void _Process(double delta)
    {
        var dt = (float)delta;
        _worldTime += dt;

        if (_attackCooldown > 0f)
        {
            _attackCooldown -= dt;
        }

        HandlePlayerMovement(dt);

        if ((Input.IsActionJustPressed("ui_accept") || Input.IsKeyPressed(Key.Space) || Input.IsMouseButtonPressed(MouseButton.Left)) && _attackCooldown <= 0f)
        {
            AttackNearestEnemy();
            _attackCooldown = AttackCooldownDuration;
        }

        _manager.Tick(dt);
        SyncWorldToManager();
        UpdateHud();
    }

    private void BuildHud()
    {
        var hud = new CanvasLayer();
        AddChild(hud);

        var panel = new PanelContainer
        {
            AnchorLeft = 0.02f,
            AnchorTop = 0.02f,
            AnchorRight = 0.98f,
            AnchorBottom = 0.98f,
            OffsetLeft = 0,
            OffsetTop = 0,
            OffsetRight = 0,
            OffsetBottom = 0
        };
        hud.AddChild(panel);

        var vbox = new VBoxContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        panel.AddChild(vbox);

        var title = new Label
        {
            Text = "ProceduralGame • Playable Prototype",
            HorizontalAlignment = HorizontalAlignment.Center,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(title);

        _stateLabel = new Label
        {
            Text = "Loading world...",
            AutowrapMode = TextServer.AutowrapMode.Word,
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(_stateLabel);

        _questLabel = new Label
        {
            Text = "Quest: waiting",
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(_questLabel);

        _lootLabel = new Label
        {
            Text = "Loot: none",
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(_lootLabel);

        _hintLabel = new Label
        {
            Text = "WASD to move • Space or click to attack • Survive the region",
            SizeFlagsHorizontal = SizeFlags.ExpandFill
        };
        vbox.AddChild(_hintLabel);
    }

    private void HandlePlayerMovement(float delta)
    {
        var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        var velocity = direction * PlayerSpeed;
        _player.Velocity = velocity;
        _player.MoveAndSlide();

        if (velocity.LengthSquared() > 0f)
        {
            _player.GlobalPosition = new Vector2(
                Mathf.Clamp(_player.GlobalPosition.X, -720f, 720f),
                Mathf.Clamp(_player.GlobalPosition.Y, -420f, 420f));
        }
    }

    private void AttackNearestEnemy()
    {
        var target = _enemyNodes.Values
            .OrderBy(node => (node.GlobalPosition - _player.GlobalPosition).LengthSquared())
            .FirstOrDefault();

        if (target == null)
        {
            return;
        }

        var enemyId = target.Name;
        var distance = (target.GlobalPosition - _player.GlobalPosition).Length();
        if (distance > AttackRange)
        {
            return;
        }

        _manager.DamageEnemy(enemyId, 60f);
        var pulse = new ColorRect
        {
            Size = new Vector2(70, 70),
            Position = new Vector2(-35, -35),
            Color = new Color(1f, 0.9f, 0.2f, 0.65f)
        };
        target.AddChild(pulse);
        var tween = CreateTween();
        tween.TweenProperty(pulse, "modulate:a", 0f, 0.2f);
        tween.TweenCallback(Callable.From(() => pulse.QueueFree()));
    }

    private void SyncWorldToManager()
    {
        var activeIds = new HashSet<string>(_manager.CurrentState.ActiveEnemies.Select(enemy => enemy.Id));
        var currentIds = _enemyNodes.Keys.ToList();

        foreach (var idleId in currentIds.Where(id => !activeIds.Contains(id)))
        {
            _enemyNodes[idleId].QueueFree();
            _enemyNodes.Remove(idleId);
        }

        for (var index = 0; index < _manager.CurrentState.ActiveEnemies.Count; index++)
        {
            var enemy = _manager.CurrentState.ActiveEnemies[index];
            if (_enemyNodes.TryGetValue(enemy.Id, out var enemyNode))
            {
                var orbitOffset = new Vector2(
                    Mathf.Cos(_worldTime + index) * 70f,
                    Mathf.Sin(_worldTime * 1.25f + index) * 40f);
                enemyNode.GlobalPosition = new Vector2(320f + index * 90f, 180f + (index % 3) * 80f) + orbitOffset;
                continue;
            }

            enemyNode = new Node2D { Name = enemy.Id };
            var body = new ColorRect
            {
                Size = new Vector2(42, 42),
                Position = new Vector2(-21, -21),
                Color = new Color(0.9f, 0.2f, 0.2f)
            };
            enemyNode.AddChild(body);

            var label = new Label
            {
                Text = enemy.Archetype.ArchetypeName,
                Position = new Vector2(-40, 24),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            enemyNode.AddChild(label);

            _world.AddChild(enemyNode);
            _enemyNodes[enemy.Id] = enemyNode;
        }
    }

    private void UpdateHud()
    {
        var region = _manager.CurrentState.CurrentRegion;
        var quest = _manager.CurrentState.ActiveQuests.FirstOrDefault();
        var lootText = _manager.CurrentState.ActiveLoot.Count > 0
            ? string.Join("\n", _manager.CurrentState.ActiveLoot.Take(4).Select(item => $"• {item.DisplayName} ({item.Rarity})"))
            : "none";

        _stateLabel.Text = $"Region: {region.RegionId}\nPath: {region.CurrentPath}\nTier: {region.CurrentTier}\nEnemies: {_manager.CurrentState.ActiveEnemies.Count}\nTime: {_manager.CurrentState.PlayTimeSeconds:F1}s";
        _questLabel.Text = quest == null
            ? "Quest: no active quest"
            : $"Quest: {quest.Title}\nObjective: {quest.Objective?.CurrentCount}/{quest.Objective?.TargetCount} ({quest.State})";
        _lootLabel.Text = $"Loot collected:\n{lootText}";
        _hintLabel.Text = _manager.CurrentState.ActiveEnemies.Count > 0
            ? "WASD to move • Space or click to attack • Survive the region"
            : "WASD to move • The region is clear — enemies will return soon";
    }
}
