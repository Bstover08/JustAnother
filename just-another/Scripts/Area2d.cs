using Godot;
using System;

// Explicitly using 'Godot.Area2D' fixes conflict name errors natively
public partial class Area2D : Godot.Area2D
{
    // The path to your target level file (editable in the Godot Inspector)
    [Export(PropertyHint.File, "*.tscn")] 
    public string NextLevelPath = "res://level_2.tscn";

    // Where to visually position this transition zone on your 2D grid map
    [Export] public Vector2 TriggerPosition = new Vector2(1200.0f, 300.0f);

    // The size dimensions of your zone (Width: 64px, Height: 128px)
    [Export] public Vector2 TriggerSize = new Vector2(64.0f, 128.0f);

    public override void _Ready()
    {
        // 1. Move this node to its designated level position
        GlobalPosition = TriggerPosition;

        // 2. Programmatically instantiate the mandatory CollisionShape2D child
        CollisionShape2D collisionShape = new CollisionShape2D();
        RectangleShape2D rectangleShape = new RectangleShape2D();
        
        // Define the bounds and assign it to the shape holder
        rectangleShape.Size = TriggerSize;
        collisionShape.Shape = rectangleShape;
        
        // Inject the freshly made collision box as a child node in memory
        AddChild(collisionShape);

        // 3. Bind the body entry signal handler natively
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        // Use the C# 'is' keyword to identify your player and cast it safely
        if (body is CharacterBody2D player)
        {
            GD.Print($"[Transition Engine] Player '{player.Name}' hit the portal zone.");

            // Confirm our target path variable isn't empty before execution
            if (!string.IsNullOrEmpty(NextLevelPath))
            {
                Error loadStatus = GetTree().ChangeSceneToFile(NextLevelPath);
                
                if (loadStatus != Error.Ok)
                {
                    GD.PrintErr($"[Error] Could not switch scenes to: {NextLevelPath}. Reason: {loadStatus}");
                }
            }
            else
            {
                GD.PrintErr("[Error] Transition failed: NextLevelPath variable is completely empty!");
            }
        }
    }
}
