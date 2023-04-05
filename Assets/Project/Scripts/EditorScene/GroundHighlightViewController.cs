using Project.Scripts.Common;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Scripts.EditorScene
{
    public class GroundHighlightViewController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private CompositeDisposable _disposable;
        
        [SerializeField] private MeshRenderer meshRenderer;

        private Transform _transform;
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _signalBus.GetStream<EditorSceneSignals.SpawnableObjectSelectedSignal>().Subscribe(signal =>
            {
                Show(signal.SpawnableObject.transform);
            }).AddTo(_disposable);
            _signalBus.GetStream<EditorSceneSignals.GroundSelectedSignal>().Subscribe(signal =>
            {
                Hide();
            }).AddTo(_disposable);
            
            _signalBus.GetStream<EditorSceneSignals.HideInspectorSignal>().Subscribe(signal =>
            {
                Hide();
            }).AddTo(_disposable);
            
            _signalBus.GetStream<EditorSceneSignals.ObjectDestroyedSignal>().Subscribe(signal =>
            {
                Hide();
            }).AddTo(_disposable);
        }

        private void Start()
        {
            Hide();
        }

        private void Show(Transform parent)
        {
            ResetScale();
            _transform.SetParent(parent);
            _transform.localPosition = new Vector3(0 , Constants.HighlightHeightFromGround, 0);
            _transform.localScale = Vector3.one * 9 / parent.transform.localScale.x;
            meshRenderer.enabled = true;
        }

        private void ResetScale()
        {
            _transform.SetParent(null);
            _transform.localScale = Vector3.one * 9;
        }

        private void Hide()
        {
            _transform.SetParent(null);
            meshRenderer.enabled = false;
        }
    }
}