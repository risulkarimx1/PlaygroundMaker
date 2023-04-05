using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Common.Popups
{
    public class TextInputPopup : PopupBase
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private TMP_InputField textInput;

        private Action _onCancelButtonClicked;


        protected override void Start()
        {
            okayButton.onClick.AsObservable().Subscribe(_ => OnOkayButtonClicked(textInput.text)).AddTo(this);
            cancelButton.onClick.AsObservable().Subscribe(_ => _onCancelButtonClicked?.Invoke()).AddTo(this);
            Hide();
        }

        public void Show(string title, string description, Action<object> okayPressed, Action cancelPressed)
        {
            _onCancelButtonClicked = cancelPressed;
            base.Show(title, description, okayPressed);
        }
    }
}