using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	private float _turnSpeed = 7;	
	[Export]
	private float _moveSpeed = 10;
	[Export]
	private float _accelerationRate = 0.1f;
	[Export]
	private float _frictionRate = 0.05f;
	private Sprite3D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite3D>("Sprite3D");
	}

	public override void _PhysicsProcess(double delta)
	{
		// turns 180 faster than 90, might need to refactor
		// for claification, this is because we are moving from point a to point b over a set time,
		//   rather than changing the angle by a set amount
		Vector2 direction = Input.GetVector("left", "right", "down", "up");
		if (direction != Vector2.Zero) {
			float interpolatedAngle = Mathf.LerpAngle(Rotation.Y, direction.Angle(), (float)(_turnSpeed * delta));
			Vector3 newAngle = new Vector3(0, interpolatedAngle, 0);
			Rotation = newAngle;
		}

		if (direction != Vector2.Zero) {
			// grab the 'length' of the direction, a.k.a. the distance of the joystick from neutral
			float inputVelocity = Mathf.Sqrt(Mathf.Pow(direction.X, 2) + Mathf.Pow(direction.Y, 2)) * _moveSpeed;
			// multiply that 'length' by the forward direction of the model
			Velocity = Velocity.Lerp(new Vector3(Mathf.Cos(Rotation.Y), 0 , -Mathf.Sin(Rotation.Y)) * inputVelocity, _accelerationRate);
		} else {
			Velocity = Velocity.Lerp(Vector3.Zero, _frictionRate);
		}

		MoveAndSlide();
	}
}
