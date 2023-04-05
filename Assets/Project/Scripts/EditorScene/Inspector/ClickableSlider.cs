using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Scripts.EditorScene.Inspector
{
    public class ClickableSlider : Slider, IPointerDownHandler, IPointerUpHandler
    {
        public Action PointerClicked { get; set; }
        public Action PointerReleased { get; set; }

        public override void OnPointerDown(PointerEventData eventData)
        {
            PointerClicked?.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            PointerReleased?.Invoke();
        }
    }
}