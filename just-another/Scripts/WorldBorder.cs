using Godot;

// Naming it LevelTransition fixes the class conflict crash permanently!
namespace JustAnother.Scripts;

public partial class LevelTransition : Area2D
{
    private string _nextLevelPath = "res://level_2.tscn";
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
            if (!string.IsNullOrEmpty(_nextLevelPath))
            {
                Error loadStatus = GetTree().ChangeSceneToFile(_nextLevelPath);
                
                if (loadStatus != Error.Ok)
                {
                    GD.PrintErr($"[Error] Could not switch scenes to: {_nextLevelPath}. Reason: {loadStatus}");
                }
            }
            else
            {
                GD.PrintErr("[Error] Transition failed: NextLevelPath variable is completely empty!");
            }
        }
    }
}