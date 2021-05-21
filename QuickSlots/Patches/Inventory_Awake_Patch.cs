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
            var slotCount = Mod.config.SlotCount;
            var player = __instance.GetComponent<Player>();
            var newQuickSlots = new QuickSlots(__instance.gameObject, __instance.toolSocket, __instance.cameraSocket, __instance, player.rightHandSlot, slotCount);
            var setQuickSlots = __instance.GetType().GetMethod("set_quickSlots", BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            setQuickSlots.Invoke(__instance, new object[] { newQuickSlots });
        }
    }
}
