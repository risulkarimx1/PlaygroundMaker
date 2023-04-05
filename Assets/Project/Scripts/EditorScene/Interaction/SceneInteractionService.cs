using System.Collections.Generic;
using Project.Scripts.Common;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Project.Scripts.EditorScene.Interaction
{
    public class SceneInteractionService : IInitializable
    {
        [Inject] private Camera _mainCamera;
        [Inject] private SignalBus _signalBus;
        [Inject(Id = nameof(MouseInputHandler))] private IInputHandler _inputHandler;
        [Inject] private CompositeDisposable _disposable;

        public void Initialize()
        {
            _inputHandler.TapDownStream.Subscribe(_ =>
            {
                var touchPosition = _inputHandler.TouchPosition;

                if (IsPointerOverUIElement(touchPosition))
                {
                    _signalBus.Fire<EditorSceneSignals.TouchedOnUiSignal>();
                    return;
                }
                
                var ray = _mainCamera.ScreenPointToRay(touchPosition);

                if (Physics.Raycast(ray, out var hit, Constants.SceneScale, Constants.SpawnableLayer))
                {
                    _signalBus.Fire(new EditorSceneSignals.SpawnableObjectSelectedSignal()
                    {
                        SpawnableObject = hit.transform.gameObject
                    });
                }
                else
                {
                    _signalBus.Fire(new EditorSceneSignals.GroundSelectedSignal
                    {
                        MousePosition = touchPosition
                    });
                    Debug.LogWarning("Ground selected");
                }
            }).AddTo(_disposable);

            _inputHandler.TapReleaseStream.Subscribe(_ =>
            {
                _signalBus.Fire<EditorSceneSignals.SelectionReleaseSignal>();
            }).AddTo(_disposable);
        }
        
        private bool IsPointerOverUIElement(Vector2 screenPosition)
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
    }
}