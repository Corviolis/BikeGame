using Godot;

public partial class CameraShader : MeshInstance3D
{
	public override void _Process(double delta)
	{
		ShaderMaterial blurshader = (ShaderMaterial)Mesh.SurfaceGetMaterial(0);
		blurshader.SetShaderParameter(new StringName("focal_point"), GetParent().GetParent<Node3D>().Position);
	}
}
