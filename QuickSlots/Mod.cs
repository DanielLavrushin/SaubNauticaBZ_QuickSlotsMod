using Common.Utility;
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace QuickSlotsMod
{
    public static class Mod
    {
        public const int MaxSlots = 12;
        public const int MinSlots = 1;

        public static Config config;

        private static string modDirectory;
        private static string[] keys = new string[MaxSlots];

        public static void Load()
        {
            var harmony = new Harmony("subnautica.quickslotsmod.mod");
            LoadConfig();

            keys[5] = config.Slot6Key;
            keys[6] = config.Slot7Key;
            keys[7] = config.Slot8Key;
            keys[8] = config.Slot9Key;
            keys[9] = config.Slot10Key;
            keys[10] = config.Slot11Key;
            keys[11] = config.Slot12Key;

            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log("Patched with harmony");

        }

        private static string GetModInfoPath()
        {
            return new FileInfo(typeof(QuickSlotsMod.Mod).Assembly.Location).Directory.FullName + "/mod.json";
        }

        private static void LoadConfig()
        {
            string modInfoPath = GetModInfoPath();
            Logger.Log("Config path " + modInfoPath);
            if (!File.Exists(modInfoPath))
            {
                config = new Config();
                Logger.Log("Unable to load config file from the path " + modInfoPath);
                return;
            }

            var modInfoObject = JSON.Parse(File.ReadAllText(modInfoPath));
            string configJson = modInfoObject["Config"].ToString();
            config = JsonUtility.FromJson<Config>(configJson);
            ValidateConfig();
        }

        private static void ValidateConfig()
        {
            Config defaultConfig = new Config();
            if (config == null)
            {
                config = defaultConfig;
                return;
            }

            if (config.SlotCount < MinSlots || config.SlotCount > MaxSlots)
            {
                config.SlotCount = defaultConfig.SlotCount;
            }
        }

        public static string GetInputForSlotFormatted(int slotID)
        {
            if (slotID < Player.quickSlotButtonsCount)
            {
                string inputName = GameInput.GetBindingName(GameInput.Button.Slot1 + slotID, GameInput.BindingSet.Primary);
                string input = LanguageCache.GetButtonFormat("{0}", GameInput.Button.Slot1 + slotID);
                return string.IsNullOrEmpty(inputName) ? "" : input;
            }
            if (slotID < 0 || slotID >= MaxSlots)
            {
                return "???";
            }

            return $"<color=#ADF8FFFF>{keys[slotID]}</color>";
        }
        public static string GetInputForSlot(int slotID)
        {
            if (slotID < Player.quickSlotButtonsCount)
            {
                string inputName = GameInput.GetBindingName(GameInput.Button.Slot1 + slotID, GameInput.BindingSet.Primary);
                string input = LanguageCache.GetButtonFormat("{0}", GameInput.Button.Slot1 + slotID);
                return string.IsNullOrEmpty(inputName) ? "" : input;
            }
            if (slotID < 0 || slotID >= MaxSlots)
            {
                return "???";
            }

            return keys[slotID];
        }
        public static bool GetKeyDownForSlot(int slotID)
        {
            return slotID >= Player.quickSlotButtonsCount && slotID < Mod.MaxSlots && Input.GetKeyDown(Mod.GetInputForSlot(slotID));
        }
    }
}
