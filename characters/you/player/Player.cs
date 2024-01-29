using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	private float _frictionRate = 140f;
	[Export]
	private float _speed = 1300f;

	private AnimatedSprite3D _playerSprite;

	public override void _Ready()
	{
		_playerSprite = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
	}
	
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("left", "right", "down", "up");
		playerMovement(direction, (float)delta);
		animatePlayer(direction);
	}

	private void playerMovement(Vector2 direction, float delta) {
		if (direction != Vector2.Zero) {
			Velocity = new Vector3(direction.X * _speed * delta, 0, -direction.Y * _speed * delta);
		} else {
			Velocity = Velocity.MoveToward(Vector3.Zero, _frictionRate * delta);
		}

		MoveAndSlide();
	}

	private void animatePlayer(Vector2 direction) {
		if (direction == Vector2.Zero) {
			_playerSprite.Play("idle");
			return;
		}
		
		direction = direction.Normalized();

		if (Mathf.Abs(direction.X) + 0.1f < Mathf.Abs(direction.Y)) {
			if (direction.Y > 0) {
				_playerSprite.Play("up");
			} else {
				_playerSprite.Play("down");
			}
		} else {
			if (direction.X > 0) {
				_playerSprite.Play("right");
			} else {
				_playerSprite.Play("left");
			}
		}
	}
}
