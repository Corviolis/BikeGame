using Godot;

public partial class Player : CharacterBody3D
{
	[Export(PropertyHint.Range, "0, 50, or_greater")]
	private int _turnSpeed = 10;
	private Sprite3D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite3D>("Sprite3D");
	}

	public override void _PhysicsProcess(double delta)
	{
		// grab vec2 from input
		// convert to radians
		// lerp between current position and new position
		Vector2 direction = Input.GetVector("left", "right", "down", "up");
		if (direction.X == 0 && direction.Y == 0) { return; }
		if (_sprite.Rotation.Y == direction.Angle()) { return; }
		float interpolatedAngle = Mathf.LerpAngle(_sprite.Rotation.Y, direction.Angle(), (float)(_turnSpeed * delta));
		Vector3 newAngle = new Vector3(0, interpolatedAngle, 0);
		_sprite.Rotation = newAngle;
	}
}
