using Godot;
using System;
using System.Threading.Tasks;
using space_rock.Player;

public partial class Enemy : Area2D
{
	[Export]
	public PackedScene BulletScene { get; set; }

	[Export] public float Speed { get; set; } = 150;
	[Export] public float RotationSpeed { get; set; } = 120;

	[Export] public int Health = 3;
	[Export] public float BulletSpread { get; set; } = 0.2f;

	private PathFollow2D _follow = new();
	public Player Target { get; set; } = null;
	
	
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
		Rotation += Mathf.DegToRad(RotationSpeed) * (float)delta;
		_follow.Progress += Speed * (float)delta;
		Position = _follow.GlobalPosition;
		if (this._follow.ProgressRatio >= 1)
		{
			QueueFree();
		}
	}

	private void _on_gun_colldown_timeout()
	{
		Shoot();
	}

	private void Shoot()
	{
		var dir = GlobalPosition.DirectionTo(Target.GlobalPosition);
		dir = dir.Rotated((float)GD.RandRange(-BulletSpread, BulletSpread));
		var b = BulletScene.Instantiate() as Enemy_Bullet;
		GetTree().Root.AddChild(b);
		b!.Start(GlobalPosition, dir);
	}

	/// <summary>
	/// 연발 사용시 호출 
	/// </summary>
	/// <param name="n"></param>
	/// <param name="delay"></param>
	public async Task ShootPulse(int n, int delay)
	{
		for (int i = 0; i < n; i++)
		{
			Shoot();
			await Task.Delay(delay);
		}
	}

	public void TakeDamage(int amount)
	{
		this.Health -= amount;
		GetNode<AnimationPlayer>("AnimationPlayer").Play("flash");
		if (Health <= 0)
		{
			Explode();
		}
	}

	public async void Explode()
	{
		this.Speed = 0;
		GetNode<Timer>("GunColldown").Stop();
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled",true);
		GetNode<Sprite2D>("Sprite2D").Hide();
		GetNode<Sprite2D>("Explosion").Show();
		
		GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("explosion");
		await ToSignal(GetNode<AnimationPlayer>("Explosion/AnimationPlayer"), new StringName("animation_finished"));
		QueueFree();

	}
	
	private void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("rocks")) return;
		Explode();
	}

	
	
}

