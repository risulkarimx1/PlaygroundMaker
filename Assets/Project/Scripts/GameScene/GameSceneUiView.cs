using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.GameScene
{
    public class GameSceneUiView : MonoBehaviour
    {
        [SerializeField] private Button menuButton;

        public Button MenuButton => menuButton;
    }
}