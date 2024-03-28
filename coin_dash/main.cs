using Godot;
using System;
using System.Threading.Tasks;

public partial class main : Node
{
	// Called when the node enters the scene tree for the first time.

	[Export] public PackedScene coin_scene;

	[Export] public int playtime = 30;

	[Export] public PackedScene powerup_scene;

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
		var tempHud = this.GetNode<hud>("HUD");
		tempHud.UpdateScore(score);
		tempHud.UpdateTime(time_left);
		SpawnCoins();
	}

	private void SpawnCoins()
	{
		var rand = new Random(DateTime.Now.Millisecond);
		for (int i = 0; i < level + 4; i++)
		{
			var c = coin_scene.Instantiate() as coin;
			AddChild(c);
			c.scrrenSize = screenSize;
			c.Position = new Vector2(rand.Next(0, (int)screenSize.X), rand.Next(0, (int)screenSize.Y));
		}

		GetNode<AudioStreamPlayer>("LevelSound").Play();
	}

	public override void _Process(double delta)
	{
		if (playing && GetTree().GetNodesInGroup("coins").Count == 0)
		{
			level += 1;
			time_left += 5;
			SpawnCoins();
			var powerTimer = GetNode<Timer>("PowerTimer");
			powerTimer.WaitTime = new Random(DateTime.Now.Millisecond).Next(5, 10);
			powerTimer.Start();
		}
	}

	private async void _on_game_timer_timeout()
	{
		time_left -= 1;
		GetNode<hud>("HUD").UpdateTime(time_left);
		if (time_left <= 0)
			await GameOver();
	}

	private async Task GameOver()
	{
		playing = false;
		GetNode<Timer>("GameTimer").Stop();

		GetTree().CallGroup("coins", "queue_free");
		await GetNode<hud>("HUD").ShowGameOver();
		GetNode<player>("Player").Die();

		GetNode<AudioStreamPlayer>("EndSound").Play();
	}

	private async void _on_player_hurt()
	{
		await GameOver();
	}

	private void _on_player_pickup(string type)
	{
		score += 1;
		GetNode<hud>("HUD").UpdateScore(score);
		GetNode<AudioStreamPlayer>("CoinSound").Play();
	}

	private void _on_hud_start_game()
	{
		NewGame();
	}

	private void _on_power_timer_timeout()
	{
		var rand = new Random(DateTime.Now.Millisecond);
		//todo : error 발생 위치 
		var p = powerup_scene.Instantiate() as powerup;
		AddChild(p);
		p.scrrenSize = screenSize;
		p.Position = new Vector2(rand.Next(0, (int)screenSize.X), rand.Next(0, (int)screenSize.Y));
	}
}






