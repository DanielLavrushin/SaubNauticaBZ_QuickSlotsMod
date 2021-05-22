using HarmonyLib;
using QuickSlotsMod.Utility;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace QuickSlotsMod.Patches
{
    [HarmonyPatch(typeof(QuickSlots))]
    [HarmonyPatch("DeselectInternal")]
    class QuickSlots_DeselectInternal_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldsfld && codes[i + 1].opcode == OpCodes.Ldloc_0)
                {
                    codes[i].operand = AccessTools.Field(typeof(NamesUtil), "slotNames");
                    break;
                }
            }
            return codes;
        }
    }
}
