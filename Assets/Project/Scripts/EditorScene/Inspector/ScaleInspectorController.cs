using UniRx;
using UnityEngine;

namespace Project.Scripts.EditorScene.Inspector
{
    public class ScaleInspectorController : InspectorControllerBase
    {
        protected override void InitializeSliderEvents()
        {
            InspectorView.ScaleSlider.PointerClicked += OnScaleSliderPointerClicked;
            InspectorView.ScaleSlider.PointerReleased += OnScaleSliderPointerReleased;
        }

        protected override void OnObjectSelected(EditorSceneSignals.SpawnableObjectSelectedSignal obj)
        {
            base.OnObjectSelected(obj);
            InspectorView.InitializeScaleSliderValue(ObjectInInspector.transform.localScale.x);

            ChangeStream?.Dispose();

            ChangeStream = InspectorView.ScaleSlider.onValueChanged.AsObservable()
                .Subscribe(scale =>
                {
                    var newScale = new Vector3(scale, scale, scale);
                    ObjectInInspector.transform.localScale = newScale;
                }).AddTo(Disposable);
        }

        private void OnScaleSliderPointerClicked()
        {
            if (ObjectInInspector != null)
            {
                TransformCommand = TranslateCommandFactory.Create(ObjectInInspector);
            }
        }

        private void OnScaleSliderPointerReleased()
        {
            if (ObjectInInspector != null && TransformCommand != null)
            {
                CommandManager.ExecuteCommand(TransformCommand);
            }
        }

        protected override void DisposeSliderEvents()
        {
            InspectorView.ScaleSlider.PointerClicked -= OnScaleSliderPointerClicked;
            InspectorView.ScaleSlider.PointerReleased -= OnScaleSliderPointerReleased;
        }
    }
}