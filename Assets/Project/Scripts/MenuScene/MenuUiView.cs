using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.MenuScene
{
    public class MenuUiView : MonoBehaviour
    {
        [SerializeField] private Button buildGameButton;
        [SerializeField] private Transform sceneNameListContainer;

        private void Awake()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }

        public Button BuildGameButton => buildGameButton;

        public Transform SceneNameListContainer => sceneNameListContainer;
    }
}
