using System;
using System.Collections.Generic;
using System.Linq;


namespace QuickSlotsMod.Utility
{
    class NamesUtil
    {
        public static string[] slotNames = Enumerable.Range(1, Mod.config.SlotCount).Select(n => "QuickSlot" + n).ToArray();
    }
}
