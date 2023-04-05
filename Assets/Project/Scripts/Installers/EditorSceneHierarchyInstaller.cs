using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    public class EditorSceneHierarchyInstaller : MonoInstaller
    {
        [SerializeField] private Camera mainCamera;

        public override void InstallBindings()
        {
            Container.BindInstance(mainCamera).AsSingle().NonLazy();
        }
    }
}