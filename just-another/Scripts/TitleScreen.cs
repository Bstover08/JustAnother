using Godot;
using System;

public partial class TitleScreen : Control
{
    private Button _button1;
    private Button _button2;
    private Button _button3;

    public override void _Ready()
    {
        _button1 = GetNode<Button>("VBoxContainer/Button");
        _button2 = GetNode<Button>("VBoxContainer/Button2");
        _button3 = GetNode<Button>("VBoxContainer/Button3");
        
        _button1.Pressed += OnButton1Pressed;
        _button2.Pressed += OnButton2Pressed;
        _button3.Pressed += OnButton3Pressed;
    }

    // Defines what happens when each button is clicked
    private void OnButton1Pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/game.tscn");
    }

    private void OnButton2Pressed()
    {
        GetTree().Quit();
    }

    private void OnButton3Pressed()
    {
        GetTree().Quit();
    }
}