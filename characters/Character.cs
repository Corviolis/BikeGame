using Godot;
using Godot.Collections;
using LilBikerBoi.characters.you.player;
using LilBikerBoi.resources;

namespace LilBikerBoi.characters;

[Tool]
public partial class Character : CharacterBody3D, IInteractible
{
	private string _characterName;
	private MeshInstance3D _marker;
	private Sprite3D _sprite;
	public bool WasDeliveredTo;

	[Export]
	public string CharacterName {
		get => _characterName;
		set {
			_characterName = value;
			LoadTexture();
		}
	}

	[Export] public bool Interactable = true;
	[Export] public string StreetAddress;
	[Export] public Array<PackedScene> Packages;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite3D>("Sprite3D");
		_marker = GetNode<MeshInstance3D>("Marker");
		LoadTexture();

		if (Engine.IsEditorHint()) return;
		if (!Interactable) GetNode("InteractZone").QueueFree();
		PackagePool.Register(Packages.Count, this);
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

	public int GetTotalPackages()
	{
		return Packages.Count;
	}

	public PackagePool.PackageData GetPackage(int packageIndex)
	{
		GD.Print("ugh: " + packageIndex);
		return new PackagePool.PackageData(this, Packages[packageIndex], StreetAddress);
	}

	public void Interact(Player player)
	{
		DialogicSharp.TimelineEnded(() => player.FinishInteraction());

		if (player.HeldPackage == null || !player.HeldPackage.Character.Equals(this))
		{
			DialogicSharp.Start(_characterName + "_default");
			return;
		}

		DialogicSharp.Start(_characterName);
		WasDeliveredTo = true;
	}
}