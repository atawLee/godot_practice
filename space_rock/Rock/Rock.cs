using Godot;
using System;
using System.Threading.Tasks;

public partial class Rock : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.

	[Signal]
	public delegate void ExplodedEventHandler(float size, float radius, Vector2 position, Vector2 linearVelocity);
	
	public Vector2 ScreenSize { get; set; } = Vector2.Zero;
	private float _size;
	private float _radius;
	private float _scale_factor = 0.2f;

	private TaskCompletionSource<bool> _tcs; //완료 확인을 위한 tcs 

	
	public override void _Ready()
	{
		var animationPlayer = GetNode<AnimationPlayer>("Explosion/AnimationPlayer");
	
		var callable = new Callable(this, nameof(OnAnimationFinished));
		animationPlayer.Connect("animation_finished", callable);
	}

	
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

		GetNode<Sprite2D>("Explosion").Scale = Vector2.One * 0.75f * this._size;

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

	public void Explode()
	{
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled",true);
		GetNode<Sprite2D>("Sprite2D").Hide();
		GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("explosion");
		GetNode<Sprite2D>("Explosion").Show();

		EmitSignal(SignalName.Exploded, this._size, _radius, Position, LinearVelocity);

		this.LinearVelocity = Vector2.Zero;
		AngularVelocity = 0;
	}
	private void OnAnimationFinished(string animName)
	{
		QueueFree();
	}
	
}
