using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace QuickSlotsMod.Patches
{

    [HarmonyPatch(typeof(uGUI_QuickSlots), "Init")]
    class uGUI_QuickSlots_Init_Patch
    {
        private static void Postfix(uGUI_QuickSlots __instance)
        {
            InstantiateGameController(__instance);
        }

        private static void InstantiateGameController(uGUI_QuickSlots instance)
        {
            var controller = instance.gameObject.GetComponent<GameController>() ?? instance.gameObject.AddComponent<GameController>();
            if (controller != null)
            {
                controller.AddHotkeyLabels(instance);
            }
        }
    }
}
