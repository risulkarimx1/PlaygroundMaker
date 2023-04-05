using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameScene.Player
{
    public class PlayerJoystick : ITickable
    {
        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;
        public ReactiveProperty<Vector2> InputDirection { get; }

        public PlayerJoystick()
        {
            InputDirection = new ReactiveProperty<Vector2>();
        }

        public void Tick()
        {
#if UNITY_EDITOR
            HandleEditorInput();
#elif UNITY_IOS || UNITY_ANDROID
            HandleMobileInput();
#endif
        }

        private void HandleMobileInput()
        {
            if (Input.touchCount <= 0) return;

            var touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    _currentTouchPosition = touch.position;
                    InputDirection.Value = (_currentTouchPosition - _startTouchPosition).normalized;
                    break;
                case TouchPhase.Ended:
                    InputDirection.Value = Vector2.zero;
                    break;
            }
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
                InputDirection.Value = (_currentTouchPosition - _startTouchPosition).normalized;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                InputDirection.Value = Vector2.zero;
            }
        }
    }
}