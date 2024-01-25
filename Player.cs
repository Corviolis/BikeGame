using Godot;
using System;

public partial class Player : CharacterBody3D
{
	private int _maxTurnAngle = 1;
	private Sprite3D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite3D>("Sprite3D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("left", "right", "down", "up");

		float rawAngle = _sprite.Rotation.AngleTo(new Vector3(direction.X, 0, direction.Y));

		GD.Print(rawAngle);

		if (direction == new Vector2()) return;

		_sprite.RotateY(Math.Clamp(rawAngle, -1, 1) * (float)delta);
	}
}
