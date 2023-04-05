using Project.Scripts.EditorScene.Command;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.EditorScene.UI
{
    public class SpawnerButtonView : MonoBehaviour
    {
        [Inject] private CompositeDisposable _disposable;
        [Inject] private ICommandManager _commandManager;
        [Inject] private SpawnCommandFactory _spawnCommandFactory;
        
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Button button;
        
        public void Initialize(Transform parent, string text, Sprite image, string assetAddress)
        {
            transform.SetParent(parent);
            buttonText.text = text;
            buttonImage.sprite = image;

            button.OnClickAsObservable().Subscribe(_ =>
            {
                var spawnCommand = _spawnCommandFactory.Create(assetAddress);
                _commandManager.ExecuteCommand(spawnCommand);
                
            }).AddTo(_disposable);
        }
    }
}