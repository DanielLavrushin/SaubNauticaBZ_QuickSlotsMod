using HarmonyLib;
using System.Collections;
using System.Reflection;


namespace QuickSlotsMod.Patches
{
    [HarmonyPatch(typeof(GameInput), "SetupDefaultKeyboardBindings")]
    class uGUI_SetupDefaultKeyboardBindings_Init_Patch
    {
        private static void Postfix(GameInput __instance)
        {
            foreach (var slot in Config.Instance.SlotBindings)
            {
                var slotIndex = slot.Value.primaryBinding.index + 1;
                slot.Value.secondaryBinding.SetToConfig(string.Empty);
                if (slotIndex > Player.quickSlotButtonsCount && slotIndex <= 9)
                {
                    slot.Value.primaryBinding.SetToConfig(slotIndex.ToString());
                }
            }
        }
    }
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
