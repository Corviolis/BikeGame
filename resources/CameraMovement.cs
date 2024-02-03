using Godot;
using LilBikerBoi.characters.you.bike;
using LilBikerBoi.characters.you.player;

public partial class CameraMovement : Camera3D
{
	[Signal]
	public delegate void ChangeCameraToPlayerEventHandler();
	[Signal]
	public delegate void ChangeCameraToBikeEventHandler();
	[Signal]
	public delegate void ChangeCameraZoomEventHandler(float distance, float cameraFollowSpeed);


	[ExportGroup("Camera Movement")]
	[Export]
	private float _followSpeedPlayer = 1.2f;
	[Export]
	private float _followSpeedBike = 0.1f;
	[Export]
	private bool _lookAtPlayer = true;
	[ExportGroup("Camera Distance")]
	[Export]
	private float _playerZoomDistance = 10f;
	[Export]
	private float _bikeZoomDistance = 17.6f;
	[Export]
	private float _zoomChangeSpeed = 1f;
	[Export]
	private float _velocityZoomOffset = 1f;

	private Player _player;
	private Bike _bike;
	private Node3D _parent;
	private float _cameraFollowSpeed;
	private float _targetDistance;

	public override void _Ready()
	{
		_player = GetNode<Player>("../../Player");
		_bike = GetNode<Bike>("../../Bike");
		_parent = GetParentNode3D();

		ChangeCameraToPlayer += () => ChangeCameraDistance(_playerZoomDistance, _followSpeedPlayer);
		ChangeCameraToBike += () => ChangeCameraDistance(_bikeZoomDistance, _followSpeedBike);
		ChangeCameraZoom += ChangeCameraDistance;

		_targetDistance = _playerZoomDistance;
		CameraTweenZoom(_playerZoomDistance, _followSpeedPlayer, 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		_parent.Position = _parent.Position.Lerp(_player.Position, _cameraFollowSpeed);
		ZoomCameraByVelocity();
		if (_lookAtPlayer) LookAt(_player.Position);
	}

	private void ChangeCameraDistance(float distance, float cameraFollowSpeed) {
		CameraTweenZoom(distance, cameraFollowSpeed, _zoomChangeSpeed);
	}

	private void CameraTweenZoom(float distance, float cameraFollowSpeed, float speed) {
		Tween cameraZoom = CreateTween();
		cameraZoom.TweenProperty(this, "_targetDistance", distance, speed)
			.SetEase(Tween.EaseType.Out)
			.FromCurrent()
			.SetTrans(Tween.TransitionType.Quart);
		
		Tween cameraFollow = CreateTween();
		cameraFollow.TweenProperty(this, "_cameraFollowSpeed", cameraFollowSpeed, speed)
			.SetEase(Tween.EaseType.Out)
			.FromCurrent()
			.SetTrans(Tween.TransitionType.Quart);
	}

	private void ZoomCameraByVelocity() {
		Vector2 cameraDirection = new Vector2(Position.Z, Position.Y).Normalized();

		float targetDistance = _targetDistance + (_velocityZoomOffset * (_player.ridingBike ? _bike.Velocity.DistanceTo(Vector3.Zero) : 0f));
		Vector3 newCameraPosition = new Vector3(0, cameraDirection.Y * targetDistance, cameraDirection.X * targetDistance);
		Position = Position.Lerp(newCameraPosition, 1f);
	}
}
