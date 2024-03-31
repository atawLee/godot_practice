using Godot;
using System;

namespace space_rock.Player;  
public partial class Player : RigidBody2D
{
	public PlayerState CurrentState = PlayerState.Init; 
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ChangeState(PlayerState state) 
		=> CurrentState = state;
}
