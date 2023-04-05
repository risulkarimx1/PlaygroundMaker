using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Project.Scripts.Common;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Persistence
{
    public interface ISceneSaver
    {
        UniTask SaveSceneAsJsonAsync(string jsonFileName);
    }
    public class SceneSaver : ISceneSaver
    {
        [Inject] private ISceneNameRegistry _sceneNameRegistry;
        
        private readonly ISpawnedItemsRegistry _spawnedItemsRegistry;

        public SceneSaver(ISpawnedItemsRegistry spawnedItemsRegistry)
        {
            _spawnedItemsRegistry = spawnedItemsRegistry;
        }

        public async UniTask SaveSceneAsJsonAsync(string jsonFileName)
        {
            var filePath = Path.Combine(Application.persistentDataPath, jsonFileName);
            var sceneData = GetSceneData();
            var jsonContent = JsonUtility.ToJson(sceneData);

            await SaveFileAsync(filePath, jsonContent);
            await _sceneNameRegistry.AddFileNameAsync(jsonFileName);
        }

        private async UniTask SaveFileAsync(string filePath, string content)
        {
            await using StreamWriter streamWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            await streamWriter.WriteAsync(content);
            Debug.Log($"File saved at: {filePath}");
        }

        private SceneData GetSceneData()
        {
            var sceneData = new SceneData
            {
                gameObjects = new List<GameObjectData>()
            };

            foreach (var spawnedObject in _spawnedItemsRegistry.GetSpawnedObjects())
            {
                var objectData = new GameObjectData
                {
                    address = _spawnedItemsRegistry.GetAddress(spawnedObject),
                    position = spawnedObject.transform.position,
                    localScale = spawnedObject.transform.localScale,
                    yRotation = spawnedObject.transform.eulerAngles.y
                };

                sceneData.gameObjects.Add(objectData);
            }

            return sceneData;
        }
    }

    [System.Serializable]
    public class SceneData
    {
        public List<GameObjectData> gameObjects;
    }

    [System.Serializable]
    public class GameObjectData
    {
        public string address;
        public Vector3 position;
        public Vector3 localScale;
        public float yRotation;
    }
}
