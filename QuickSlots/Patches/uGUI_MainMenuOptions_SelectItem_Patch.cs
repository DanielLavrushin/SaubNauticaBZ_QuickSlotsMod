using HarmonyLib;
using System;
using UnityEngine;

namespace QuickSlotsMod.Patches
{
    [HarmonyPatch(typeof(uGUI_TabbedControlsPanel), "AddBindingOption", new Type[] { typeof(int), typeof(string), typeof(GameInput.Device), typeof(GameInput.Button) })]
    class uGUI_MainMenuOptions_AddBindingOption_Patch
    {
        private static void Postfix(uGUI_TabbedControlsPanel __instance, int tabIndex, string label, GameInput.Device device, GameInput.Button button)
        {
            if (button == GameInput.Button.Slot5 && device == GameInput.Device.Keyboard)
            {
                if (__instance.gameObject.GetComponent<uGUI_QuickSlots_ConfigTab>() != null)
                {
                    var qsc = __instance.gameObject.GetComponent<uGUI_QuickSlots_ConfigTab>();
                    qsc.SetControls();
                }
            }
        }
    }

    [HarmonyPatch(typeof(uGUI_TabbedControlsPanel), "AddTab", new Type[] { typeof(string) })]
    class uGUI_MainMenuOptions_SelectItem_Patch
    {
        public static int kbTabIndex = -1;

        private static void Postfix(uGUI_TabbedControlsPanel __instance, string label, int __result)
        {
            if (label == "Keyboard")
            {
                kbTabIndex = __result;
                if (__instance.gameObject.GetComponent<uGUI_QuickSlots_ConfigTab>() == null)
                {
                    var tabConfig = __instance.gameObject.AddComponent<uGUI_QuickSlots_ConfigTab>();
                    tabConfig.SetIndex(kbTabIndex);
                }
            }
        }
    }
}
