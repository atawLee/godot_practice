using Godot;
using System;
using System.Web;
using space_rock.Player;

public partial class Main : Node
{
	
	
	[Export]
	public PackedScene RockScene { get; set; }

	public Vector2 ScreenSize { get; set; } = Vector2.Zero;

	private int _level = 0;
	private int _score = 0;
	private bool isPlaying = false;
	
	
	public override void _Ready()
	{
		
		this.ScreenSize = GetViewport().GetVisibleRect().Size;
		for (int i = 0; i < 3; i++)
		{
			SpawnRock(3);
		}
		
		
	}

	private void OnPlaying()
	{
		isPlaying = true;
		var timer = GetNode<Timer>("HUD/Timer");
		timer.Timeout -= OnPlaying; 
	}

	private void SpawnRock(float size , Vector2? position = null, Vector2? velocity =null)
	{
		if (position == null)
		{
			var spawn = GetNode<PathFollow2D>("RockPath/RockSpawn");
			spawn.Progress = GD.Randi();
			position = spawn.Position;
		}

		if (velocity == null)
		{
			velocity = Vector2.Right.Rotated((float)GD.RandRange(0,Math.Tau) *
											 (float)125);
		}

		var r = RockScene.Instantiate() as Rock;
		r.Exploded += OnRockExploded;
		r.ScreenSize = this.ScreenSize;
		r.Start((Vector2)position,(Vector2)velocity,size);
		CallDeferred(new StringName("add_child"), r);

	}

	private void OnRockExploded(float size, float radius, Vector2 position, Vector2 linearvelocity)
	{
		if (size <= 1)
		{
			return;
		}

		int[] offsets = { -1, 1 };

		foreach(int offset in offsets)
		{
			var dir = GetNode<RigidBody2D>("Player").Position.DirectionTo(position).Orthogonal() * offset;
			var newpos = position + dir * radius;
			var newvel = dir * linearvelocity.Length() * 1.1f;
			SpawnRock(size - 1f, newpos, newvel);
		}

		_score += (int)(10 * size);
	}

	public void NewLevel()
	{
		_level += 1;
		GetNode<HUD>("HUD").ShowMessage($"Wave {_level}");
		for (int i = 0; i < _level; i++)
		{
			SpawnRock(3);
		}
	}

	public void NewGame()
	{
		GetTree().CallGroup("rocks","quque_free");
		_level = 0;
		_score = 0;
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		
		var timer = GetNode<Timer>("HUD/Timer");
		timer.Timeout += OnPlaying;
		hud.ShowMessage("Get Ready");
		GetNode<Player>("Player").Reset();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!isPlaying)
		{
			return;
		}

		if (GetTree().GetNodesInGroup("rocks").Count == 0)
		{
			NewLevel();
		}
	}

	public void GameOver()
	{
		isPlaying = false;
		GetNode<HUD>("HUD").GameOver();
	}
}


