using Godot;

namespace JustAnother.Scripts
{
	public partial class Six42 : CharacterBody2D
	{
		// Movement configuration fields
		[Export] public float Speed = 300.0f;
		[Export] public float JumpVelocity = -400.0f;

		// Private reference to your AnimatedSprite2D node for Encapsulation
		private AnimatedSprite2D _animatedSprite;

		public override void _Ready()
		{
			// Fetch the AnimatedSprite2D child node safely when the game starts
			if (HasNode("AnimatedSprite2D"))
			{
				_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector2 velocity = Velocity;

			// 1. Apply environmental gravity force if the robot is airborne
			if (!IsOnFloor())
			{
				// Project settings handle default gravity calculation automatically
				velocity += GetGravity() * (float)delta;
			}

		 
			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			}

			// 3. Fetch horizontal movement vector (Handles Arrow Keys & Joysticks)
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

			// 4. Safely process state animation switching
			UpdateAnimations(direction);
		}

		private void UpdateAnimations(Vector2 direction)
		{
			if (_animatedSprite == null) return;

			// Priority 1: Check if the player is airborne to play the Jump frame
			if (!IsOnFloor())
			{
				_animatedSprite.Play("Jump");
				
				// Keep flipping texture tracking while jumping through the air
				if (direction.X != 0)
				{
					_animatedSprite.FlipH = direction.X < 0;
				}
			}
			// Priority 2: Ground movement tracking
			else if (direction.X != 0)
			{
				_animatedSprite.Play("Walk");
				_animatedSprite.FlipH = direction.X < 0;
			}
			// Priority 3: Default stand still state
			else
			{
				_animatedSprite.Play("Idle");
			}
		}
	}
}
