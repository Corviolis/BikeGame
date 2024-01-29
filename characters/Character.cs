using Godot;
using System;
using LilBikerBoi.resources;

[Tool]
public partial class Character : CharacterBody3D
{
	private String _characterName;

	[Export]
	public String CharacterName {
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
		String textureFile = $"res://characters/{_characterName}/{_characterName}.png";
		if (!FileAccess.FileExists(textureFile)) return;

		CompressedTexture2D image = GD.Load<CompressedTexture2D>(textureFile);
		Sprite3D sprite = GetNode<Sprite3D>("Sprite3D");
		sprite.Texture = image;
	}

	public void StartDialog()
	{
		//Dialogic.Start(_characterName);
	}
}
