using HarmonyLib;
using System;
using UnityEngine;

namespace QuickSlotsMod.Patches
{

    [HarmonyPatch(typeof(uGUI_TabbedControlsPanel), "AddTab", new Type[] { typeof(string) })]
    class uGUI_TabbedControlsPanel_SelectItem_Patch
    {

        private static void Postfix(uGUI_TabbedControlsPanel __instance, string label, int __result)
        {
            if (label == "Keyboard" || label == "Controller")
            {
                if (__instance.gameObject.GetComponent<uGUI_QuickSlots_ConfigTab>() == null)
                {
                    var tabConfig = __instance.gameObject.AddComponent<uGUI_QuickSlots_ConfigTab>();
                    tabConfig.SetIndex(__result);
                }
            }
        }
    }
}
