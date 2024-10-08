using System.Reflection;
using UnityEngine;
using BepInEx;
using LethalLib.Modules;
using BepInEx.Logging;
using System.IO;
using MahjongScrap.Configuration;
using System;
using System.Collections.Generic;

namespace MahjongScrap {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID)] 
    public class Plugin : BaseUnityPlugin {
        internal static new ManualLogSource Logger = null!;
        internal static PluginConfig BoundConfig { get; private set; } = null!;
        public static AssetBundle? ModAssets;

        private void Awake() {
            Logger = base.Logger;

            // If you don't want your mod to use a configuration file, you can remove this line, Configuration.cs, and other references.
            BoundConfig = new PluginConfig(base.Config);

            InitializeNetworkBehaviours();

            var bundleName = "mahjongscrapmodassets";
            ModAssets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Info.Location), bundleName));
            if (ModAssets == null) {
                Logger.LogError($"Failed to load custom assets.");
                return;
            }

            //Checks if Tiles are enabled > Registers and loads the tiles if enabled
            bool tilesEnabled = BoundConfig.EnableMahjongTiles.Value;
            if (tilesEnabled)
            {
                Logger.LogInfo($"EnableMahjongTiles = [{tilesEnabled}] > enabling Mahjong Tiles");
                RegisterTiles();
            }
            else
            {
                Logger.LogInfo($"EnableMahjongTiles = [{tilesEnabled}] > skipped loading Mahjong Tiles");
            }


            //Checks if Paintings are enabled > Registers and loads the paintings if enabled
            bool paintingsEnabled = BoundConfig.EnableMahjongSoulPaintings.Value;
            if (paintingsEnabled)
            {
                Logger.LogInfo($"EnableMahjongSoulPaintings = [{tilesEnabled}] > enabling Mahjong Soul Paintings");
                RegisterPaintings();
            }
            else
            {
                Logger.LogInfo($"EnableMahjongSoulPaintings = [{tilesEnabled}] > skipped loading Mahjong Soul Paintings");
            }
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        //To keep the code clean

        //Register all tiles.
        private void RegisterTiles()
        {
            if (ModAssets == null)
            {
                Logger.LogWarning("The asset bundel is null or not set!");
            }

            List<Item> tiles = LoadTiles();
            List<Item> redTiles = LoadRedTiles();

            if (tiles != null && tiles.Count > 0)
            {
                int iTileRarity = BoundConfig.MahjongTileSpawnWeight.Value;
                foreach (Item tile in tiles)
                {
                    NetworkPrefabs.RegisterNetworkPrefab(tile.spawnPrefab);
                    Items.RegisterScrap(tile, iTileRarity, Levels.LevelTypes.All);
                }
                Logger.LogInfo("Done loading normal tiles");
            }
            if (redTiles != null && redTiles.Count > 0)
            {
                int iRedTileRarity = BoundConfig.MahjongTileSpawnWeight.Value;
                foreach (Item tile in redTiles)
                {
                    NetworkPrefabs.RegisterNetworkPrefab(tile.spawnPrefab);
                    Items.RegisterScrap(tile, iRedTileRarity, Levels.LevelTypes.All);
                }
                Logger.LogInfo("Done loading red fives");
            }
        }

        //Load all normal tile Assets
        private List<Item> LoadTiles()
        {
            List<Item> tiles = new List<Item>();
            if (ModAssets != null)
            {
                tiles.Add(ModAssets.LoadAsset<Item>("EastTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouthTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("WestTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("NorthTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("GreenDragontile"));
                tiles.Add(ModAssets.LoadAsset<Item>("RedDragonTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("WhiteDragonTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuOneTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuTwoTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuThreeTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuFourTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuFiveTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuSixTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuSevenTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuEightTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("PinzuNineTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuOneTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuTwoTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuThreeTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuFourTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuFiveTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuSixTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuSevenTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuEightTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("SouzuNineTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("ManzuOneTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("ManzuTwoTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("ManzuThreeTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("ManzuFourTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("ManzuFiveTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("ManzuSixTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("ManzuSevenTile"));
                tiles.Add(ModAssets.LoadAsset<Item>("ManzuEightTile"));
            }
            else
            {
                Logger.LogWarning("Something went wrong with loading the tiles");
            }

            return tiles;
        }

        //Load Red Five Assets
        private List<Item> LoadRedTiles()
        {

            List<Item> redTiles = new List<Item>();

            if (ModAssets != null) {
                redTiles.Add(ModAssets.LoadAsset<Item>("ManzuRedFiveTile"));
                redTiles.Add(ModAssets.LoadAsset<Item>("PinzuRedFiveTile"));
                redTiles.Add(ModAssets.LoadAsset<Item>("SouzuRedFiveTile"));
            }
            else
            {
                Logger.LogWarning("Something went wrong with loading the red tiles");
            }

            return redTiles;
        }

        private void RegisterPaintings()
        {

            if (ModAssets == null)
            {
                Logger.LogWarning("The asset bundel is null or not set!");
            }

            List<Item> paintings = LoadPaintings();

            if (paintings != null && paintings.Count > 0)
            {
                int iTileRarity = BoundConfig.MahjongSoulPaintingsSpawnWeight.Value;
                foreach (Item painting in paintings)
                {
                    NetworkPrefabs.RegisterNetworkPrefab(painting.spawnPrefab);
                    Items.RegisterScrap(painting, iTileRarity, Levels.LevelTypes.All);
                }
                Logger.LogInfo("Done loading mahjong soul paintings");
            }
        }


        private List<Item> LoadPaintings()
        {
            List<Item> paintings = new List<Item>();
            if (ModAssets != null)
            {
                paintings.Add(ModAssets.LoadAsset<Item>("PaintingPortraitIchihime"));
            }
            else
            {
                Logger.LogWarning("Something went wrong with loading the paintings");
            }
            return paintings;
        }

        private static void InitializeNetworkBehaviours() {
            // See https://github.com/EvaisaDev/UnityNetcodePatcher?tab=readme-ov-file#preparing-mods-for-patching
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        } 
    }
}