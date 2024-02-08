using Godot;

namespace LilBikerBoi.addons.game;

[Tool]
public partial class Package3D : RigidBody3D
{
    public enum PackageSize
    {
        Small,
        Medium,
        Large
    }

    [Export]
    public PackageSize Size { get; set; }

    public override void _EnterTree()
    {
        SetCollisionLayerValue(1, false);
        SetCollisionLayerValue(3, true);
        SetCollisionMaskValue(1, true);
        SetCollisionMaskValue(2, true);
        SetCollisionMaskValue(3, true);
    }
}