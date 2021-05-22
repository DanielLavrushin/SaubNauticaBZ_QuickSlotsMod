using HarmonyLib;
using System;

namespace QuickSlotsMod.Patches
{
    [HarmonyPatch(typeof(uGUI_TabbedControlsPanel), "AddBindingOption", new Type[] { typeof(int), typeof(string), typeof(GameInput.Device), typeof(GameInput.Button) })]
    class uGUI_TabbedControlsPanel_AddBindingOption_Patch
    {
        private static void Postfix(uGUI_TabbedControlsPanel __instance, int tabIndex, string label, GameInput.Device device, GameInput.Button button)
        {
            if (button == GameInput.Button.Slot5 && device == GameInput.GetPrimaryDevice())
            {
                var qsc = __instance.gameObject.GetComponent<uGUI_QuickSlots_ConfigTab>();
                if (qsc != null)
                {
                    qsc.SetControls();
                }
            }
        }
    }
}
