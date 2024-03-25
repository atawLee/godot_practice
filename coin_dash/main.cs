using Godot;
using System;

public partial class main : Node
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	public PackedScene coin_scene;

	[Export] public int playtime = 30;

	private int level = 1;
	private int score = 0;
	private int time_left = 0;
	private Vector2 screenSize = Vector2.Zero;
	private bool playing = false;
	private player player;

	public override void _Ready()
	{
		screenSize = GetViewport().GetVisibleRect().Size;
		this.player = GetNode<player>("Player");
		player.screenSize = screenSize;
		player.Hide();
		NewGame();
	}

	public void NewGame()
	{
		playing = true;
		level = 1;
		score = 0;
		time_left = playtime;
		player.Start();
		player.Show();
		var timer = GetNode<Timer>("GameTimer");
		timer.Start();
		SpawnCoins();
	}

	private void SpawnCoins()
	{
		for (int i = 0; i < level + 4;i++)
		{
			var c = coin_scene.Instantiate() as coin;
			AddChild(c);
			c.scrrenSize = screenSize;
			var rand = new Random(DateTime.Now.Millisecond);
			c.Position = new Vector2(rand.Next(0, (int)screenSize.X), rand.Next(0, (int)screenSize.Y));


		}
	}

	public override void _Process(double delta)
	{
		if (playing && GetTree().GetNodesInGroup("coins").Count == 0)
		{
			
		}
	}
}
