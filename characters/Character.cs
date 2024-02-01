using Godot;
using LilBikerBoi.resources;

namespace LilBikerBoi.characters;

[Tool]
public partial class Character : CharacterBody3D
{
	private string _characterName;
	private MeshInstance3D _marker;
	private Sprite3D _sprite;

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
		_sprite = GetNode<Sprite3D>("Sprite3D");
		_marker = GetNode<MeshInstance3D>("Marker");

		if (Engine.IsEditorHint()) return;
		LoadTexture();
	}

	private void LoadTexture()
	{
		if (_sprite == null || _marker == null) return;

		string textureFile = $"res://characters/{_characterName}/{_characterName}.png";
		if (!FileAccess.FileExists(textureFile))
		{
			_sprite.Texture = null;
			_marker.Show();
			return;
		}
		_marker.Hide();

		CompressedTexture2D image = GD.Load<CompressedTexture2D>(textureFile);
		_sprite.Texture = image;
	}

	public void StartDialog()
	{
		DialogicSharp.Start(_characterName);
	}
}
