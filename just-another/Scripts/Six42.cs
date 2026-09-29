using Godot;

namespace JustAnother.Scripts;

public partial class Six42 : CharacterBody2D
{
    [Export] public float Speed = 300.0f;
    [Export] public float JumpVelocity = -400.0f;

    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        // FORCE ADD: Adds this specific running instance into the universal system tag matrix
        // This bypasses whatever name you gave this node in your Level scene!
        AddToGroup("player_target");

        if (HasNode("AnimatedSprite2D"))
        {
            _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();

        UpdateAnimations(direction);
    }

    private void UpdateAnimations(Vector2 direction)
    {
        if (_animatedSprite == null) return;

        if (!IsOnFloor())
        {
            _animatedSprite.Play("Jump");
            if (direction.X != 0)
            {
                _animatedSprite.FlipH = direction.X < 0;
            }
        }
        else if (direction.X != 0)
        {
            _animatedSprite.Play("Walk");
            _animatedSprite.FlipH = direction.X < 0;
        }
        else
        {
            _animatedSprite.Play("Idle");
        }
    }
}