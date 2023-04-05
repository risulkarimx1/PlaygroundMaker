using Project.Scripts.Common;
using Project.Scripts.EditorScene;
using Project.Scripts.EditorScene.Command;
using Project.Scripts.EditorScene.Inspector;
using Project.Scripts.EditorScene.Interaction;
using Project.Scripts.EditorScene.Persistence;
using Project.Scripts.EditorScene.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "EditorSceneInstaller", menuName = "Installers/EditorSceneInstaller")]
    public class EditorSceneInstaller : ScriptableObjectInstaller<EditorSceneInstaller>
    {
        [SerializeField] private EditorSceneUiView editorSceneUiViewPrefab;
        [SerializeField] private InspectorView inspectorViewPrefab;
        [SerializeField] private GameObject spawnerButtonPrefab;
        [SerializeField] private GroundHighlightViewController groundHighlightPrefab;
        [SerializeField] private CameraRig cameraRigPrefab;
        public override void InstallBindings()
        {
            Container.Bind<CompositeDisposable>().AsSingle();
            InstallSignals();
            
            Container.BindInterfacesAndSelfTo<SpawnAddressablesRegistry>().FromScriptableObjectResource(Constants.SpawnableItemRegistryPath).AsSingle();

            Container.BindInterfacesAndSelfTo<EditorSceneCameraConfig>().FromScriptableObjectResource(Constants.EditorSceneCameraConfigPath)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<SceneSaver>().AsSingle();
            Container.Bind<EditorSceneUiView>().FromComponentInNewPrefab(editorSceneUiViewPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<EditorSceneUiController>().AsSingle().NonLazy();
            
            Container.Bind<InspectorView>().FromComponentInNewPrefab(inspectorViewPrefab).AsSingle();
            
            Container.BindInterfacesAndSelfTo<RotationInspectorController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ScaleInspectorController>().AsSingle().NonLazy();
            
            Container.BindFactory<SpawnerButtonView, SpawnerButtonFactory>()
                .FromComponentInNewPrefab(spawnerButtonPrefab)
                .WithGameObjectName(nameof(spawnerButtonPrefab));

            Container.Bind<CameraRig>().FromComponentInNewPrefab(cameraRigPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<CameraInputHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CameraMovementController>().AsSingle().NonLazy();

            
            Container.Bind<IInputHandler>().WithId(nameof(MouseInputHandler)).To<MouseInputHandler>().AsSingle();


            Container.BindInterfacesAndSelfTo<SceneInteractionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SelectedObjectMover>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnUiBarVisibilityController>().AsSingle();

            Container.BindInterfacesAndSelfTo<SpawnedItemsRegistry>().AsSingle();
            
            Container.Bind<GroundHighlightViewController>().FromComponentInNewPrefab(groundHighlightPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SpawnCommand>().AsTransient();
            Container.BindFactory<string, SpawnCommand, SpawnCommandFactory>().AsSingle();
            Container.BindFactory<GameObject, TransformCommand, TranslateCommandFactory>().AsSingle();
            Container.BindFactory<GameObject, DestroyCommand, DestroyCommandFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<CommandManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<EditorSceneManager>().AsSingle().NonLazy();

            
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<EditorSceneSignals.GroundSelectedSignal>();
            Container.DeclareSignal<EditorSceneSignals.SpawnableObjectSelectedSignal>();
            Container.DeclareSignal<EditorSceneSignals.SelectionReleaseSignal>();
            Container.DeclareSignal<EditorSceneSignals.ObjectSpawnedSignal>();
            Container.DeclareSignal<EditorSceneSignals.ObjectDestroyedSignal>();
            Container.DeclareSignal<EditorSceneSignals.HideInspectorSignal>();
            Container.DeclareSignal<EditorSceneSignals.SpawnUiVisibilitySignal>();
            Container.DeclareSignal<EditorSceneSignals.TouchedOnUiSignal>();
        }
    }
}