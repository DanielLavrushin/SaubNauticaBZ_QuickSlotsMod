using Common.Utility;
using QuickSlotsMod.Patches;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuickSlotsMod
{

    [Serializable]
    public class Config
    {
        /// <summary>
        /// Default config slots
        /// </summary>
        static Config _instance;
        private static Dictionary<string, string[]> defaultSlots = new Dictionary<string, string[]> {
            { "5", new[] {"6", "" } },
            { "6", new[] {"7", "" } },
            { "7", new[] {"8", "" } },
            { "8", new[] {"9", "" } },
            { "9", new[] {"0", "" } },
        };
        internal int SlotCount { get { return Player.quickSlotButtonsCount + (Slots?.Keys.Count ?? defaultSlots.Count); } }
        public Dictionary<string, string[]> Slots { get; set; }
        internal Dictionary<int, string> SlotLabels = new Dictionary<int, string>();
        internal event EventHandler OnConfigSave;
        internal Dictionary<int, uGUI_QuickSlots_Bindings> SlotBindings = new Dictionary<int, uGUI_QuickSlots_Bindings>();
        public static Config Instance
        {
            get
            {
                return _instance ?? Load();
            }
        }

        public void Save()
        {
            var n = new JSONObject();
            var h = n["Slots"].AsObject;
            foreach (var v in _instance.Slots)
            {
                var a = h[v.Key] = new JSONArray();
                h[v.Key].AsArray.Add(v.Key, v.Value?[0] ?? string.Empty);
                h[v.Key].AsArray.Add(v.Key, v.Value?[1] ?? string.Empty);
            }
            File.WriteAllText(GetModInfoPath(), n.ToString(JSONTextMode.Indent));
            OnConfigSave?.Invoke(this, null);
        }
        public static Config Load()
        {
            try
            {
                _instance = new Config() { Slots = defaultSlots };
                var modInfoObject = JSON.Parse(File.ReadAllText(GetModInfoPath()));

                if (modInfoObject["Slots"] != null)
                {
                    Instance.Slots = new Dictionary<string, string[]>();
                    foreach (var v in modInfoObject["Slots"])
                    {
                        Instance.Slots[v.Key] = new[] { v.Value.AsArray[0]?.Value, v.Value.AsArray[1]?.Value };
                    }
                }
            }
            catch
            {
                Logger.Warning("Unable to load settings.json. Creating default one...");
                _instance = new Config() { Slots = defaultSlots };
                _instance.Save();
            }


            return _instance;
        }

        private static string GetModInfoPath()
        {
            var file = new FileInfo(typeof(QuickSlotsMod).Assembly.Location).Directory.FullName + "\\settings.json";

            return file;
        }
    }
}
