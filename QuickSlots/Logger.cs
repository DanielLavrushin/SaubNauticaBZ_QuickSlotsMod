using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QuickSlotsMod
{
    public static class Logger
    {
        public static void Log(string message)
        {
            UnityEngine.Debug.Log("[QuickSlotsMod] " + message);
        }

        public static void Log(string format, params object[] args)
        {
            UnityEngine.Debug.Log("[QuickSlotsMod] " + string.Format(format, args));
        }
    }
}
