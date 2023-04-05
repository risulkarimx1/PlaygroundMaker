using Project.Scripts.GameScene;
using Project.Scripts.GameScene.Player;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "GameSceneInstaller", menuName = "Installers/GameSceneInstaller")]
    public class GameSceneInstaller : ScriptableObjectInstaller<GameSceneInstaller>
    {
        [SerializeField] private PlayerView playerPrefab;
        [SerializeField] private PlayerCamera playerCameraPrefab;
        [SerializeField] private GameSceneUiView gameSceneUiViewPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<CompositeDisposable>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerJoystick>().AsSingle().NonLazy();
            
            Container.Bind<PlayerView>().FromComponentInNewPrefab(playerPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();
            
            Container.Bind<PlayerCamera>().FromComponentInNewPrefab(playerCameraPrefab).AsSingle();
            
            Container.Bind<GameSceneUiView>().FromComponentInNewPrefab(gameSceneUiViewPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<GameSceneUiController>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<SceneObjectsLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameSceneManager>().AsSingle().NonLazy();
        }
    }
}