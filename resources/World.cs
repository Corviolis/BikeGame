using Godot;
using LilBikerBoi.resources;

public partial class World : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("packages"))
		{
			Vector3 pos = GetNode<Marker3D>("PackageSpawnPoint").GlobalPosition;
			foreach (var packageData in PackagePool.Generate())
			{
				GD.Print("pain");
				Node3D package = packageData.GetAsNode3D();
				AddChild(package);
				package.GlobalPosition = pos;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
