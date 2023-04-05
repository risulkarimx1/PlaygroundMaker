using System;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.EditorScene.Command
{
    public interface ICommand
    {
        UniTask Execute([NotNull] Action<GameObject> onComplete);
        UniTask Undo([NotNull] Action <GameObject> onComplete);

        string ToString();
    }
}