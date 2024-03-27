using Godot;
using System;

public partial class player : Area2D
{
	[Export]
	public float speed= 350;

	[Signal]
	public delegate void PickupEventHandler();

	[Signal]
	public delegate void HurtEventHandler();

	
	
	private Vector2 velocity = Vector2.Zero;
	public Vector2 screenSize = new Vector2(480, 720);
	
	public AnimatedSprite2D HeroAni2D => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			
	}	

	public override void _Process(double delta)
	{
		velocity = GetInputVector();
		this.Position += velocity * speed * (float)delta;

		HeroAni2D.Animation = velocity.Length() > 0 ? "run" : "idle";
		if (velocity.X != 0)
		{
			HeroAni2D.FlipH = velocity.X < 0;
		}
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
	
	public void Start()
	{
		SetProcess(true);
		Position = screenSize / 2;
		HeroAni2D.Animation = "idle";
	}

	public void Die()
	{
		HeroAni2D.Animation = "hurt";
		SetProcess(false);
	}
	
	private async void _on_area_entered(Area2D area)
	{
		//todo : 완성후  수정 예정 패턴매칭으로 바꿀것 
		if (area.IsInGroup("coins"))
		{
			var coinValue = area as coin;
			_ = coinValue ?? throw new Exception("wtf");
			await coinValue.Pickup();
			EmitSignal(SignalName.Pickup);
		}
		
		//todo 51 page 진행 예정 

		if (area.IsInGroup("obstacles"))
		{
			EmitSignal(SignalName.Hurt);
			Die();
		}
		
	}

}


