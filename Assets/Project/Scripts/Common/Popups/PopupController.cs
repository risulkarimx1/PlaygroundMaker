using System;
using Zenject;

namespace Project.Scripts.Common.Popups
{
    public class PopupController
    {
        private readonly PopupView _popupView;

        [Inject]
        public PopupController(PopupView popupView)
        {
            _popupView = popupView;
        }

        public void ShowPopup<T>(string title, string description, Action<object> okayPressed,
            Action cancelPressed = null) where T : PopupBase
        {
            _popupView.Blocker.enabled = true;
            _popupView.ShowPopup<T>(title, description, okayPressed, cancelPressed);
        }

        public void HidePopup<T>() where T : PopupBase
        {
            if (typeof(T) == typeof(ConfirmationPopup))
            {
                _popupView.ConfirmationPopup.Hide();
            }
            else if (typeof(T) == typeof(InformationPopup))
            {
                _popupView.InformationPopup.Hide();
            }
            else if (typeof(T) == typeof(TextInputPopup))
            {
                _popupView.TextInputPopup.Hide();
            }

            _popupView.Blocker.enabled = false;
        }
    }
}