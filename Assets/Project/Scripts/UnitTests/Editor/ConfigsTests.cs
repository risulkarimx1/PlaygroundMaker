using NUnit.Framework;
using Project.Scripts.Common;
using Project.Scripts.EditorScene.Interaction;
using Project.Scripts.EditorScene.Persistence;
using Zenject;

namespace Project.Scripts.UnitTests.Editor
{
    [TestFixture]
    public class ConfigsTests: ZenjectUnitTestFixture
    {
        [SetUp]
        public void Initialize()
        {

            Container.BindInterfacesAndSelfTo<SpawnAddressablesRegistry>().FromScriptableObjectResource(Constants.SpawnableItemRegistryPath).AsSingle();

            Container.BindInterfacesAndSelfTo<EditorSceneCameraConfig>().FromScriptableObjectResource(Constants.EditorSceneCameraConfigPath)
                .AsSingle();
        }

        [Test]
        public void SpawnAddressablesRegistryNoNull()
        {
            var registry = Container.Resolve<SpawnAddressablesRegistry>();
            Assert.NotNull(registry);
        }
        
        [Test]
        public void EditorSceneCameraConfigNoNull()
        {
            var config = Container.Resolve<EditorSceneCameraConfig>();
            Assert.NotNull(config);
        }
    }
}
