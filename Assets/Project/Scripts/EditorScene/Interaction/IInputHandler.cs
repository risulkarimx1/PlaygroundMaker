using System;
using UnityEngine;

namespace Project.Scripts.EditorScene.Interaction
{
    public interface IInputHandler
    {
        IObservable<long> TapDownStream { get; }
        IObservable<long> TapHoldStream { get; }
        IObservable<long> TapReleaseStream { get; }
        
        public Vector3 TouchPosition { get; }
    }
}