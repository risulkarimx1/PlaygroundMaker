using System.ComponentModel;
using System.IO;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Project.Scripts.Common;
using Project.Scripts.EditorScene.Persistence;
using UniRx;
using UnityEngine;

namespace Project.Scripts.UnitTests.Editor
{
    public class PersistnaceSystemsTests : UnitTestsBase
    {
        private const string TestJsonFileName = "testScene.json";
        
        [SetUp]
        public void CommonInstall()
        {
            DefaultInstall();
            Container.Bind<CompositeDisposable>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneSaver>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneNameRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnedItemsRegistry>().AsSingle();
        }

        [TearDown]
        public void Cleanup()
        {
            var filePath = Path.Combine(Application.persistentDataPath, TestJsonFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var disposable = Container.Resolve<CompositeDisposable>();
            disposable?.Dispose();
        }

        [Test]
        public void SaveSceneAsJsonAsync_SavesFileCorrectly()
        {
            var sceneSaver = Container.Resolve<ISceneSaver>();
            var filePath = Path.Combine(Application.persistentDataPath, TestJsonFileName);
            sceneSaver.SaveSceneAsJsonAsync(TestJsonFileName).Forget();
            Assert.IsTrue(File.Exists(filePath));
        }
    }
}