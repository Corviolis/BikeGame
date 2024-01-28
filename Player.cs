using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	private float _turnSpeed = 7f;	
	[Export]
	private float _maxTurnDelta = Mathf.Pi/6;	
	[Export]
	private float _moveSpeed = 100f;
	[Export]
	private float _accelerationRate = 500f;
	[Export]
	private float _frictionRate = 0.1f;
	[Export]
	private float _brakeFrictionRate = 6f;

	private Sprite3D _sprite;


	public override void _Ready()
	{
		_sprite = GetNode<Sprite3D>("Sprite3D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("left", "right", "down", "up");
		RotateBike(direction, (float)delta);
		MoveBikeForward(direction, (float)delta);
	}

	private void RotateBike(Vector2 direction, float delta) {
		// TODO: add some sort of 'memory', so if you turn 180 it remembers 
		//   whether your last rotation was clockwise or counter?
		if (direction != Vector2.Zero) {
			float clampedDifference = Mathf.Clamp(Mathf.AngleDifference(Rotation.Y, direction.Angle()), -_maxTurnDelta, _maxTurnDelta);
			float interpolatedAngle = Mathf.LerpAngle(Rotation.Y, Rotation.Y + clampedDifference, _turnSpeed * delta);
			Vector3 newRotation = new Vector3(0, interpolatedAngle, 0);
			Rotation = newRotation;
		}
	}

	private void MoveBikeForward(Vector2 direction, float delta) {
		// grab the speed/radius of the current and input velocities
		// interpolate the speed between the current and input speeds
		// apply it to the forward direction/angle of the bike's rotation

		if (direction != Vector2.Zero) {
			// grab the 'length' of the direction, a.k.a. the distance of the joystick from neutral or it's speed
			float currentSpeed = Mathf.Sqrt(Mathf.Pow(Velocity.X, 2) + Mathf.Pow(Velocity.Y, 2));
			float inputSpeed = Mathf.Sqrt(Mathf.Pow(direction.X, 2) + Mathf.Pow(direction.Y, 2)) * _moveSpeed;

			if (inputSpeed < currentSpeed)
				inputSpeed = currentSpeed;

			float speed = Mathf.Lerp(currentSpeed, inputSpeed, _accelerationRate * delta);

			GD.Print("cur: " + currentSpeed);
			GD.Print("in: " + inputSpeed);

			//float speed = inputSpeed > currentSpeed ? currentSpeed : inputSpeed;




			// multiply that 'length' by the forward direction of the model
			Velocity = new Vector3(Mathf.Cos(Rotation.Y), 0 , -Mathf.Sin(Rotation.Y)) * speed * delta;
		} else {
			// slow down
			//float brakeRatio = (float) Mathf.Clamp(Input.GetActionStrength("brake"), 0.5, 1);
			//if (brakeRatio > 0.5)
			//	Velocity = Velocity.Lerp(Vector3.Zero, brakeRatio * _brakeFrictionRate * delta);
		}

		MoveAndSlide();
	}
}