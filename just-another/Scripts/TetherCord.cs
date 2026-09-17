using Godot;
using System;

public partial class TetherCord : Line2D
{
	// Assign these paths in the Godot Inspector
	[Export] public NodePath PlayerPath;
	[Export] public NodePath PlugPath;

	private Node2D _player;
	private Node2D _plug;

	public override void _Ready()
	{
		// Fetch the nodes based on the assigned inspector paths
		if (PlayerPath != null) _player = GetNode<Node2D>(PlayerPath);
		if (PlugPath != null) _plug = GetNode<Node2D>(PlugPath);

		// Ensure the Line2D has exactly 2 points
		ClearPoints();
		AddPoint(Vector2.Zero);
		AddPoint(Vector2.Zero);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player != null && _plug != null)
		{
			// Update line positions relative to the Line2D's global origin
			SetPointPosition(0, ToLocal(_player.GlobalPosition));
			SetPointPosition(1, ToLocal(_plug.GlobalPosition));
		}
	}
}
