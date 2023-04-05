using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Common.Popups
{
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI titleText;
        [SerializeField] protected TextMeshProUGUI descriptionText;
        [SerializeField] protected Button okayButton;

        protected Action<object> OnOkayButtonClicked;

        protected virtual void Start()
        {
            okayButton.onClick.AsObservable().Subscribe(_ => OnOkayButtonClicked(null)).AddTo(this);
            Hide();
        }

        public void Show(string title, string description, Action<object> okayPressed)
        {
            titleText.text = title;
            descriptionText.text = description;
            OnOkayButtonClicked = okayPressed;
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}