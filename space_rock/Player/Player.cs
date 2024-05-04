using Godot;
using System;
using System.Diagnostics;

namespace space_rock.Player;  
public partial class Player : RigidBody2D
{
	[Signal]
	public delegate void ShieldChangedEventHandler(float value);

	[Export] public float MaxShield { get; set; } = 100f;
	[Export] public float ShieldRegen { get; set; } = 5.0f;

	private float _shield;

	public float Shield
	{
		get => _shield;
		set
		{
			var temp = Math.Min(value, this.MaxShield);
			_shield = temp;
			EmitSignal(SignalName.ShieldChanged, this.MaxShield);

			if (_shield <= 0)
			{
				_lives -= 1;
				Explode();
			}
		}
	}
	[Export] public int EnginePower { get; set; } = 500;
	[Export] public int SpinPower { get; set; } = 500;

	public PlayerState CurrentState = PlayerState.Init;

	[Export] public PackedScene BulletPackedScene { get; set; }

	[Export] public float FireRate = 0.25f;

	private bool _canShoot = true;
	public Vector2 ScreenSize { get; set; }

	private Vector2 _thrust = Vector2.Zero;
	private float _rotationDir = 0;
	private CollisionShape2D PlayerShape2d => GetNode<CollisionShape2D>("CollisionShape2D");

	[Signal]
	public delegate void LivesChangedEventHandler(int value);

	[Signal]
	public delegate void DeadEventHandler();

	private bool _resetPosition = false;

	private int _lives;
	public int Lives
	{
		get => _lives;
		set
		{
			_lives = value;
			EmitSignal(SignalName.LivesChanged, _lives);

			if (_lives < 0)
			{
				ChangeState(PlayerState.Dead);
			}
			else
			{
				ChangeState(PlayerState.Invalulnerable);
			}
		}
	}

	public void Reset()
	{
		_resetPosition = true;
		GetNode<Sprite2D>("Sprite2D").Show();
		Lives = 3;
		ChangeState(PlayerState.Alive);
	}
	public override void _Ready()
	{
		ChangeState(PlayerState.Alive);
		this.ScreenSize = GetViewportRect().Size;
		GetNode<Timer>("GunCoolDown").WaitTime = FireRate;
	}

	public override void _Process(double delta)
	{
		GetInput();
	}

	private void ChangeState(PlayerState state)
	{
		var player = this.PlayerShape2d;
		
		var sprite = GetNode<Sprite2D>("Sprite2D");
		var modulate = sprite.Modulate;
		switch (state)
		{
			case PlayerState.Init:
				player.SetDeferred("disabled", true);
				modulate.A = 0.5f;
				break;
			case PlayerState.Alive:
				player.SetDeferred("disabled", false);
				modulate.A = 1.0f;
				break;
			case PlayerState.Invalulnerable:
				player.SetDeferred("disabled", true);
				modulate.A = 0.5f;
				GetNode<Timer>("InvaulnerablilityTimer").Start();
				break;
			case PlayerState.Dead:
				player.SetDeferred("disabled", true);
				sprite.Hide();
				LinearVelocity = Vector2.Zero;
				EmitSignal(SignalName.Dead);
				break;
		}
		this.CurrentState = state;
	}

	private void GetInput()
	{
		this._thrust = Vector2.Zero;
		if (CurrentState is PlayerState.Dead or PlayerState.Init)
			return;

		if (Input.IsActionPressed("thrust"))
		{
			_thrust = Transform.X * EnginePower;
		}

		if (Input.IsActionPressed("shoot") && _canShoot)
			Shoot();

		_rotationDir = Input.GetAxis(new StringName("rotation_left"), new StringName("rotation_right"));
	}

	private void Shoot()
	{
		if (this.CurrentState == PlayerState.Invalulnerable)
		{
			return;
		}

		_canShoot = false;
		GetNode<Timer>("GunCoolDown").Start();
		var b = BulletPackedScene.Instantiate() as Bullet;
		Debug.Assert(b != null,"Bullet NotFound");
		GetTree().Root.AddChild(b);
		b.Start(GetNode<Marker2D>("Muzzle").GlobalTransform);
	}

	public override void _PhysicsProcess(double delta)
	{
		//base._PhysicsProcess(delta);
		ConstantForce = _thrust;
		ConstantTorque = _rotationDir * SpinPower;
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		//base._IntegrateForces(state);
		var xForm = state.Transform;
		xForm.Origin.X = Wrapf(xForm.Origin.X, 0, ScreenSize.X);
		xForm.Origin.Y = Wrapf(xForm.Origin.Y, 0, ScreenSize.Y);
		state.Transform = xForm;

		if (_resetPosition)
		{
			xForm.Origin = ScreenSize / 2; 
			_resetPosition = false;
		}
	}
	
	public float Wrapf(float value, float min, float max)
	{
		float range = max - min;
		while (value < min)
		{
			value += range;
		}
		while (value >= max)
		{
			value -= range;
		}
		return value;
	}
	
	private void _on_gun_cool_down_timeout()
	{
		_canShoot = true;
	}
	
	private void _on_invaulnerablility_timer_timeout()
	{
		ChangeState(PlayerState.Alive);
	}
	
	private void _on_body_entered(Node body)
	{
		if (body.IsInGroup("rocks"))
		{
			var rock = body as Rock;
			rock.Explode();
			_lives = -1;
			Explode();
		}
	}

	private async void Explode()
	{
		var explosion = GetNode<Sprite2D>("Explosion");
		explosion.Show();
		var ani = GetNode<AnimationPlayer>("Explosion/AnimationPlayer");
		//ani.AnimationFinished += AniOnAnimationFinished;
		ani.Play("explosion");
		await ToSignal(ani, "animation_finished");
		explosion.Hide();
	}
}









