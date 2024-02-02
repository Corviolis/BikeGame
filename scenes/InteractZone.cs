using Godot;

public partial class InteractZone : Area3D, IInteractibleZone
{
	private Node _parent;

	public override void _Ready()
	{
		_parent = GetParent();
	}

	void IInteractibleZone.Interact() {
		if (_parent is IInteractible carl) {
			carl.Interact();
		}
	}

	Vector3 IInteractibleZone.ReturnGlobalPosition() {
		return GlobalPosition;
	}
}
