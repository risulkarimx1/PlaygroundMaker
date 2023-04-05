using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Persistence
{
    public interface ISpawnedItemsRegistry
    {
        ReactiveProperty<int> SpawnedObjectCount { get; }
        string GetAddress(GameObject gameObject);
        string GetName(GameObject gameObject);
        IEnumerable<GameObject> GetSpawnedObjects();

        void AddObject(GameObject spawnedObject, string address);
        void RemoveObject(GameObject spawnedObject);
    }

    public class SpawnedItemsRegistry : ISpawnedItemsRegistry, IInitializable
    {
        [Inject] private SpawnAddressablesRegistry _spawnAddressablesRegistry;
        [Inject] private CompositeDisposable _compositeDisposable;
        [Inject] private SignalBus _signalBus;

        private readonly Dictionary<GameObject, string> _objectToAssetAddressMap = new();
        private int _spawnedObjectCount;
        private ReactiveProperty<int> _getSpawnedObjectCount;


        public ReactiveProperty<int> SpawnedObjectCount { get; private set; }

        public SpawnedItemsRegistry()
        {
            SpawnedObjectCount = new ReactiveProperty<int>();
        }

        public string GetAddress(GameObject gameObject)
        {
            return _objectToAssetAddressMap[gameObject];
        }

        public string GetName(GameObject gameObject)
        {
            return _spawnAddressablesRegistry.GetNameFromAddress(GetAddress(gameObject));
        }

        public IEnumerable<GameObject> GetSpawnedObjects()
        {
            return _objectToAssetAddressMap.Keys;
        }

        public void AddObject(GameObject spawnedObject, string address)
        {
            _objectToAssetAddressMap.Add(spawnedObject, address);
            SpawnedObjectCount.Value = _objectToAssetAddressMap.Count;
        }

        public void RemoveObject(GameObject destroyedObject)
        {
            _objectToAssetAddressMap.Remove(destroyedObject);
            SpawnedObjectCount.Value = _objectToAssetAddressMap.Count;
        }

        public void Initialize()
        {
            _signalBus.GetStream<EditorSceneSignals.ObjectSpawnedSignal>().Subscribe(signal =>
            {
                AddObject(signal.SpawnedObject, signal.AssetAddress);
            }).AddTo(_compositeDisposable);

            _signalBus.GetStream<EditorSceneSignals.ObjectDestroyedSignal>().Subscribe(signal =>
            {
                RemoveObject(signal.DestroyedObject);
                
            }).AddTo(_compositeDisposable);
        }
    }
}