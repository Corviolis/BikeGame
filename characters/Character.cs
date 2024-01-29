using Godot;
using LilBikerBoi.resources;

[Tool]
public partial class Character : CharacterBody3D
{
	private string _characterName;

	[Export]
	public string CharacterName {
		get => _characterName;
		set {
			_characterName = value;
			LoadTexture();
		}
	}

	public override void _Ready()
	{
		GetNode<MeshInstance3D>("Marker").QueueFree();

		if (Engine.IsEditorHint()) return;
		LoadTexture();
	}

	private void LoadTexture()
	{
		string textureFile = $"res://characters/{_characterName}/{_characterName}.png";
		if (!FileAccess.FileExists(textureFile)) return;

		CompressedTexture2D image = GD.Load<CompressedTexture2D>(textureFile);
		Sprite3D sprite = GetNode<Sprite3D>("Sprite3D");
		sprite.Texture = image;
	}

	public void StartDialog()
	{
		DialogicSharp.Start(_characterName);
	}
}
