using Godot;
using System;

public partial class Enemy : Area2D
{
	[Export]
	public PackedScene BulletScene { get; set; }

	[Export] public float Speed { get; set; } = 150;

	[Export] public float RotationSpeed { get; set; } = 120;

	[Export] public int Health = 3;

	private PathFollow2D _follow = new();
	private object _target = null;
	
	
	public override void _Ready()
	{
		GetNode<Sprite2D>("Sprite2D").Frame = (int)GD.Randi() % 3;
		var enemyPaths = GetNode("EnemyPaths").GetChildren();
		var path = enemyPaths[(int)GD.Randi() % enemyPaths.Count];
		path.AddChild(this._follow);
		_follow.Loop = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		

	}

	public override void _PhysicsProcess(double delta)
	{
		
	}

	private void _on_gun_colldown_timeout()
	{
		// Replace with function body.
	}

}


