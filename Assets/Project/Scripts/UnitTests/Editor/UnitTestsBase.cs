using Project.Scripts.Common;
using Project.Scripts.EditorScene;
using Project.Scripts.EditorScene.Interaction;
using Project.Scripts.EditorScene.Persistence;
using Zenject;

namespace Project.Scripts.UnitTests.Editor
{
    public class UnitTestsBase : ZenjectUnitTestFixture
    {
        protected void DefaultInstall()
        {
            InstallSignals();
            ConfigsInstall();
        }

        private void ConfigsInstall()
        {
            Container.BindInterfacesAndSelfTo<SpawnAddressablesRegistry>()
                .FromScriptableObjectResource(Constants.SpawnableItemRegistryPath).AsSingle();

            Container.BindInterfacesAndSelfTo<EditorSceneCameraConfig>()
                .FromScriptableObjectResource(Constants.EditorSceneCameraConfigPath)
                .AsSingle();
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