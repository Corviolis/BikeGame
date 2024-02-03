#if TOOLS
using Godot;

namespace LilBikerBoi.addons.game;

[Tool]
public partial class Game : EditorPlugin
{
	public override void _EnterTree()
	{
		var script = GD.Load<Script>("res://addons/game/Package3D.cs");
		var texture = GD.Load<CompressedTexture2D>("res://ui/E.png");
		AddCustomType("Package3D", "RigidBody3D", script, texture);
	}

	public override void _ExitTree()
	{
		RemoveCustomType("Package3D");
	}
}
#endif
