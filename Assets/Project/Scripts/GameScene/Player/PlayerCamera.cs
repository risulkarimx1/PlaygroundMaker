using Cinemachine;
using UnityEngine;

namespace Project.Scripts.GameScene.Player
{
    public class PlayerCamera: MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        public void Initialize(Transform target)
        {
            cinemachineVirtualCamera.Follow = target;
            cinemachineVirtualCamera.LookAt = target;
        }
    }
}