using System;
using System.Collections.Generic;
using Godot;
using LilBikerBoi.characters;

namespace LilBikerBoi.resources;

public static class PackagePool
{
    private const int ShortStories = 1;
    private const int MediumStories = 1;
    private const int LongStories = 1;

    private static int day = 1;
    private static readonly Random Rnd = new();
    private static readonly Dictionary<int, List<CharacterData>> Recipients = new();

    public class PackageData
    {
        public readonly Character Character;
        private readonly PackedScene _packageModel;
        public string Address;

        public PackageData(Character character, PackedScene packageModel, string address)
        {
            Character = character;
            _packageModel = packageModel;
            Address = address;
        }

        public Node3D GetAsNode3D()
        {
            return _packageModel.Instantiate<Node3D>();
        }
    }

    private class CharacterData
    {
        private readonly Character _character;
        private int _packageIndex;

        public CharacterData(Character character)
        {
            _character = character;
        }

        public int GetRemainingPackages()
        {
            return _character.GetTotalPackages() - _packageIndex - Convert.ToInt16(_character.WasDeliveredTo);
        }

        public PackageData GetNextPackage()
        {
            PackageData packageData = _character.GetPackage(_packageIndex);
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

        for (int i = 0; i < amount; i++)
        {
            int f = Rnd.Next(characters.Count);
            //GD.Print(f);
            packages.Add(characters[f].GetNextPackage());
        }
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
            foreach (var data in characterData)
            {
                int remainingPackages = data.GetRemainingPackages();
                if (remainingPackages == 0 || remainingPackages > 3 - day)
                {
                    List<CharacterData> characters = Recipients.GetValueOrDefault(packageCount, new());
                    characters.Remove(data);
                }
            }
        }
    }
}