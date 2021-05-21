using Common.Utility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace QuickSlotsMod
{
    public static class Mod
    {

        public static Config config;


        public static void Load()
        {
            config = Config.Instance;
            var harmony = new Harmony("subnautica.quickslotsmod.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main")
            {
                Main.Load();
            }
        }

    }
}
