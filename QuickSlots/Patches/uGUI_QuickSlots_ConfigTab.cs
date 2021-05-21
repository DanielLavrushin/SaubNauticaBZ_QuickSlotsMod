using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;


namespace QuickSlotsMod.Patches
{
    internal class uGUI_QuickSlots_ConfigTab : MonoBehaviour
    {
        internal static bool isOpened;
        public static uGUI_OptionsPanel optionsPanel;
        public static uGUI_BindingData dataState;
        internal static IDictionary<string, KeyCode> inputsMapping = new Dictionary<string, KeyCode>();
        int index = -1;
        List<uGUI_Bindings> bindings = new List<uGUI_Bindings>();



        void Awake()
        {
            var inputs = (IList)(typeof(GameInput).GetField("inputs", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null));
            foreach (var kk in inputs)
            {
                var key = kk.GetType().GetField("name").GetValue(kk).ToString();
                var value = (KeyCode)Enum.Parse(typeof(KeyCode), kk.GetType().GetField("keyCode").GetValue(kk).ToString());
                if (!inputsMapping.ContainsKey(key))
                {
                    inputsMapping.Add(key, value);
                }
            }

            optionsPanel = GetComponent<uGUI_OptionsPanel>();
            bindings = Traverse.Create(optionsPanel).Field("bindings").GetValue<List<uGUI_Bindings>>();
        }
        public void OnEnable()
        {
            optionsPanel = GetComponent<uGUI_OptionsPanel>();
            bindings = Traverse.Create(optionsPanel).Field("bindings").GetValue<List<uGUI_Bindings>>();
            isOpened = true;
        }

        private void OnDisable()
        {
            Config.Instance.Save();
            isOpened = false;
        }

        public void SetControls()
        {
            var labelTemplate = Language.main.Get("OptionSlot1").Split(' ');
            var translatedLabel = labelTemplate.Length == 2 ? labelTemplate[0] : "Slot";
            Config.Instance.SlotLabels = new Dictionary<int, string>();
            Config.Instance.SlotBindings = new Dictionary<int, uGUI_QuickSlots_Bindings>();
            if (index > -1)
            {
                for (int i = Player.quickSlotButtonsCount; i < Mod.config.SlotCount; i++)
                {
                    var label = $"{translatedLabel} {(i + 1)}";
                    if (!Config.Instance.SlotLabels.ContainsKey(i)) Config.Instance.SlotLabels.Add(i, label);
                    var gameObject = optionsPanel.AddItem(index, optionsPanel.bindingOptionPrefab);

                    var componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
                    if (componentInChildren != null)
                    {
                        gameObject.name = "OptionSlot" + i;
                        componentInChildren.text = label;
                        componentInChildren.color = uGUI.currentColor;
                    }

                    var originalBindingsComponent = gameObject.GetComponentInChildren<uGUI_Bindings>();
                    var originalBindingComponents = gameObject.GetComponentsInChildren<uGUI_Binding>().Select(x => x.gameObject).ToList();

                    foreach (var ob in gameObject.GetComponentsInChildren<uGUI_Binding>())
                    {
                        dataState = ob.data;
                        Destroy(ob);
                    }


                    var bg = originalBindingsComponent.selectionBackground;
                    bindings.Remove(originalBindingsComponent);
                    Destroy(originalBindingsComponent);
                    var qsBinds = gameObject.transform.Find("Bindings").gameObject.AddComponent<uGUI_QuickSlots_Bindings>();
                    Config.Instance.SlotBindings.Add(i, qsBinds);
                    qsBinds.Setup(i, bg, null);
                }
            }
        }

        internal void SetIndex(int _index)
        {
            index = _index;
        }
    }
}
