using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene RockScene { get; set; }

	public Vector2 ScreenSize { get; set; } = Vector2.Zero; 
	
	
	public override void _Ready()
	{
		this.ScreenSize = GetViewport().GetVisibleRect().Size;
		for (int i = 0; i < 3; i++)
		{
			SpawnRock(3);
		}
	}

	private void SpawnRock(int size , Vector2? position = null, Vector2? velocity =null)
	{
		if (position == null)
		{
			var spawn = GetNode<PathFollow2D>("RockPath/RockSpawn");
			spawn.Progress = (float)RandomUtils.RandfRange(0, int.MaxValue);
			position = spawn.Position;
		}

		if (velocity == null)
		{
			velocity = Vector2.Right.Rotated((float)RandomUtils.RandfRange(0, (float)Math.Tau) *
											 (float)RandomUtils.RandfRange(50, 125));
		}

		var r = RockScene.Instantiate() as Rock;
		r.ScreenSize = this.ScreenSize;
		r.Start((Vector2)position,(Vector2)velocity,size);
		CallDeferred("add_child", r);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
