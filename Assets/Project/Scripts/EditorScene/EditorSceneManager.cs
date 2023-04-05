using System;
using UniRx;
using Zenject;

namespace Project.Scripts.EditorScene
{
    public class EditorSceneManager: IInitializable, IDisposable
    {
        [Inject] private CompositeDisposable _disposable;
        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}