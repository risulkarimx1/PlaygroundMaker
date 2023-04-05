using Project.Scripts.Common;
using Project.Scripts.Common.Popups;
using Project.Scripts.EditorScene.Persistence;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public class ProjectInstaller : ScriptableObjectInstaller<ProjectInstaller>
    {
        [SerializeField] private LoadingViewController loadingViewControllerPrefab;
        [SerializeField] private PopupView popupViewPrefab;
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<SceneProxy>().AsSingle().NonLazy();
            Container.Bind<LoadingViewController>().FromComponentInNewPrefab(loadingViewControllerPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SpawnAddressablesRegistry>().FromScriptableObjectResource(Constants.SpawnableItemRegistryPath)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<ElementSpawner>().AsSingle();
            
            Container.Bind<PopupView>().FromComponentInNewPrefab(popupViewPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneNameRegistry>().AsSingle().NonLazy();
            Container.Bind<PopupController>().AsSingle();
        }
    }
}