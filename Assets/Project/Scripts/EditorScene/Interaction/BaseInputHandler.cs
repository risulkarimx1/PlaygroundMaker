using System;
using UniRx;
using UnityEngine;

namespace Project.Scripts.EditorScene.Interaction
{
    public abstract class BaseInputHandler : IInputHandler
    {
        public IObservable<long> TapDownStream { get; protected set; }
        public IObservable<long> TapHoldStream { get; protected set; }
        public IObservable<long> TapReleaseStream { get; protected set; }

        public abstract Vector3 TouchPosition { get; }

        protected void InitializeStreams(Func<bool> tapDown, Func<bool> tapHold, Func<bool> tapRelease)
        {
            TapDownStream = Observable.EveryUpdate()
                .Where(_ => tapDown());

            TapHoldStream = Observable.EveryUpdate()
                .Where(_ => tapHold());

            TapReleaseStream = Observable.EveryUpdate()
                .Where(_ => tapRelease());
        }
    }
}