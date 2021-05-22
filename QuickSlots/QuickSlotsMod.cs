using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using QModManager.API;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using QuickSlotsMod.Patches;
using System.Text.RegularExpressions;

namespace QuickSlotsMod
{
    public class QuickSlotsMod : MonoBehaviour
    {
        private static FieldInfo uGUI_QuickSlots_icons = typeof(uGUI_QuickSlots).GetField("icons", BindingFlags.NonPublic | BindingFlags.Instance);


        private uGUI_QuickSlots quickSlots;
        Dictionary<int, TextMeshProUGUI> texts;
        GameInput.Device primaryDevice;
        string pressedKey = string.Empty;
        private void Awake()
        {
            Config.Instance.OnConfigSave += Instance_OnConfigSave;
            texts = new Dictionary<int, TextMeshProUGUI>();
            primaryDevice = GameInput.GetPrimaryDevice();
            quickSlots = GetComponent<uGUI_QuickSlots>();
            AddHotkeyLabels(quickSlots);
        }

        private void Instance_OnConfigSave(object sender, EventArgs e)
        {
            AddHotkeyLabels(quickSlots);
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            if (uGUI.main == null || uGUI.main.loading.IsLoading || !IngameMenu.main.enabled)
            {
                return;
            }

            if (Input.anyKeyDown && CanSetQuickSlots())
            {
                for (int i = Player.quickSlotButtonsCount; i < Mod.config.SlotCount; i++)
                {
                    if (GetKeyDownForSlot(i))
                    {
                        SelectQuickSlot(i);
                    }
                }
            }
        }


        public bool GetKeyDownForSlot(int slotID)
        {
            var primarySlot = GetInputForSlot(slotID);
            var secondarySlot = GetInputForSlot(slotID, GameInput.BindingSet.Secondary);
            return slotID >= Player.quickSlotButtonsCount && slotID < Mod.config.SlotCount &&
               ((!string.IsNullOrWhiteSpace(primarySlot) && Input.GetKeyDown(uGUI_QuickSlots_ConfigTab.inputsMapping[primarySlot])) || (!string.IsNullOrWhiteSpace(secondarySlot) && Input.GetKeyDown(uGUI_QuickSlots_ConfigTab.inputsMapping[secondarySlot])));
        }
        private void SelectQuickSlot(int slotID)
        {
            Inventory.main.quickSlots.SlotKeyDown(slotID);
            pressedKey = string.Empty;
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
        public void AddHotkeyLabels(uGUI_QuickSlots instance)
        {
            uGUI_ItemIcon[] icons = (uGUI_ItemIcon[])uGUI_QuickSlots_icons.GetValue(instance);
            if (icons == null || icons.Length == 0)
            {
                return;
            }

            var textPrefab = GetTextPrefab();
            if (textPrefab == null)
            {
                return;
            }

            for (int i = 0; i < icons.Length; ++i)
            {
                var icon = icons[i];

                var prevtext = icon.transform.GetComponentInChildren<TextMeshProUGUI>();
                if (prevtext != null)
                    prevtext.text = GetInputForSlotFormatted(i);
                else
                    CreateNewText(textPrefab, icon.transform, GetInputForSlotFormatted(i), i);
            }
        }
        public static string GetInputForSlot(int slotID, GameInput.BindingSet binding = GameInput.BindingSet.Primary)
        {
            if (slotID < Player.quickSlotButtonsCount)
            {
                string inputName = GameInput.GetBindingName(GameInput.Button.Slot1 + slotID, binding);
                string input = LanguageCache.GetButtonFormat("{0}", GameInput.Button.Slot1 + slotID);
                return string.IsNullOrEmpty(inputName) ? string.Empty : input;
            }
            if (slotID < 0 || slotID >= Mod.config.SlotCount)
            {
                return string.Empty;
            }
            var key = Mod.config.Slots[slotID.ToString()][(int)binding];
            return key;
        }

        public static string GetInputForSlotFormatted(int slotID)
        {
            var key = string.Empty;
            if (slotID < Player.quickSlotButtonsCount)
            {
                key = GameInput.GetBindingName(GameInput.Button.Slot1 + slotID, GameInput.BindingSet.Primary) ?? GameInput.GetBindingName(GameInput.Button.Slot1 + slotID, GameInput.BindingSet.Secondary) ?? string.Empty;
            }
            else
            {
                key = GetInputForSlot(slotID);
                if (string.IsNullOrEmpty(key))
                    key = GetInputForSlot(slotID, GameInput.BindingSet.Secondary);
            }
            var r = Regex.Match(key, @">(.*?)<", RegexOptions.Singleline);
            key = r.Success ? r.Groups[1].Value : key;
            key = TryGetDefaultLabel(key);

            key = slotID >= Player.quickSlotButtonsCount && Mod.config.Slots[slotID.ToString()].Length > 2 && !string.IsNullOrWhiteSpace(Mod.config.Slots[slotID.ToString()][2]) ? Mod.config.Slots[slotID.ToString()][2] : key;
            return $"<color=#ADF8FFFF>{key}</color>";
        }

        internal static string TryGetDefaultLabel(string key)
        {
            KeyCode keyCode;
            if (Enum.TryParse<KeyCode>(key, out keyCode))
            {
                key = keyCode switch
                {
                    KeyCode.Escape => "Esc",
                    KeyCode.PageUp => "pUp",
                    KeyCode.PageDown => "pDwn",
                    KeyCode.Comma => ",",
                    KeyCode.Colon => ":",
                    KeyCode.Semicolon => ";",
                    KeyCode.Quote => "'",
                    KeyCode.DoubleQuote => "\"",
                    KeyCode.KeypadEnter => "Ent",
                    KeyCode.Return => "Ent",
                    KeyCode.Delete => "Del",
                    KeyCode.Insert => "Ins",
                    KeyCode.Numlock => "NmLck",
                    KeyCode.ScrollLock => "SrLck",
                    KeyCode.Print => "Prnt",
                    KeyCode.Keypad0 => "0",
                    KeyCode.Keypad1 => "1",
                    KeyCode.Keypad2 => "2",
                    KeyCode.Keypad3 => "3",
                    KeyCode.Keypad4 => "4",
                    KeyCode.Keypad5 => "5",
                    KeyCode.Keypad6 => "6",
                    KeyCode.Keypad7 => "7",
                    KeyCode.Keypad8 => "8",
                    KeyCode.Keypad9 => "9",
                    KeyCode.KeypadDivide => "/",
                    KeyCode.KeypadEquals => "=",
                    KeyCode.KeypadMinus => "-",
                    KeyCode.Minus => "-",
                    KeyCode.Plus => "+",
                    KeyCode.KeypadPlus => "+",
                    KeyCode.KeypadMultiply => "*",
                    KeyCode.Asterisk => "*",
                    KeyCode.Question => "?",
                    KeyCode.DownArrow => "Dwn",
                    KeyCode.UpArrow => "Up",
                    KeyCode.LeftArrow => "Left",
                    KeyCode.RightArrow => "Right",
                    KeyCode.LeftAlt => "Alt",
                    KeyCode.RightAlt => "Alt",
                    KeyCode.LeftControl => "Ctrl",
                    KeyCode.RightControl => "Ctrl",
                    KeyCode.LeftShift => "Shft",
                    KeyCode.RightShift => "Shft",
                    KeyCode.LeftWindows => "Wnd",
                    KeyCode.KeypadPeriod => ".",
                    KeyCode.Period => ".",
                    KeyCode.Underscore => "_",

                    _ => key

                };
            }
            return key;
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
