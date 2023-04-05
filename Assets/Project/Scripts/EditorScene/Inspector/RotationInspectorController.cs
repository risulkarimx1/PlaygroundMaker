using UniRx;

namespace Project.Scripts.EditorScene.Inspector
{
    public class RotationInspectorController : InspectorControllerBase 
    {
        protected override void InitializeSliderEvents()
        {
            InspectorView.RotationSlider.PointerClicked += OnRotationSliderPointerClicked;
            InspectorView.RotationSlider.PointerReleased += OnRotationSliderPointerReleased;
        }

        protected override void OnObjectSelected(EditorSceneSignals.SpawnableObjectSelectedSignal obj)
        {
            base.OnObjectSelected(obj);
            ChangeStream?.Dispose();
            InspectorView.InitializeRotationSliderValue(ObjectInInspector.transform.eulerAngles.y);

            ChangeStream = InspectorView.RotationSlider.onValueChanged.AsObservable()
                .Subscribe(y =>
                {
                    var rotationInEuler = ObjectInInspector.transform.eulerAngles;
                    rotationInEuler.y = y;
                    ObjectInInspector.transform.eulerAngles = rotationInEuler;
                }).AddTo(Disposable);
        }

        private void OnRotationSliderPointerClicked()
        {
            if (ObjectInInspector != null)
            {
                TransformCommand = TranslateCommandFactory.Create(ObjectInInspector);
            }
        }

        private void OnRotationSliderPointerReleased()
        {
            if (ObjectInInspector != null && TransformCommand != null)
            {
                CommandManager.ExecuteCommand(TransformCommand);
            }
        }

        protected override void DisposeSliderEvents()
        {
            InspectorView.RotationSlider.PointerClicked -= OnRotationSliderPointerClicked;
            InspectorView.RotationSlider.PointerReleased -= OnRotationSliderPointerReleased;
        }
    }
}