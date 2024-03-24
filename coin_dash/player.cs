using Godot;
using System;

public partial class player : Area2D
{
	[Export]
	public float speed= 350;
	
	private Vector2 velocity = Vector2.Zero;
	private Vector2 screenSize = new Vector2(480, 720);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		velocity = GetInputVector();
		this.Position += velocity * speed * (float)delta;
	}

	private Vector2 GetInputVector()
	{
		float inputHorizontal = 0f;
		float inputVertical = 0f;

		if (Input.IsActionPressed("ui_right"))
		{
			inputHorizontal += 1;
		}

		if (Input.IsActionPressed("ui_left"))
		{
			inputHorizontal -= 1;
		}

		if (Input.IsActionPressed("ui_down"))
		{
			inputVertical += 1;
		}

		if (Input.IsActionPressed("ui_up"))
		{
			inputVertical -= 1;
		}

		Vector2 inputVector = new Vector2(inputHorizontal, inputVertical);
		return inputVector.Normalized(); // 벡터를 정규화하여 대각선 이동 시 속도가 증가하지 않도록 합니다.
	}
}
