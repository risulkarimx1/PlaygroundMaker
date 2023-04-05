using System;
using Project.Scripts.Common.Popups;
using Project.Scripts.EditorScene.Command;
using Project.Scripts.EditorScene.Persistence;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene.Inspector
{
    public abstract class InspectorControllerBase : IInitializable, IDisposable
    {
        [Inject] private DestroyCommandFactory _destroyCommandFactory;
        
        [Inject] protected InspectorView InspectorView;
        [Inject] protected CompositeDisposable Disposable;
        [Inject] protected SignalBus SignalBus;
        [Inject] protected TranslateCommandFactory TranslateCommandFactory;
        [Inject] protected ICommandManager CommandManager;
        [Inject] private PopupController _popupController;
        [Inject] private CompositeDisposable _compositeDisposable;
        [Inject] private ISpawnedItemsRegistry _spawnedItemsRegistry;

        private IDisposable _deleteButtonStream;
        protected IDisposable ChangeStream;
        protected GameObject ObjectInInspector;
        protected TransformCommand TransformCommand;

        public void Initialize()
        {
            InspectorView.SetVisibility(false);
            SignalBus.Subscribe<EditorSceneSignals.SpawnableObjectSelectedSignal>(OnObjectSelected);
            SignalBus.Subscribe<EditorSceneSignals.GroundSelectedSignal>(OnGroundSelected);
            SignalBus.GetStream<EditorSceneSignals.HideInspectorSignal>().Subscribe(_ =>
            {
                HideInspector();
            }).AddTo(_compositeDisposable);
            InitializeSliderEvents();
        }

        protected abstract void InitializeSliderEvents();

        private void OnGroundSelected(EditorSceneSignals.GroundSelectedSignal obj)
        {
            ObjectInInspector = null;
            HideInspector();
        }

        private void HideInspector()
        {
            InspectorView.SetVisibility(false);
            ChangeStream?.Dispose();
        }

        protected virtual void OnObjectSelected(EditorSceneSignals.SpawnableObjectSelectedSignal obj)
        {
            InspectorView.SetVisibility(true, _spawnedItemsRegistry.GetName(obj.SpawnableObject));
            ObjectInInspector = obj.SpawnableObject;
            InitializeDeleteButton();
        }

        private void InitializeDeleteButton()
        {
            _deleteButtonStream?.Dispose();

            _deleteButtonStream = InspectorView.DeleteObjectButton.onClick.AsObservable().Subscribe(_ =>
            {
                HideInspector();
                var destroyCommand = _destroyCommandFactory.Create(ObjectInInspector);
                _popupController.ShowPopup<ConfirmationPopup>("Delete Object", "Are you sure you want to delete?",
                    _ =>
                    {
                        CommandManager.ExecuteCommand(destroyCommand);
                        _popupController.HidePopup<ConfirmationPopup>();
                    }, () => { _popupController.HidePopup<ConfirmationPopup>(); });
            }).AddTo(Disposable);
        }

        public void Dispose()
        {
            SignalBus.TryUnsubscribe<EditorSceneSignals.SpawnableObjectSelectedSignal>(OnObjectSelected);
            SignalBus.TryUnsubscribe<EditorSceneSignals.GroundSelectedSignal>(OnGroundSelected);
            DisposeSliderEvents();
        }

        protected abstract void DisposeSliderEvents();
    }
}