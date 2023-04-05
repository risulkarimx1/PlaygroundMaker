using System;
using Project.Scripts.Common;
using Project.Scripts.GameScene.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GameScene
{
    public class GameSceneManager : IInitializable, IDisposable
    {
        [Inject] private CompositeDisposable _disposable;
        [Inject] private SceneProxy _sceneProxy;
        [Inject] private ISceneObjectsLoader _sceneObjectsLoader;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PlayerController _playerController;
        [Inject] private PlayerCamera _playerCamera;

        public async void Initialize()
        {
            if (_sceneProxy.SceneName == null)
            {
                Debug.LogError("No Scene is loaded. Going back to main menu");
                await _sceneLoader.SwitchScene(Constants.GameSceneName, Constants.MenuSceneName);
            }
            else
            {
                var sceneName = _sceneProxy.SceneName;
                await _sceneObjectsLoader.LoadSceneObjectsAsync(sceneName);

                SetupCamera();
            }
        }

        private void SetupCamera()
        {
            _playerCamera.Initialize(_playerController.PlayerTransform);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}