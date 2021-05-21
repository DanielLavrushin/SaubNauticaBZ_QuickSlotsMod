using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace QuickSlotsMod.Patches
{
    internal class uGUI_QuickSlots_Binding : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICancelHandler
    {
        bool active;
        GameInput.BindingSet bindingSet;
        TextMeshProUGUI currentText;
        public uGUI_BindingData data;
        public int index = -1;
        string value;
        List<uGUI_QuickSlots_Bindings> bindingsList;
        GameInput.Device device = GameInput.Device.Keyboard;
        public void Initialize(int _index, GameInput.BindingSet _bindingSet)
        {
            bindingsList = Config.Instance.SlotBindings.Where(x => x.Key != _index).Select(x => x.Value).ToList();
            index = _index;
            bindingSet = _bindingSet;
            interactable = uGUI.interactable;
            data = uGUI_QuickSlots_ConfigTab.dataState;
            currentText = gameObject.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            value = Config.Instance.Slots[index.ToString()][(int)bindingSet];
            RefreshValue();
        }

        void Update()
        {
            if (active)
            {
                var primaryDevice = GameInput.GetPrimaryDevice();
                var text = GameInput.GetPressedInput(primaryDevice);
                bool flag = false;

                if (!string.IsNullOrEmpty(text))
                {
                    if (GameInput.IsBindable(text))
                    {
                        flag = true;
                        if (text == "Escape")
                        {
                            text = null;
                        }
                    }
                    else
                    {
                        text = null;
                    }
                    if (flag)
                    {
                        SetActive(false);
                        GameInput.ClearInput();
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                    if (text != null)
                    {
                        TryBind(text);
                    }
                }
            }
        }


        public void TryBind(string input, bool cancel = false)
        {
            var assignedValue = Config.Instance.Slots[index.ToString()][(int)bindingSet];
            if (cancel && !string.IsNullOrEmpty(assignedValue))
            {
                string arg = $"<color=#ADF8FFFF>{uGUI.GetDisplayTextForBinding(GameInput.GetInputName(input))}</color>";
                string text = string.Format(Language.main.Get("UnbindFormat"), arg, string.Format("<color=#ADF8FFFF>{0}</color>", Config.Instance.SlotLabels[index]));
                uGUI_QuickSlots_ConfigTab.optionsPanel.dialog.Show(text, delegate (int option)
                {
                    if (option == 1)
                    {
                        SetToConfig(null);

                    }
                }, Language.main.Get("No"), Language.main.Get("Yes"));
                return;
            }

            var listPool = Pool<ListPool<KeyValuePair<GameInput.Button, GameInput.BindingSet>>>.Get();
            var list = listPool.list;
            GameInput.Button sbnInput = GameInput.Button.None;
            Enum.TryParse<GameInput.Button>(input, true, out sbnInput);
            BindConflicts.GetConflicts(device, input, sbnInput, list);
            var slotsConflicts = new Dictionary<int, string[]>();
            var stringBuilder = new StringBuilder();
            string value = Language.main.Get("InputSeparator");
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(value);
                }
                stringBuilder.AppendFormat("<color=#ADF8FFFF>{0}</color>", Language.main.Get("Option" + list[i].Key));
            }

            foreach (var slot in Config.Instance.Slots.Where(x => x.Key != index.ToString()).ToList())
            {
                var slIndex = 0;
                foreach (var slValue in slot.Value)
                {
                    if (slValue == input)
                    {
                        stringBuilder.Append(value);
                        stringBuilder.AppendFormat("<color=#ADF8FFFF>{0}</color>", Config.Instance.SlotLabels[int.Parse(slot.Key)]);
                        slotsConflicts.Add(int.Parse(slot.Key), slot.Value);
                    }
                    slIndex++;
                }
            }

            if (slotsConflicts.Count + list.Count > 0)
            {
                string arg2 = $"<color=#ADF8FFFF>{uGUI.GetDisplayTextForBinding(GameInput.GetInputName(input))}</color>";
                string format = Language.main.GetFormat("BindConflictFormat", arg2, stringBuilder, string.Format("<color=#ADF8FFFF>{0}</color>", Config.Instance.SlotLabels[index]));
                uGUI_QuickSlots_ConfigTab.optionsPanel.dialog.Show(format, delegate (int option)
                {
                    if (option == 1)
                    {
                        foreach (var sc in slotsConflicts)
                        {
                            if (sc.Value[0] == input)
                            {
                                Config.Instance.SlotBindings[sc.Key].primaryBinding.SetToConfig(string.Empty);
                            }
                            if (sc.Value[1] == input)
                            {
                                Config.Instance.SlotBindings[sc.Key].secondaryBinding.SetToConfig(string.Empty);
                            }
                        }
                        SetToConfig(input);
                    }
                }, Language.main.Get("No"), Language.main.Get("Yes"));
            }
            else
            {
                SetToConfig(input);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
        public string CompileTimeCheck()
        {
            return null;
        }

        void SetActive(bool _active)
        {
            active = _active;
            RefreshValue();
        }

        private void UpdateState(bool forceDeselect = false)
        {
            if (active)
            {
                base.spriteState = data.stateActive;
            }
            else
            {
                base.spriteState = data.stateNormal;
            }
        }
        public void SetToConfig(string input)
        {
            Config.Instance.Slots[index.ToString()][(int)bindingSet] = input;
            value = input;
            RefreshValue();

            var alternateValue = Config.Instance.Slots[index.ToString()][bindingSet == GameInput.BindingSet.Primary ? 1 : 0];
            if (!string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(alternateValue))
            {
                if (bindingSet == GameInput.BindingSet.Primary && alternateValue == value)
                    Config.Instance.SlotBindings[index].secondaryBinding.SetToConfig(string.Empty);
                else if (bindingSet == GameInput.BindingSet.Secondary && alternateValue == value)
                    Config.Instance.SlotBindings[index].primaryBinding.SetToConfig(string.Empty);

            }
        }

        void RefreshValue()
        {
            if (active || string.IsNullOrWhiteSpace(value))
            {
                currentText.text = string.Empty;
            }
            else
            {
                currentText.text = uGUI.GetDisplayTextForBinding(GameInput.GetInputName(value));
            }
            UpdateState();
        }

        public void OnCancel(BaseEventData eventData)
        {
            SetActive(false);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            UpdateState();
            FPSInputModule.BubbleEvent(gameObject, eventData, ExecuteEvents.selectHandler);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            UpdateState(true);
            FPSInputModule.BubbleEvent(gameObject, eventData, ExecuteEvents.deselectHandler);
        }
        public void OnSubmit(BaseEventData eventData)
        {
            SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                TryBind(value, true);
            }
            else
            {
                SetActive(!active);
            }
        }
    }
}
