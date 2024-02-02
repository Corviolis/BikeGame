using Godot;

public interface IInteractibleZone {
	void Interact();
	Vector3 ReturnGlobalPosition();
}

public interface IInteractible {
	void Interact();
}