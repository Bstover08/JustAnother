using Godot;
using System;

public partial class PlugBlock : RigidBody2D
{
	[Export] public NodePath PlayerPath;
	[Export] public float MaxCordLength = 200.0f;
	[Export] public float PullTension = 15.0f;

	private Node2D _player;

	public override void _Ready()
	{
		if (PlayerPath != null) _player = GetNode<Node2D>(PlayerPath);
		
		// Lock rotation so it doesn't roll like a wheel
		LockRotation = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player == null) return;

		Vector2 toPlayer = _player.GlobalPosition - GlobalPosition;
		float distance = toPlayer.Length();

		if (distance > MaxCordLength)
		{
			Vector2 direction = toPlayer.Normalized();
			float overshoot = distance - MaxCordLength;

			// Instead of compounding forces, directly pull the velocity toward the player
			// This gives it a true heavy drag feel without rubber-banding into the sky
			Vector2 targetVelocity = direction * (overshoot * PullTension);
			LinearVelocity = LinearVelocity.MoveToward(targetVelocity, 500.0f * (float)delta);
		}
	}
}
