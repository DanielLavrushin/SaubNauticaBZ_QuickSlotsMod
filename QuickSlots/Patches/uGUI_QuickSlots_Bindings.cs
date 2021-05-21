using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QuickSlotsMod.Patches
{
    internal class uGUI_QuickSlots_Bindings : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler, ICompileTimeCheckable
    {
        public int index;
        internal uGUI_QuickSlots_Binding primaryBinding;
        internal uGUI_QuickSlots_Binding secondaryBinding;
        public GameObject selectionBackground;
        private FMODAsset hoverSound;
        void Start()
        {
            primaryBinding = gameObject.transform.Find("Primary Binding").gameObject.AddComponent<uGUI_QuickSlots_Binding>();
            secondaryBinding = gameObject.transform.Find("Secondary Binding").gameObject.AddComponent<uGUI_QuickSlots_Binding>();
            primaryBinding.Initialize(index, GameInput.BindingSet.Primary);
            secondaryBinding.Initialize(index, GameInput.BindingSet.Secondary);

        }


        public void Setup(int _index, GameObject background, FMODAsset sound)
        {
            index = _index;
            selectionBackground = background;
            hoverSound = sound;
        }

        public void OnSelect(BaseEventData eventData)
        {
            eventData.Use();
            SetSelected(selected: true);
            if (hoverSound != null)
            {
                RuntimeManager.PlayOneShot(hoverSound.path);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            eventData.Use();
            SetSelected(selected: false);
        }
        private void SetSelected(bool selected)
        {
            selectionBackground.SetActive(selected && GameInput.GetPrimaryDevice() == GameInput.Device.Controller);
        }
        public string CompileTimeCheck()
        {
            return null;
        }
    }
}
