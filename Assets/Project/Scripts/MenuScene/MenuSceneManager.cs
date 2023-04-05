using System;
using UniRx;
using Zenject;

namespace Project.Scripts.MenuScene
{
    public class MenuSceneManager: IInitializable, IDisposable
    {
        [Inject] private CompositeDisposable _disposable;
        
        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}