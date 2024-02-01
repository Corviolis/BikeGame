using Godot;

public partial class CameraMovement : Camera3D
{
	[Export]
	private float _cameraFollowSpeed = 1f;
	[Export]
	private bool _lookAtPlayer = true;
	private Node3D _player;
	private Node3D _parent;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetNode<Node3D>("../../Player");
		_parent = GetParentNode3D();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		_parent.Position = _parent.Position.Lerp(_player.Position, _cameraFollowSpeed);
		if (_lookAtPlayer) LookAt(_player.Position);
	}
}
