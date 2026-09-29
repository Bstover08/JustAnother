using Godot;

namespace JustAnother.Scripts;

public partial class Enemy : CharacterBody2D
{
    [Export] public float Speed = 60.0f;
    [Export] public float Direction = -1.0f; // -1.0f is left, 1.0f is right
    [Export] public float Gravity = 980.0f;
    [Export] public float KillDistance = 32.0f;

    private Node2D _playerNode;
    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        // Adjust the string to match the exact name of your AnimatedSprite2D node
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Apply gravity if airborne
        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }

        // 1. Check for wall collision first
        if (IsOnWall())
        {
            Direction *= -1.0f; // Invert movement direction immediately
            
            if (_animatedSprite != null)
            {
                // Flip the sprite texture to face the new direction
                _animatedSprite.FlipH = Direction < 0;
            }
        }

        // 2. Apply updated direction to horizontal movement
        velocity.X = Direction * Speed;
        Velocity = velocity;
        
        // 3. Move the character using physics engine
        MoveAndSlide();

        // Target acquisition (Player tracking)
        if (_playerNode == null)
        {
            _playerNode = GetTree().GetFirstNodeInGroup("player_target") as Node2D;
        }

        // Proximity kill check
        if (_playerNode != null)
        {
            float distance = GlobalPosition.DistanceTo(_playerNode.GlobalPosition);
            if (distance <= KillDistance)
            {
                GD.Print($"CRITICAL HIT! Distance: {distance}px. Reloading scene...");
                GetTree().ReloadCurrentScene();
            }
        }
    }
}
