using Common.Utility;
using QuickSlotsMod.Patches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace QuickSlotsMod
{
    public class Main : MonoBehaviour
    {

        public static void Load()
        {
            new GameObject("QuickSlotsMod_BZ.Main").AddComponent<Main>();
        }

        void Start()
        {

            Run();
        }

        private void Run()
        {
            uGUI_OptionsPanel uGUI_OptionsPanel = IngameMenu.main.GetComponentsInChildren<uGUI_OptionsPanel>(includeInactive: true)
                                                   .Where(x => x.name == "Options")
                                                   .FirstOrDefault();
            if ((bool)uGUI_OptionsPanel)
            {
                if (uGUI_MainMenuOptions_SelectItem_Patch.kbTabIndex > -1)
                {
                    if (uGUI_OptionsPanel.gameObject.GetComponent<uGUI_QuickSlots_ConfigTab>() == null)
                    {
                        var qsCongfig = uGUI_OptionsPanel.gameObject.AddComponent<uGUI_QuickSlots_ConfigTab>();
                        qsCongfig.SetIndex(uGUI_MainMenuOptions_SelectItem_Patch.kbTabIndex);
                    }
                }
            }
        }
    }
}
