using NUnit.Framework;
using Project.Scripts.EditorScene.Persistence;
using UniRx;
using UnityEngine;

namespace Project.Scripts.UnitTests.Editor
{
    public class SpawnedItemsRegistryTests : UnitTestsBase
    {
        private const string TestAddress1 = "test_address_1";
        private const string TestName1 = "test_name_1";
        private const string TestAddress2 = "test_address_2";
        private const string TestName2 = "test_name_2";

        [SetUp]
        public void CommonInstall()
        {
            DefaultInstall();
            Container.BindInterfacesAndSelfTo<SpawnedItemsRegistry>().AsSingle().NonLazy();
            Container.Bind<CompositeDisposable>().AsSingle();
        }

        [Test]
        public void SpawnedItemsRegistry_InitializesProperly()
        {
            // Act
            var spawnedItemsRegistry = Container.Resolve<ISpawnedItemsRegistry>();

            // Assert
            Assert.IsNotNull(spawnedItemsRegistry);
            Assert.AreEqual(0, spawnedItemsRegistry.SpawnedObjectCount.Value);
        }

        [Test]
        public void SpawnedItemsRegistry_UpdatesOnObjectSpawnedSignal()
        {
            var spawnedItemsRegistry = Container.Resolve<ISpawnedItemsRegistry>();
            var testObject1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            testObject1.name = TestName1;
            var testObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            testObject2.name = TestName2;
            
            spawnedItemsRegistry.AddObject(testObject1, TestAddress1);
            spawnedItemsRegistry.AddObject(testObject2, TestAddress2);
            
            Assert.AreEqual(2, spawnedItemsRegistry.SpawnedObjectCount.Value);
            Assert.AreEqual(TestAddress1, spawnedItemsRegistry.GetAddress(testObject1));
            Assert.AreEqual(TestAddress2, spawnedItemsRegistry.GetAddress(testObject2));
        }
    }
}
