using Godot;

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
	private float _cameraFollowSpeedPlayer = 1.2f;
	[Export]
	private float _cameraFollowSpeedBike = 0.1f;
	[Export]
	private bool _lookAtPlayer = true;
	[ExportGroup("Camera Distance")]
	[Export]
	private float _playerDistance = 10f;
	[Export]
	private float _bikeDistance = 17.6f;
	[Export]
	private float _cameraChangeSpeed = 1f;

	private Node3D _player;
	private Node3D _parent;
	private float _cameraFollowSpeed;

	public override void _Ready()
	{
		_player = GetNode<Node3D>("../../Player");
		_parent = GetParentNode3D();

		ChangeCameraToPlayer += () => ChangeCameraDistance(_playerDistance, _cameraFollowSpeedPlayer);
		ChangeCameraToBike += () => ChangeCameraDistance(_bikeDistance, _cameraFollowSpeedBike);
		ChangeCameraZoom += ChangeCameraDistance;

		CameraZoom(_playerDistance, _cameraFollowSpeedPlayer, 0f);
	}

	public override void _PhysicsProcess(double delta)
	{
		_parent.Position = _parent.Position.Lerp(_player.Position, _cameraFollowSpeed);
		if (_lookAtPlayer) LookAt(_player.Position);
	}

	private void ChangeCameraDistance(float distance, float cameraFollowSpeed) {
		CameraZoom(distance, cameraFollowSpeed, _cameraChangeSpeed);
	}

	private void CameraZoom(float distance, float cameraFollowSpeed, float speed) {
		Vector2 newPosition = new Vector2(Position.Z, Position.Y).Normalized() * distance;
		
		Tween cameraZoom = CreateTween();
		cameraZoom.TweenProperty(this, "position", new Vector3(0, newPosition.Y, newPosition.X), speed)
			.SetEase(Tween.EaseType.Out)
			.FromCurrent()
			.SetTrans(Tween.TransitionType.Quart);
		
		Tween cameraFollow = CreateTween();
		cameraFollow.TweenProperty(this, "_cameraFollowSpeed", cameraFollowSpeed, speed)
			.SetEase(Tween.EaseType.Out)
			.FromCurrent()
			.SetTrans(Tween.TransitionType.Quart);
	}

}
