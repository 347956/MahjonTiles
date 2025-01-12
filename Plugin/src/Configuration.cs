﻿using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;

namespace MahjongScrap.Configuration {
    public class PluginConfig
    {
        // For more info on custom configs, see https://lethal.wiki/dev/intermediate/custom-configs
        public ConfigEntry<int> MahjongTileSpawnWeight;
        public ConfigEntry<int> MahjongRedTileSpawnWeight;
        public ConfigEntry<bool> EnableMahjongTiles;
        public ConfigEntry<bool> EnableMahjongSoulPaintings;
        public ConfigEntry<bool> EnableMahjongSoulHalloweenPaintings;
        public ConfigEntry<int> MahjongSoulPaintingsSpawnWeight;

        public PluginConfig(ConfigFile cfg)
        {
            MahjongTileSpawnWeight = cfg.Bind("Mahjong Tiles", "Normal Tiles Spawn weight", 20,
                "The spawn chance weight for Mahjong Tiles, relative to other existing scrap.\n" +
                "Goes up from 0, lower is more rare, 100 and up is very common.");

            MahjongRedTileSpawnWeight = cfg.Bind("Mahjong Tiles", "Red Five Spawn weight", 5,
                "The spawn chance weight for Red Fives, relative to other existing scrap.\n" +
                "Goes up from 0, lower is more rare, 100 and up is very common.");

            EnableMahjongTiles = cfg.Bind("Mahjong Tiles", "Enable Mahjong Tiles", true,
                "Enables or Disables Mahjong Tiles");

            EnableMahjongSoulPaintings = cfg.Bind("Paintings", "Enable Mahjong Soul paintings", true,
                "Enables or Disables Mahjong Soul inspired paintings");
            
            MahjongSoulPaintingsSpawnWeight = cfg.Bind("Paintings", "SpawnWeight Mahjong Soul paintings", 5,
                "The spawn chance weight for Mahjong Soul paintings, relative to other existing scrap.\n" +
                "Goes up from 0, lower is more rare, 100 and up is very common.");

            EnableMahjongSoulHalloweenPaintings = cfg.Bind("Paintings", "Enable Mahjong Soul Halloween paintings", true,
                "Enables or Disables Mahjong Soul inspired Halloween paintings");

            ClearUnusedEntries(cfg);
        }

        private void ClearUnusedEntries(ConfigFile cfg) {
            // Normally, old unused config entries don't get removed, so we do it with this piece of code. Credit to Kittenji.
            PropertyInfo orphanedEntriesProp = cfg.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg, null);
            orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
            cfg.Save(); // Save the config file to save these changes
        }
    }
}