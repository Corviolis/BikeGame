using System.Collections.Generic;
using Godot;

namespace LilBikerBoi.characters.you.player;

public partial class Player : CharacterBody3D
{
	public bool ridingBike = false;

	[Export]
	private float _frictionRate = 140f;
	[Export]
	private float _speed = 1300f;

	private AnimatedSprite3D _playerSprite;
	private Node3D _bike;
	private Area3D _interactZone;
	private Sprite3D _interactLabel;
	private readonly List<IInteractibleZone> currentlyOverlappingZones = new();

	public override void _Ready()
	{
		_playerSprite = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
		_bike = GetNode<Node3D>("../Bike");
		_interactZone = GetNode<Area3D>("PlayerInteractZone");
		_interactLabel = GetNode<Sprite3D>("InteractLabel");

		void incrementAreaEntered(Area3D area) {
			if (area is IInteractibleZone weezer)
			currentlyOverlappingZones.Add(weezer);
		}
		_interactZone.AreaEntered += incrementAreaEntered;
		void decrementAreaEntered(Area3D area) {
			if (area is IInteractibleZone weezer)
			currentlyOverlappingZones.Remove(weezer);
		}
		_interactZone.AreaExited += decrementAreaEntered;
	}

	public override void _Process(double delta)
	{
		_interactLabel.Visible = currentlyOverlappingZones.Count > 0;

	}

	public override void _PhysicsProcess(double delta)
	{
		if (ridingBike) {
			Position = _bike.Position;
			return;
		}

		Vector2 direction = Input.GetVector("left", "right", "down", "up");
		PlayerMovement(direction, (float)delta);
		AnimatePlayer(direction);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact")) {
			if (currentlyOverlappingZones.Count == 0) return;

			// sort overlapping zones so the closest one is the first one in the list
			// TODO: maybe a search would be more efficient?
			currentlyOverlappingZones.Sort(delegate(IInteractibleZone x, IInteractibleZone y) {
				float xDif = x.ReturnGlobalPosition().DistanceTo(Position);
				float yDif = y.ReturnGlobalPosition().DistanceTo(Position);
				if (xDif > yDif) return 1;
				else if (yDif > xDif) return -1;
				else return 0;
			});
			currentlyOverlappingZones[0].Interact();
		}
	}

	private void PlayerMovement(Vector2 direction, float delta) {
		if (direction != Vector2.Zero) {
			Velocity = new Vector3(direction.X * _speed * delta, 0, -direction.Y * _speed * delta);
		} else {
			Velocity = Velocity.MoveToward(Vector3.Zero, _frictionRate * delta);
		}

		MoveAndSlide();
	}

	private void AnimatePlayer(Vector2 direction) {
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
