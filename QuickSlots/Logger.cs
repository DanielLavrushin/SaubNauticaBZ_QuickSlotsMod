using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using QModManager.Utility;

namespace QuickSlotsMod
{
    public static class Logger
    {
        public static void Log(string message)
        {
            if (QModManager.Utility.Logger.DebugLogsEnabled)
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, "[QuickSlotsMod] " + message, null, true);
            UnityEngine.Debug.Log("[QuickSlotsMod] " + message);
        }

        public static void Log(string format, params object[] args)
        {
            if (QModManager.Utility.Logger.DebugLogsEnabled)
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, "[QuickSlotsMod] " + string.Format(format, args), null, true);
            UnityEngine.Debug.Log("[QuickSlotsMod] " + string.Format(format, args));
        }
    }
}
