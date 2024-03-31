using Godot;
using System;

namespace space_rock.Player;  
public partial class Player : RigidBody2D
{
	[Export] public int EnginePower { get; set; } = 500;
	[Export] public int SpinPower { get; set; } = 500;
	
	public PlayerState CurrentState = PlayerState.Init;

	private Vector2 _thrust = Vector2.Zero;
	private float _rotationDir = 0;
	private CollisionShape2D PlayerShape2d => GetNode<CollisionShape2D>("CollisionShape2D"); 
	public override void _Ready()
	{
		ChangeState(PlayerState.Alive);
	}

	public override void _Process(double delta)
	{
		GetInput();
	}

	private void ChangeState(PlayerState state)
	{
		var player = this.PlayerShape2d;
		switch (state)
		{
			case PlayerState.Init:
				player.SetDeferred("disabled", true);
				break;
			case PlayerState.Alive:
				player.SetDeferred("disabled", false);
				break;
			case PlayerState.Invalulnerable:
				player.SetDeferred("disabled", true);
				break;
			case PlayerState.Dead:
				player.SetDeferred("disabled", true);
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

		_rotationDir = Input.GetAxis(new StringName("rotation_left"), new StringName("rotation_right"));
	}

	public override void _PhysicsProcess(double delta)
	{
		//base._PhysicsProcess(delta);
		ConstantForce = _thrust;
		ConstantTorque = _rotationDir * SpinPower;
	}
}
