using Godot;
using System;

public partial class coin : Area2D
{
	public Vector2 scrrenSize = Vector2.Zero;
	public void Pickup()
	{
		QueueFree();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
