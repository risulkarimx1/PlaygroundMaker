using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EditorScene.Inspector
{
    public class InspectorView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject inspectorPanel;
        [SerializeField] private Button deleteObjectButton;
        [SerializeField] private ClickableSlider rotationSlider;
        [SerializeField] private ClickableSlider scaleSlider;

        public ClickableSlider RotationSlider => rotationSlider;
        public ClickableSlider ScaleSlider => scaleSlider;

        public Button DeleteObjectButton => deleteObjectButton;

        public TextMeshProUGUI NameText => nameText;

        public void InitializeRotationSliderValue(float rotation)
        {
            rotationSlider.value = rotation;
        }
        public void InitializeScaleSliderValue(float rotation)
        {
            scaleSlider.value = rotation;
        }

        public void SetVisibility(bool state, string objectName = null)
        {
            if (state && objectName!= null)
            {
                nameText.text = objectName;
            }
            
            inspectorPanel.SetActive(state);
        }
    }
}