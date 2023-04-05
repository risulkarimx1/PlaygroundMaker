using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Common;
using Project.Scripts.EditorScene.Persistence;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Command
{
    public class DestroyCommandFactory : PlaceholderFactory<GameObject, DestroyCommand>
    {
    }


    public class DestroyCommand : ICommand
    {
        [Inject] private IElementSpawner _spawner;
        [Inject] private SignalBus _signalBus;
        [Inject] private ISpawnedItemsRegistry _spawnRegistry;

        private readonly Vector3 _position;
        private readonly Quaternion _rotation;
        private readonly Vector3 _scale;
        private readonly GameObject _selectedObject;

        private GameObject _respawnedObject;
        private string _assetAddress;

        public DestroyCommand(GameObject selectedObject)
        {
            _selectedObject = selectedObject;
            _position = _selectedObject.transform.position;
            _rotation = _selectedObject.transform.rotation;
            _scale = _selectedObject.transform.localScale;
        }
        
        public UniTask Execute(Action<GameObject> onCompleted)
        {
            _assetAddress = _spawnRegistry.GetAddress(_selectedObject);
            
            _signalBus.Fire(new EditorSceneSignals.ObjectDestroyedSignal()
            {
                DestroyedObject = _selectedObject
            });
            
            GameObject.Destroy(_selectedObject);
            onCompleted?.Invoke(_selectedObject);

            return UniTask.CompletedTask;
        }

        public async UniTask Undo(Action<GameObject> onCompleted)
        {
            _respawnedObject = await _spawner.SpawnElementAsync(_assetAddress, _position);
            _respawnedObject.transform.rotation = _rotation;
            _respawnedObject.transform.localScale = _scale;

            _signalBus.Fire(new EditorSceneSignals.ObjectSpawnedSignal()
            {
                SpawnedObject = _respawnedObject,
                AssetAddress = _assetAddress
            });
            
            onCompleted?.Invoke(_respawnedObject);
        }

        public override string ToString()
        {
            return $"{_selectedObject} Command type is Destroy";
        }
    }
}
