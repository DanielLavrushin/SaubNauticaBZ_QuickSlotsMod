using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QuickSlotsMod
{
    public class GameController : MonoBehaviour
    {
        private static FieldInfo uGUI_QuickSlots_icons = typeof(uGUI_QuickSlots).GetField("icons", BindingFlags.NonPublic | BindingFlags.Instance);

        private uGUI_QuickSlots quickSlots;
        private bool tryAddLabels;

        private void Awake()
        {
            Logger.Log("GameController Added");
            quickSlots = GetComponent<uGUI_QuickSlots>();
        }

        private void OnDestroy()
        {
            Logger.Log("GameController Destroyed");
        }

        private void Update()
        {
            if (uGUI.main == null || uGUI.main.loading.IsLoading)
            {
                return;
            }

            if (CanSetQuickSlots())
            {
                for (int i = Player.quickSlotButtonsCount; i < Mod.config.SlotCount; ++i)
                {
                    if (Mod.GetKeyDownForSlot(i))
                    {
                        SelectQuickSlot(i);
                    }
                }
            }

            if (tryAddLabels)
            {
                AddHotkeyLabels(quickSlots);
            }
        }

        private bool CanSetQuickSlots()
        {
            if (Inventory.main == null)
            {
                return false;
            }

            bool isIntroActive = IntroVignette.isIntroActive;
            if (isIntroActive)
            {
                return false;
            }

            Player player = Player.main;
            return player != null && player.GetMode() != Player.Mode.Piloting && player.GetCanItemBeUsed();
        }

        private void SelectQuickSlot(int slotID)
        {
            Inventory.main.quickSlots.SlotKeyDown(slotID);
        }

        public void AddHotkeyLabels(uGUI_QuickSlots instance)
        {
            if (instance == null || !Mod.config.ShowInputText)
            {
                tryAddLabels = true;
                return;
            }

            uGUI_ItemIcon[] icons = (uGUI_ItemIcon[])uGUI_QuickSlots_icons.GetValue(instance);
            if (icons == null || icons.Length == 0)
            {
                tryAddLabels = true;
                return;
            }

            var textPrefab = GetTextPrefab();
            if (textPrefab == null)
            {
                tryAddLabels = true;
                return;
            }

            for (int i = 0; i < icons.Length; ++i)
            {
                uGUI_ItemIcon icon = icons[i];
                var text = CreateNewText(textPrefab, icon.transform, Mod.GetInputForSlotFormatted(i), i);
            }
            tryAddLabels = false;
        }

        private static TextMeshProUGUI GetTextPrefab()
        {
            var prefabObject = GameObject.FindObjectOfType<HandReticle>();
            if (prefabObject == null)
            {
                return null;
            }

            var prefab = prefabObject?.compTextHandSubscript;
            if (prefab == null)
            {
                return null;
            }


            return prefab;
        }

        private static TextMeshProUGUI CreateNewText(TextMeshProUGUI prefab, Transform parent, string newText, int index = -1)
        {
            TextMeshProUGUI text = GameObject.Instantiate(prefab);
            text.gameObject.layer = parent.gameObject.layer;
            text.gameObject.name = "QuickSlotText" + (index >= 0 ? index.ToString() : "");
            text.transform.SetParent(parent, false);
            text.transform.localScale = new Vector3(1, 1, 1);
            text.gameObject.SetActive(true);
            text.enabled = true;
            text.text = newText;
            text.fontSize = 17;
            RectTransformExtensions.SetParams(text.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), parent);
            text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
            text.rectTransform.anchoredPosition = new Vector3(0, -36);
            text.alignment = TextAlignmentOptions.Midline;
            text.raycastTarget = false;

            return text;
        }
    }
}
