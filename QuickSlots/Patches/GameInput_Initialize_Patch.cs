//using HarmonyLib;
//using UnityEngine;

//namespace QuickSlotsMod.Patches
//{
//    [HarmonyPatch(typeof(GameInput), "Awake")]
//    class GameInput_Awake_Patch
//    {
//        static void Postfix(GameInput __instance)
//        {
//            //     Logger.Log("uGUI_GameInput_Awake_Patch");
//        }
//    }

//    [HarmonyPatch(typeof(GameInput), "Initialize")]
//    class GameInput_Initialize_Patch
//    {
//        static void Postfix(GameInput __instance)
//        {
//            //        Logger.Log("uGUI_GameInput_Initialize_Patch");

//            //       var AddKeyInput = typeof(GameInput).GetMethod("AddKeyInput");

//            //   AddKeyInput.Invoke(__instance, new object[] { ",", KeyCode.Comma.ToString(), GameInput.Device.Keyboard });

//        }
//    }
//}
