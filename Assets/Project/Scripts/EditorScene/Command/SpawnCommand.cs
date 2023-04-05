using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Common;
using Project.Scripts.EditorScene.Interaction;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Command
{
    public class SpawnCommandFactory : PlaceholderFactory<string, SpawnCommand>
    {
    }

    public class SpawnCommand : ICommand
    {
        [Inject] private Camera _camera;
        [Inject] private IElementSpawner _spawner;
        [Inject] private IInputHandler _inputHandler;
        [Inject] private SignalBus _signalBus;

        private GameObject _instantiatedObject;
        private readonly string _assetAddress;

        public SpawnCommand(string assetAddress)
        {
            _assetAddress = assetAddress;
        }

        public async UniTask Execute(Action<GameObject> onCompleted)
        {
            var mousePosition = _inputHandler.TouchPosition;

            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit, 1000, Constants.GroundLayer)) return;

            var target = hit.point;
            target.y = 0;

            _instantiatedObject = await _spawner.SpawnElementAsync(_assetAddress, target);
            
            _signalBus.Fire( new EditorSceneSignals.ObjectSpawnedSignal
            {
                SpawnedObject = _instantiatedObject,
                AssetAddress = _assetAddress
            });

            onCompleted?.Invoke(_instantiatedObject);
        }

        public async UniTask Undo(Action<GameObject> onCompleted)
        {
            Debug.Log($"calling undo on {_instantiatedObject}");
            _signalBus.Fire( new EditorSceneSignals.ObjectDestroyedSignal()
            {
                DestroyedObject = _instantiatedObject
            });
            GameObject.Destroy(_instantiatedObject);
            await UniTask.DelayFrame(1);
            onCompleted?.Invoke(null);
        }

        public override string ToString()
        {
            return $"{_instantiatedObject} Command type is Spawn";
        }
    }
}