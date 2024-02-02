using Godot;
using LilBikerBoi.characters.you.player;

public partial class InteractZone : Area3D, IInteractibleZone
{
	private Node _parent;

	public override void _Ready()
	{
		_parent = GetParent();
	}

	void IInteractibleZone.Interact(Player player) {
		if (_parent is IInteractible carl) {
			carl.Interact(player);
		}
	}

	Vector3 IInteractibleZone.ReturnGlobalPosition() {
		return GlobalPosition;
	}
}
