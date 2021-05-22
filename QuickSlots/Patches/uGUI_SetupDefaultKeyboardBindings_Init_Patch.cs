using HarmonyLib;


namespace QuickSlotsMod.Patches
{
    [HarmonyPatch(typeof(GameInput), "SetupDefaultKeyboardBindings")]
    class uGUI_GameInput_SetupDefaultKeyboardBindings_Patch
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
}
