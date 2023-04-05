using UniRx;
using Zenject;

namespace Project.Scripts.EditorScene.UI
{
    public class SpawnUiBarVisibilityController : IInitializable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private CompositeDisposable _disposable;
        
        public void Initialize()
        {
            _signalBus.GetStream<EditorSceneSignals.GroundSelectedSignal>().Subscribe(signal =>
            {
                _signalBus.Fire(new EditorSceneSignals.SpawnUiVisibilitySignal
                {
                    IsVisible = false
                });
            }).AddTo(_disposable);

            _signalBus.GetStream<EditorSceneSignals.SelectionReleaseSignal>().Subscribe(_ =>
            {
                _signalBus.Fire(new EditorSceneSignals.SpawnUiVisibilitySignal
                {
                    IsVisible = true
                });
            }).AddTo(_disposable);
        }
    }
}