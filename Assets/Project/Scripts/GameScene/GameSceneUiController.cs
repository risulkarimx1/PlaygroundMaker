using Project.Scripts.Common;
using UniRx;
using Zenject;

namespace Project.Scripts.GameScene
{
    public class GameSceneUiController : IInitializable
    {
        [Inject] private GameSceneUiView _gameSceneUiView;
        [Inject] private CompositeDisposable _disposable;
        [Inject] private SceneLoader _sceneLoader;

        public void Initialize()
        {
            _gameSceneUiView.MenuButton.onClick.AsObservable().Subscribe(async _ =>
            {
                await _sceneLoader.SwitchScene(Constants.GameSceneName, Constants.MenuSceneName);
            }).AddTo(_disposable);
        }
    }
}