using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Common
{
    public class SceneLoader
    {
        [Inject] private LoadingViewController _loadingViewController;
        [Inject] private SceneProxy _sceneProxy;
        public async UniTask SwitchScene(string currentSceneName, string nextSceneName, string args = null)
        {
            _sceneProxy.SceneName = args;
            _loadingViewController.Show();
            var progress = Progress.Create<float>(fill => { _loadingViewController.UpdateFill(fill); });
            await SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive).ToUniTask(progress);
            await SceneManager.UnloadSceneAsync(currentSceneName);
            await UniTask.Delay(Constants.FakeDelayBecauseIloveIt);
            _loadingViewController.Hide();
        }
    }
}
