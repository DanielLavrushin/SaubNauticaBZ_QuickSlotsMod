using HarmonyLib;
using QuickSlotsMod.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Logger = UnityEngine.Logger;

namespace QuickSlotsMod.Patches
{

    /***
     * The QuickSlots.slotNames variable was set in the Inventory_Awake_Patch, but it sometimes resets back to the hardcoded length of 5. When this
     * happens it causes index out of range exceptions when you select the added quick slots in game and eventually causes strange flickering and makes items unusable.
     * I couldn't determine what triggers this, but we can transpile the few methods that call the "slotNames" variable and load our own to avoid it.
     */
    [HarmonyPatch(typeof(QuickSlots))]
    [HarmonyPatch("Update")]
    class QuickSlots_Update_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldsfld)
                {
                    codes[i].operand = AccessTools.Field(typeof(NamesUtil), "slotNames");
                    break;
                }
            }
            return codes;
        }
    }
}
