using System.IO;
using Cysharp.Threading.Tasks;
using Project.Scripts.Common;
using Project.Scripts.EditorScene.Persistence;
using UnityEngine;

namespace Project.Scripts.GameScene
{
    public interface ISceneObjectsLoader
    {
        UniTask LoadSceneObjectsAsync(string sceneName);
    }

    public class SceneObjectsLoader : ISceneObjectsLoader
    {
        private readonly IElementSpawner _elementSpawner;
        private readonly ISceneNameRegistry _sceneNameRegistry;

        public SceneObjectsLoader(IElementSpawner elementSpawner, ISceneNameRegistry sceneNameRegistry)
        {
            _elementSpawner = elementSpawner;
            _sceneNameRegistry = sceneNameRegistry;
        }

        public async UniTask LoadSceneObjectsAsync(string sceneName)
        {
            string fileName = $"{sceneName}";
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(filePath))
            {
                string jsonContent = await File.ReadAllTextAsync(filePath);
                SceneData sceneData = JsonUtility.FromJson<SceneData>(jsonContent);

                foreach (GameObjectData gameObjectData in sceneData.gameObjects)
                {
                    await _elementSpawner.SpawnElementAsync(
                        gameObjectData.address,
                        gameObjectData.position,
                        gameObjectData.yRotation
                    );
                }
            }
            else
            {
                Debug.LogError($"Scene file not found: {filePath}");
            }
        }
    }
}