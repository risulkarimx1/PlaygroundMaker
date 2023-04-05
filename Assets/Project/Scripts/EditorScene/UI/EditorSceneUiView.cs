using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EditorScene.UI
{
    public class EditorSceneUiView : MonoBehaviour
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private GameObject spawnButtonFrame;
        [SerializeField] private Transform spawnerButtonHolder;
        [SerializeField] private Button undoButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button playButton;

        public Button MenuButton => menuButton;

        public Transform SpawnerButtonHolder => spawnerButtonHolder;

        public Button UndoButton => undoButton;

        public Button SaveButton => saveButton;

        public Button PlayButton => playButton;

        public void SetSpawnUiVisibility(bool isVisible)
        {
            spawnButtonFrame.gameObject.SetActive(isVisible);
        }
    }
}