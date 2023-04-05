using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Common.Popups
{
    public class ConfirmationPopup : PopupBase
    {
        [SerializeField] private Button cancelButton;

        private Action _onCancelButtonClicked;

        protected override void Start()
        {
            base.Start();
            cancelButton.onClick.AsObservable().Subscribe(_ => _onCancelButtonClicked?.Invoke()).AddTo(this);
        }

        public void Show(string title, string description, Action<object> okayPressed, Action cancelPressed)
        {
            _onCancelButtonClicked = cancelPressed;
            base.Show(title, description, okayPressed);
        }
    }
}