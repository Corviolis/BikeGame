using Godot;

public partial class InteractZone : Area3D, IInteractibleZone
{
	void IInteractibleZone.Interact() {
		GD.Print($"{GetParentNode3D().Name}: I was interacted with!");
	}

	Vector3 IInteractibleZone.ReturnGlobalPosition() {
		return GlobalPosition;
	}
}
