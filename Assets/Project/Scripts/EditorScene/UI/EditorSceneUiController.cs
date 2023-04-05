using System;
using Project.Scripts.Common;
using Project.Scripts.Common.Popups;
using Project.Scripts.EditorScene.Command;
using Project.Scripts.EditorScene.Persistence;
using UniRx;
using Zenject;

namespace Project.Scripts.EditorScene.UI
{
    public class EditorSceneUiController : IInitializable
    {
        [Inject] private CompositeDisposable _compositeDisposable;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private SpawnerButtonFactory _spawnerButtonFactory;
        [Inject] private SpawnAddressablesRegistry _spawnAddressablesRegistry;
        [Inject] private SpawnedItemsRegistry _spawnedItemsRegistry;
        [Inject] private ICommandManager _commandManager;
        [Inject] private EditorSceneUiView _editorSceneUiView;
        [Inject] private SignalBus _signal;
        [Inject] private ISceneSaver _sceneSaver;
        [Inject] private PopupController _popupController;

        public void Initialize()
        {
            CreateSpawnerButtons();
            SubscribeToMenuButtonClick();
            SubscribeToUndoButtonClick();
            SubscribeToSpawnUiSignal();
            SubscribeToSaveSceneButton();
            SubscribeToGameSceneButton();
            UpdateButtonInteractability();
        }

        private void UpdateButtonInteractability()
        {
            _spawnedItemsRegistry.SpawnedObjectCount.Subscribe(count =>
            {
                var sceneHasObjects = count > 0;

                _editorSceneUiView.SaveButton.interactable = sceneHasObjects;
                _editorSceneUiView.PlayButton.interactable = sceneHasObjects;
                _editorSceneUiView.UndoButton.interactable = sceneHasObjects;
            }).AddTo(_compositeDisposable);
        }

        private void SubscribeToSaveSceneButton()
        {
            _editorSceneUiView.SaveButton.onClick.AsObservable().Subscribe(_ =>
            {
                PromptSaveSceneWithConfirmation();
            }).AddTo(_compositeDisposable);
        }
        private void SubscribeToGameSceneButton()
        {
            _editorSceneUiView.PlayButton.onClick.AsObservable().Subscribe(async _ => { PromptSaveSceneWithConfirmation(async sceneName =>
            {
                await _sceneLoader.SwitchScene(Constants.EditorSceneName, Constants.GameSceneName, sceneName);
            }); }).AddTo(_compositeDisposable);
        }

        private void PromptSaveSceneWithConfirmation(Action<string> moveToGameScene = null)
        {
            _popupController.ShowPopup<TextInputPopup>("Save Scene", "Are you ready to save?", async obj =>
            {
                var sceneName = obj as string;
                if (string.IsNullOrEmpty(sceneName) == false)
                {
                    if (sceneName.Contains(".json") == false)
                    {
                        sceneName = $"{sceneName}.json";
                    }

                    await _sceneSaver.SaveSceneAsJsonAsync(sceneName);
                    _popupController.HidePopup<TextInputPopup>();
                    _popupController.ShowPopup<InformationPopup>("Success!", "The File name was saved",
                        _ =>
                        {
                            _popupController.HidePopup<InformationPopup>();
                            moveToGameScene?.Invoke(sceneName);
                        });
                }
                else
                {
                    _popupController.HidePopup<TextInputPopup>();
                    _popupController.ShowPopup<InformationPopup>("Error!", "The File name can't be empty",
                        _ => { _popupController.HidePopup<InformationPopup>(); });
                }
            }, () => { _popupController.HidePopup<TextInputPopup>(); });
        }

        private void SubscribeToSpawnUiSignal()
        {
            _signal.GetStream<EditorSceneSignals.SpawnUiVisibilitySignal>().Subscribe(signal =>
            {
                _editorSceneUiView.SetSpawnUiVisibility(signal.IsVisible);
            }).AddTo(_compositeDisposable);
        }

        private void SubscribeToMenuButtonClick()
        {
            _editorSceneUiView.MenuButton.onClick.AsObservable().Subscribe(async _ =>
            {
                _signal.Fire<EditorSceneSignals.HideInspectorSignal>();
                await _sceneLoader.SwitchScene(Constants.EditorSceneName, Constants.MenuSceneName);
            }).AddTo(_compositeDisposable);
        }

        private void SubscribeToUndoButtonClick()
        {
            _editorSceneUiView.UndoButton.onClick.AsObservable().Subscribe(_ =>
            {
                _signal.Fire<EditorSceneSignals.HideInspectorSignal>();
                _commandManager.UndoLastCommand();
            }).AddTo(_compositeDisposable);
        }

        private void CreateSpawnerButtons()
        {
            for (int i = 0; i < _spawnAddressablesRegistry.SpawnableObjects.Length; i++)
            {
                var buttonData = _spawnAddressablesRegistry.SpawnableObjects[i];
                var spawnerButton = _spawnerButtonFactory.Create();
                spawnerButton.Initialize(_editorSceneUiView.SpawnerButtonHolder, buttonData.name, buttonData.thumbnail,
                    buttonData.assetAddress);
            }
        }
    }
}