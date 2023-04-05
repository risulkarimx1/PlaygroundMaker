using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Interaction
{
    public class CameraMovementController : IInitializable, IDisposable
    {
        [Inject] private EditorSceneCameraConfig _config;
        [Inject] private CameraRig _cameraRig;
        [Inject] private CameraInputHandler _cameraInputHandler;
        [Inject] private CompositeDisposable _disposable;

        private Transform _targetObject;

        public void Initialize()
        {
            _targetObject = _cameraRig.TargetObject.transform;

            _cameraInputHandler.OnMovementInputReceived.Subscribe(inputDirection =>
            {
                Vector3 worldDirection = GetWorldMovementDirection(inputDirection);
                _targetObject.transform.position -= worldDirection * 5 * Time.deltaTime;
            }).AddTo(_disposable);

            _cameraInputHandler.OnZoomInputReceived.Subscribe(pinchZoomAmount =>
            {
                var transformPosition = _targetObject.transform.position;
                var targetPosition = transformPosition + new Vector3(0, pinchZoomAmount, 0);
                _targetObject.transform.position = Vector3.Lerp(transformPosition, targetPosition, Time.deltaTime * _config.SmoothDamping);
            }).AddTo(_disposable);

            _cameraInputHandler.OnRotationInputReceived.Subscribe(rotationAmount =>
            {
                Quaternion currentRotation = _targetObject.transform.rotation;
                Quaternion targetRotation = currentRotation * Quaternion.Euler(Vector3.up * rotationAmount);
                _targetObject.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * _config.SmoothDamping);
            }).AddTo(_disposable);
        }

        private Vector3 GetWorldMovementDirection(Vector2 inputDirection)
        {
            var transform = _targetObject.transform;
            return transform.forward * inputDirection.y + transform.right * inputDirection.x;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}