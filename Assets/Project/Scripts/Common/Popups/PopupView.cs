using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Common.Popups
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private Image blocker;
        [SerializeField] private InformationPopup informationPopup;
        [SerializeField] private ConfirmationPopup confirmationPopup;
        [SerializeField] private TextInputPopup textInputPopup;

        public InformationPopup InformationPopup => informationPopup;
        public ConfirmationPopup ConfirmationPopup => confirmationPopup;
        public TextInputPopup TextInputPopup => textInputPopup;

        public Image Blocker => blocker;

        private void Start()
        {
            blocker.enabled = false;
        }

        public void ShowPopup<T>(string title, string description, Action<object> okayPressed,
            Action cancelPressed = null) where T : PopupBase
        {
            if (typeof(T) == typeof(ConfirmationPopup))
            {
                confirmationPopup.Show(title, description, okayPressed, cancelPressed);
            }
            else if (typeof(T) == typeof(TextInputPopup))
            {
                textInputPopup.Show(title, description, okayPressed, cancelPressed);
            }
            else if (typeof(T) == typeof(InformationPopup))
            {
                informationPopup.Show(title, description, okayPressed);
            }
        }
    }
}