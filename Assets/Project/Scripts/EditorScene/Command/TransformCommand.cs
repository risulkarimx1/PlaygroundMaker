using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Common;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Command
{
    public class TranslateCommandFactory : PlaceholderFactory<GameObject, TransformCommand>
    {
    }

    public class TransformCommand : ICommand
    {
        private readonly GameObject _selectedObject;
        private Vector3 _initialPosition;
        private Vector3 _initialScale;
        private Quaternion _initialRotation;

        public TransformCommand(GameObject selectedObject)
        {
            _selectedObject = selectedObject;
            _initialPosition = _selectedObject.transform.position;
            _initialRotation = _selectedObject.transform.rotation;
            _initialScale = _selectedObject.transform.localScale;
        }

        public UniTask Execute(Action<GameObject> onCompleted)
        {
            var positionDifference = Vector3.Distance(_initialPosition, _selectedObject.transform.position);
            var rotationDifference = Quaternion.Angle(_initialRotation, _selectedObject.transform.rotation);
            var scaleDifference = Vector3.Distance(_initialScale, _selectedObject.transform.localScale);

            var isPositionSame = positionDifference < Constants.TransformCommandTolerance;
            var isRotationSame = rotationDifference < Constants.TransformCommandTolerance;
            var isScaleSame = scaleDifference < Constants.TransformCommandTolerance;

            if (!isPositionSame || !isRotationSame || !isScaleSame)
            {
                onCompleted?.Invoke(_selectedObject);
            }

            return UniTask.CompletedTask;
        }

        public UniTask Undo(Action<GameObject> onCompleted)
        {
            _selectedObject.transform.position = _initialPosition;
            _selectedObject.transform.rotation = _initialRotation;
            _selectedObject.transform.localScale = _initialScale;
            onCompleted?.Invoke(_selectedObject);
            return UniTask.CompletedTask;
        }
    }
}
