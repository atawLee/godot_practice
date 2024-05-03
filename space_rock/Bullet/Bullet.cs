using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public int Speed { get; set; } = 1000;

	public Vector2 Velocity = Vector2.Zero;

	public void Start(Transform2D transform)
	{
		this.Transform = transform;
		Velocity = Transform.X * Speed;
	}
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		this.Position += Velocity * (float)delta;
	}
	
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		QueueFree();
	}
	
	private void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("rocks"))
		{
			var rock = body as Rock;
			rock?.Explode();
			QueueFree();
		}
	}
	
	private void _on_area_entered(Area2D area)
	{
		if (area.IsInGroup("enemies"))
		{
			var enemy = area as Enemy;
			enemy!.TakeDamage(1);
		}
	}
}