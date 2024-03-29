using Godot;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class coin : Area2D
{
	public Vector2 scrrenSize = Vector2.Zero;
	public async Task Pickup()
	{
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
		var tw = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Quad);
		tw.TweenProperty(this, "scale", Scale * 3, 0.3);
		tw.TweenProperty(this, "modulate:a", 0.0, 0.3);
		await ToSignal(tw, "finished");
		QueueFree();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Random rand = new Random((int)DateTime.Now.Ticks);
		GetNode<Timer>("Timer").Start(rand.Next(3,8));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_timer_timeout()
	{
		var ani = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		ani.Frame = 0;
		ani.Play();
	}
}
