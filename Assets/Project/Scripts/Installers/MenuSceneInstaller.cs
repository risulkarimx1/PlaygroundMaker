using Project.Scripts.MenuScene;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "MenuSceneInstaller", menuName = "Installers/MenuSceneInstaller")]
    public class MenuSceneInstaller : ScriptableObjectInstaller<MenuSceneInstaller>
    {
        [SerializeField] private MenuUiView menuUiViewPrefab;
        [SerializeField] private SceneNameItem sceneNameItemPrefab;

        public override void InstallBindings()
        {
            Container.Bind<MenuUiView>().FromComponentInNewPrefab(menuUiViewPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MenuUiController>().AsSingle().NonLazy();

            Container.Bind<CompositeDisposable>().AsSingle();
            
            Container.BindFactory<SceneNameItem, SceneNameItemFactory>()
                .FromComponentInNewPrefab(sceneNameItemPrefab);
            
            Container.BindInterfacesAndSelfTo<MenuSceneManager>().AsSingle().NonLazy();
        }
    }
}