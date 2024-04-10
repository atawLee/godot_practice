using Godot;
using System;

public partial class Rock : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.

	public Vector2 ScreenSize { get; set; } = Vector2.Zero;
	private float _size;
	private float _radius;
	private float _scale_factor = 0.2f;
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Start(Vector2 position, Vector2 velocity, float size)
	{
		this.Position = position;
		this._size = size;
		this.Mass = 1.5f * size;
		var sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Scale = Vector2.One * _scale_factor * size;
		this._radius = sprite.Texture.GetSize().X / 2 * sprite.Scale.X;
		var shape = new CircleShape2D();
		shape.Radius = _radius;
		GetNode<CollisionShape2D>("CollisionShape2D").Shape = shape;
		LinearVelocity = velocity;
		AngularVelocity = RandomUtils.RandfRange((float)-Math.PI, (float)Math.PI);

	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		var xForm = state.Transform;
		xForm.Origin.X = Wrapf(xForm.Origin.X, 0 - _radius, ScreenSize.X + _radius);
		xForm.Origin.Y = Wrapf(xForm.Origin.Y, 0 - _radius, ScreenSize.Y + _radius);
		state.Transform = xForm;
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
}
