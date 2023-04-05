using Project.Scripts.Common;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameScene.Player
{
    public class PlayerController : IInitializable
    {
        private static readonly int IsWalking = Animator.StringToHash(Constants.PlayerIsWalkingAnimationKey);

        [Inject] private PlayerView _playerView;
        [Inject] private PlayerJoystick _playerJoystick;
        [Inject] private CompositeDisposable _disposable;

        private float _currentRotationVelocity;
        public Transform PlayerTransform => _playerView.PlayerTransform;

        public void Initialize()
        {
            Observable.EveryUpdate().Where(_ => Input.touchCount > 0).Subscribe(_ =>
            {
                var direction = _playerJoystick.InputDirection.Value;
                HandleAnimation(direction);
                PlayerTransform.Translate(Vector3.forward * Time.deltaTime * Mathf.Max(0, direction.y) * _playerView.MoveSpeed);
                PlayerTransform.Rotate(Vector3.up * direction.x * Time.deltaTime * _playerView.RotationSpeed);
            }).AddTo(_disposable);
        }
        
        private void HandleAnimation(Vector2 direction)
        {
            _playerView.PlayerPlayerAnimator.SetBool(IsWalking, direction is { sqrMagnitude: > 0, y: > 0 });
        }
    }
}