using Godot;

namespace JustAnother.Scripts;

public partial class LevelTransition : Area2D
{
    // The target level path—completely direct and self-contained
    private string _nextLevelPath = "res://secondlevel.tscn";

    public override void _Ready()
    {
        // Connect Godot's physics collision signal directly to our function
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        // Check if the object that touched the zone is the player character
        if (body is CharacterBody2D player)
        {
            GD.Print($"[Portal] Player '{player.Name}' hit the zone. Snapping directly to: {_nextLevelPath}");

            // Tell Godot to immediately unload the current level and load the next one
            Error loadStatus = GetTree().ChangeSceneToFile(_nextLevelPath);
            
            if (loadStatus != Error.Ok)
            {
                GD.PrintErr($"[Error] Could not switch scenes to: {_nextLevelPath}. Reason: {loadStatus}");
            }
        }
    }
}