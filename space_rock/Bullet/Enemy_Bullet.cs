using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy_Bullet : Area2D
{
	[Export] public float Speed { get; set; } = 1000f;

	public void Start(Vector2 position, Vector2 direction)
	{
		this.Position = position;
		this.Rotation = direction.Angle();
	}
	
	public override void _Ready()
	{
	}
	
	public override void _Process(double delta)
	{
		this.Position += Transform.X * this.Speed * (float)delta;
	}
	
	private void _on_body_entered(Node2D body)
	{
		QueueFree();
	}

	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		QueueFree();
	}
}
