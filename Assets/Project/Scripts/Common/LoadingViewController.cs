using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Common
{
    public class LoadingViewController : MonoBehaviour
    {
        [SerializeField] private Image fill;

        private void Start()
        {
            fill.fillAmount = 0;
            Hide();
        }

        public void Show()
        {
            fill.fillAmount = 0;
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);


        public void UpdateFill(float fillAmount)
        {
            fill.fillAmount = fillAmount;
        }
    }
}