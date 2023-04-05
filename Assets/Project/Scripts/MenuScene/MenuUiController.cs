using Project.Scripts.Common;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.MenuScene
{
    public class MenuUiController : IInitializable
    {
        [Inject] private MenuUiView _menuUiView;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private ISceneNameRegistry _sceneNameRegistry;
        [Inject] private CompositeDisposable _disposable;
        [Inject] private SceneNameItemFactory _nameItemFactory;

        public async void Initialize()
        {
            _menuUiView.BuildGameButton.onClick.AsObservable().Subscribe(async _ =>
            {
                await _sceneLoader.SwitchScene(Constants.MenuSceneName, Constants.EditorSceneName);
            }).AddTo(_disposable);

            var sceneNames = await _sceneNameRegistry.GetFileNamesAsync();

            foreach (var sceneName in sceneNames)
            {
                var sceneNameItem = _nameItemFactory.Create();
                sceneNameItem.Initialize(sceneName, _menuUiView.SceneNameListContainer, async () =>
                {
                    Debug.Log($"Load Play Scene with {sceneName} json file");
                    await _sceneLoader.SwitchScene(Constants.MenuSceneName, Constants.GameSceneName, sceneName);
                });
            }
        }
    }
}