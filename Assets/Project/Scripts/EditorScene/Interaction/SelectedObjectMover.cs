using System;
using Project.Scripts.Common;
using Project.Scripts.EditorScene.Command;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Interaction
{
    public class SelectedObjectMover : IInitializable
    {
        [Inject] private Camera _camera;
        [Inject] private SignalBus _signalBus;
        [Inject] private CompositeDisposable _disposable;
        [Inject(Id = nameof(MouseInputHandler))] private IInputHandler _inputHandler;
        [Inject] private ICommandManager _commandManager;
        [Inject] private TranslateCommandFactory _translateCommandFactory;

        private IDisposable _selectionReleaseStream;
        private IDisposable _touchPositionStream;
        public void Initialize()
        {
            _signalBus.GetStream<EditorSceneSignals.SpawnableObjectSelectedSignal>().Subscribe(signal =>
            {
                _signalBus.Fire( new EditorSceneSignals.SpawnUiVisibilitySignal()
                {
                    IsVisible = false
                });
                DisposeExistingStreams();
                
                var selectedObject = signal.SpawnableObject;
                var translateCommand = _translateCommandFactory.Create(selectedObject);
                
                var initialOffset = Vector3.zero;

                if (MouseToGroundPosition(_inputHandler.TouchPosition, out var offset))
                {
                    initialOffset = selectedObject.transform.position - offset;
                }

                _touchPositionStream = _inputHandler.TapHoldStream.Subscribe(_ =>
                {
                    var touchPosition = _inputHandler.TouchPosition;
                    
                    if (MouseToGroundPosition(touchPosition, out var targetPosition))
                    {
                        selectedObject.transform.position = targetPosition + initialOffset;
                    }
                }).AddTo(_disposable);

                _selectionReleaseStream =_signalBus.GetStream<EditorSceneSignals.SelectionReleaseSignal>().Subscribe(_ =>
                {

                    _commandManager.ExecuteCommand(translateCommand);
                    
                    DisposeExistingStreams();
                    
                    _signalBus.Fire( new EditorSceneSignals.SpawnUiVisibilitySignal()
                    {
                        IsVisible = true
                    });
                }).AddTo(_disposable);
            }).AddTo(_disposable);
        }

        private void DisposeExistingStreams()
        {
            _touchPositionStream?.Dispose();
            _selectionReleaseStream?.Dispose();
        }

        private bool MouseToGroundPosition(Vector3 touchPosition, out Vector3 targetPosition)
        {
            var ray = _camera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out var hit, 1000, Constants.GroundLayer))
            {
                targetPosition = hit.point;
                targetPosition.y = 0;
                return true;
            }

            targetPosition = Vector3.negativeInfinity;
            return false;
        }
    }
}