using HarmonyLib;
using System;
using System.Reflection;
using QuickSlotsMod;
using Logger = UnityEngine.Logger;

namespace QuickSlotsMod.Patches
{



    [HarmonyPatch(typeof(Inventory))]
    [HarmonyPatch("Awake")]
    class Inventory_Awake_Patch
    {
        static void Postfix(Inventory __instance)
        {
            int slotCount = Mod.config.SlotCount;

            Player player = __instance.GetComponent<Player>();
            QuickSlots newQuickSlots = new QuickSlots(__instance.gameObject, __instance.toolSocket, __instance.cameraSocket, __instance, player.rightHandSlot, slotCount);

            var setQuickSlots = __instance.GetType().GetMethod("set_quickSlots", BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            setQuickSlots.Invoke(__instance, new object[] { newQuickSlots });

            Logger.Log("Quick Slots  Modified, new slot count: {0}", __instance.quickSlots.slotCount);
        }
    }
}
