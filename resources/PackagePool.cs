using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LilBikerBoi.addons.game;
using LilBikerBoi.characters;

namespace LilBikerBoi.resources;

public static class PackagePool
{
    private const int ShortStories = 1;
    private const int MediumStories = 1;
    private const int LongStories = 1;

    private static readonly Random Rng = new();
    private static readonly Dictionary<int, List<CharacterData>> Recipients = new();

    public class PackageData
    {
        public readonly Character Character;
        private readonly PackedScene _packageModel;
        private readonly CompressedTexture2D _label;

        public PackageData(Character character, PackedScene packageModel, CompressedTexture2D label)
        {
            Character = character;
            _packageModel = packageModel;
            _label = label;
        }

        public Package3D GetAsNode()
        {
            return _packageModel.Instantiate<Package3D>();
        }
    }

    private class CharacterData
    {
        public readonly Character Character;
        private int _packageIndex;

        public CharacterData(Character character)
        {
            Character = character;
        }

        public int GetRemainingPackages()
        {
            return Character.GetTotalPackages() - _packageIndex - Convert.ToInt16(Character.WasDeliveredTo);
        }

        public PackageData GetNextPackage()
        {
            PackageData packageData = Character.GetPackage(_packageIndex);
            _packageIndex++;
            return packageData;
        }
    }

    public static void Register(int storySize, Character character)
    {
        List<CharacterData> characters = Recipients.GetValueOrDefault(storySize, new());
        characters.Add(new CharacterData(character));
        Recipients.Add(storySize, characters);
    }

    private static List<PackageData> GetRandomPackages(int storySize, int amount)
    {
        List<PackageData> packages = new();
        List<CharacterData> characters = Recipients.GetValueOrDefault(storySize, new());
        if (characters.Count == 0) return packages;

        List<CharacterData> shuffledCharacters = characters.OrderBy(_ => Rng.Next()).ThenBy(c => c.Character.WasDeliveredTo).ToList();
        for (int i = 0; i < amount && i < characters.Count; i++) packages.Add(shuffledCharacters[i].GetNextPackage());
        return packages;
    }

    public static List<PackageData> Generate()
    {
        List<PackageData> packages = new();
        packages.AddRange(GetRandomPackages(1, ShortStories));
        packages.AddRange(GetRandomPackages(2, MediumStories));
        packages.AddRange(GetRandomPackages(3, LongStories));
        return packages;
    }

    public static void RemoveUnreachable()
    {
        foreach (var (packageCount, characterData) in Recipients)
        {
            Recipients.TryGetValue(packageCount, out List<CharacterData> characters);
            if (characters == null) continue;

            foreach (var data in characterData)
            {
                int remainingPackages = data.GetRemainingPackages();
                if (remainingPackages == 0 || remainingPackages > 3 - World.Day)
                    characters.Remove(data);
            }
        }
    }
}