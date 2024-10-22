using System.Collections.Generic;
using Godot;
using LilBikerBoi.characters.you.bike;
using LilBikerBoi.resources;

namespace LilBikerBoi.characters.you.player;

public partial class Player : CharacterBody3D
{
	public bool ridingBike = false;

	[Export]
	private float _frictionRate = 1.4f;
	[Export]
	private float _speed = 12f;

	private AnimatedSprite3D _playerSprite;
	private Bike _bike;
	private Area3D _interactZone;
	private Sprite3D _interactLabel;
	private readonly List<IInteractibleZone> currentlyOverlappingZones = new();
	private bool _inInteraction;
	public PackagePool.PackageData HeldPackage = null;

	public override void _Ready()
	{
		_playerSprite = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
		_bike = GetNode<Bike>("../Bike");
		_interactZone = GetNode<Area3D>("PlayerInteractZone");
		_interactLabel = GetNode<Sprite3D>("InteractLabel");

		_interactZone.AreaEntered += area => {
			if (area is IInteractibleZone weezer) currentlyOverlappingZones.Add(weezer);
		};

		_interactZone.AreaExited += area => {
			if (area is IInteractibleZone weezer) currentlyOverlappingZones.Remove(weezer);
		};
	}

	public override void _Process(double delta)
	{
		// Tag interactibles with shader variable when they are closest
		_interactLabel.Visible = currentlyOverlappingZones.Count > 0;

	}

	public override void _PhysicsProcess(double delta)
	{
		if (ridingBike) {
			Position = new Vector3(_bike.Position.X, 0, _bike.Position.Z);
			Visible = false;
			return;
		}
		Visible = true;

		if (_inInteraction) return;

		Vector2 direction = Input.GetVector("left", "right", "down", "up");
		PlayerMovement(direction);
		AnimatePlayer(direction);
	}

	public override void _Input(InputEvent @event)
	{
		if (ridingBike)
		{
			_bike.InputFromParent(@event);
			return;
		}

		if (!_inInteraction && @event.IsActionPressed("interact")) {
			if (currentlyOverlappingZones.Count == 0) return;

			// sort overlapping zones so the closest one is the first one in the list
			// TODO: maybe a search would be more efficient?
			currentlyOverlappingZones.Sort(delegate(IInteractibleZone x, IInteractibleZone y) {
				float xDif = x.ReturnGlobalPosition().DistanceTo(Position);
				float yDif = y.ReturnGlobalPosition().DistanceTo(Position);
				if (xDif > yDif) return 1;
				if (yDif > xDif) return -1;
				return 0;
			});
			_inInteraction = true;
			currentlyOverlappingZones[0].Interact(this);
		}
	}

	private void PlayerMovement(Vector2 direction) {
		if (direction != Vector2.Zero) {
			Velocity = new Vector3(direction.X * _speed, 0, -direction.Y * _speed);
		} else {
			Velocity = Velocity.MoveToward(Vector3.Zero, _frictionRate);
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

	public void FinishInteraction()
	{
		_inInteraction = false;
	}
}
