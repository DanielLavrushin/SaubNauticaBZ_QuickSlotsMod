using HarmonyLib;
using System.Collections;
using System.Reflection;

namespace QuickSlotsMod.Patches
{
    [HarmonyPatch(typeof(uGUI_QuickSlots), "Init")]
    class uGUI_QuickSlots_Init_Patch
    {
        private static void Postfix(uGUI_QuickSlots __instance)
        {
            InstantiateQuickSlotsMod(__instance);
        }

        private static void InstantiateQuickSlotsMod(uGUI_QuickSlots instance)
        {
            var controller = instance.gameObject.GetComponent<QuickSlotsMod>() ?? instance.gameObject.AddComponent<QuickSlotsMod>();
            if (controller != null)
            {
                controller.AddHotkeyLabels(instance);
            }
        }
    }
}
