using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Common
{
    public class ElementSpawner : IElementSpawner
    {
        private readonly Dictionary<string, GameObject> _prefabCache = new();

        public async UniTask<GameObject> SpawnElementAsync(string address, Vector3 position)
        {
            if (_prefabCache.ContainsKey(address))
            {
                var prefab = _prefabCache[address];
                return InstantiatePrefab(prefab, position);
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            await handle.ToUniTask();
            _prefabCache.Add(address, handle.Result);
            var spawnableObject = InstantiatePrefab(handle.Result, position);
            return spawnableObject;
        }

        public async UniTask<GameObject> SpawnElementAsync(string address, Vector3 position, float rotation)
        {
            var spawnedObject = await SpawnElementAsync(address, position);
            var currentAngle = spawnedObject.transform.eulerAngles;
            currentAngle.y = rotation;
            spawnedObject.transform.eulerAngles = currentAngle;
            return spawnedObject;
        }

        private static GameObject InstantiatePrefab(GameObject prefab, Vector3 position)
        {
            var instance = GameObject.Instantiate(prefab, position, Quaternion.identity);
            instance.name = $"{instance.name}: {instance.GetInstanceID()}";
            return instance;
        }
    }

    public interface IElementSpawner
    {
        UniTask<GameObject> SpawnElementAsync(string address, Vector3 position);
        UniTask<GameObject> SpawnElementAsync(string address, Vector3 position, float rotation);
    }
}