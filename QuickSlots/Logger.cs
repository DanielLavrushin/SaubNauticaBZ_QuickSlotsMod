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
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, message, null, QModManager.Utility.Logger.DebugLogsEnabled);
        }

        public static void Log(string message, params object[] args)
        {
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, string.Format(message, args), null, QModManager.Utility.Logger.DebugLogsEnabled);
        }
        internal static void Warning(string message)
        {
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Warn, message, null, false);
        }
        public static void Warning(string message, params object[] args)
        {
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Warn, string.Format(message, args), null, QModManager.Utility.Logger.DebugLogsEnabled);
        }
        internal static void Error(Exception ex)
        {
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Error, null, ex, false);
        }
    }
}
