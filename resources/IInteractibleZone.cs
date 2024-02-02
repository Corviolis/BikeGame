using Godot;
using LilBikerBoi.characters.you.player;

public interface IInteractibleZone {
	void Interact(Player player);
	Vector3 ReturnGlobalPosition();
}

public interface IInteractible {
	void Interact(Player player);
}