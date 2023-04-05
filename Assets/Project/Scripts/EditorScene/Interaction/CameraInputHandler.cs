using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Interaction
{
    public class CameraInputHandler : BaseInputHandler, IInitializable, IDisposable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private CompositeDisposable _disposable;
        [Inject] private EditorSceneCameraConfig _config;

        public IObservable<Vector2> OnMovementInputReceived => _onMovementInputReceived;
        private readonly Subject<Vector2> _onMovementInputReceived = new Subject<Vector2>();

        public IObservable<float> OnZoomInputReceived => _onZoomInputReceived;
        private readonly Subject<float> _onZoomInputReceived = new Subject<float>();

        public IObservable<float> OnRotationInputReceived => _onRotationInputReceived;
        private readonly Subject<float> _onRotationInputReceived = new Subject<float>();

        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;
        private Vector2 _inputDirection;

        private IDisposable _updateStream;

        public override Vector3 TouchPosition => Input.mousePosition;

        public void Initialize()
        {
            _signalBus.GetStream<EditorSceneSignals.GroundSelectedSignal>()
                .Subscribe(_ => { InitializeUpdateStream(); }).AddTo(_disposable);

            _signalBus.GetStream<EditorSceneSignals.SpawnableObjectSelectedSignal>().Subscribe(_ =>
            {
                DisposeUpdateStream();
            }).AddTo(_disposable);
            _signalBus.GetStream<EditorSceneSignals.TouchedOnUiSignal>().Subscribe(_ => { DisposeUpdateStream(); })
                .AddTo(_disposable);
        }

        private void DisposeUpdateStream()
        {
            _updateStream?.Dispose();
            _updateStream = null;
        }

        private void InitializeUpdateStream()
        {
            if (_updateStream != null) return;

            _updateStream = Observable.EveryUpdate().Subscribe(_ =>
            {
                if (Application.isMobilePlatform)
                {
                    HandleMobileInput();
                }
                else
                {
                    HandleEditorInput();
                }
            }).AddTo(_disposable);
        }

        private void HandleMobileInput()
        {
            switch (Input.touchCount)
            {
                case 1:
                    HandleSingleTouch();
                    break;
                case 2:
                    HandlePinchZoom();
                    HandleTwoFingerRotation();
                    break;
            }
        }

        private void HandleSingleTouch()
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                _currentTouchPosition = touch.position;
                _inputDirection = (_currentTouchPosition - _startTouchPosition).normalized;
                _onMovementInputReceived.OnNext(_inputDirection);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _inputDirection = Vector2.zero;
            }
        }

        private void HandlePinchZoom()
        {
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);

            var prevTouchDelta = touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition);
            var currentTouchDelta = touch0.position - touch1.position;

            var prevTouchDistance = prevTouchDelta.magnitude;
            var currentTouchDistance = currentTouchDelta.magnitude;

            var pinchZoomAmount = (prevTouchDistance - currentTouchDistance) * Time.deltaTime * _config.PinchZoomSpeed;

            _onZoomInputReceived.OnNext(pinchZoomAmount);
        }

        private void HandleTwoFingerRotation()
        {
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);

            if (touch0.phase != TouchPhase.Moved || touch1.phase != TouchPhase.Moved) return;
            var touch0StartPosition = touch0.position - touch0.deltaPosition;
            var touch1StartPosition = touch1.position - touch1.deltaPosition;
            var midpointStartPosition = (touch0StartPosition + touch1StartPosition) / 2;

            var rotationAmount = 0f;

            if (touch0.position.x < midpointStartPosition.x)
            {
                var touch0VerticalMovement = touch0.deltaPosition.y;
                var touch1VerticalMovement = touch1.deltaPosition.y;

                if ((touch0VerticalMovement > 0 && touch1VerticalMovement < 0) ||
                    (touch0VerticalMovement < 0 && touch1VerticalMovement > 0))
                {
                    rotationAmount = (touch0VerticalMovement - touch1VerticalMovement) * _config.RotationSpeed *
                                     Time.deltaTime;
                }
            }
            else
            {
                var touch0VerticalMovement = touch0.deltaPosition.y;
                var touch1VerticalMovement = touch1.deltaPosition.y;

                if ((touch0VerticalMovement > 0 && touch1VerticalMovement < 0) ||
                    (touch0VerticalMovement < 0 && touch1VerticalMovement > 0))
                {
                    rotationAmount = (touch1VerticalMovement - touch0VerticalMovement) * _config.RotationSpeed *
                                     Time.deltaTime;
                }
            }

            _onRotationInputReceived.OnNext(rotationAmount);
        }

        private void HandleEditorInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startTouchPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                _currentTouchPosition = Input.mousePosition;
                _inputDirection = (_currentTouchPosition - _startTouchPosition).normalized;
                _onMovementInputReceived.OnNext(_inputDirection);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _inputDirection = Vector2.zero;
            }
        }

        public void Dispose()
        {
            _onMovementInputReceived.Dispose();
            _onZoomInputReceived.Dispose();
            _onRotationInputReceived.Dispose();
            _disposable.Dispose();
        }
    }
}

