using UnityEngine;

namespace Project.Scripts.GameScene.Player
{
    public class PlayerView: MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Rigidbody playerRigidbody;

        public Transform PlayerTransform => playerTransform;

        public Animator PlayerPlayerAnimator => playerAnimator;

        public float MoveSpeed => moveSpeed;

        public float RotationSpeed => rotationSpeed;

        public Rigidbody PlayerRigidbody => playerRigidbody;
    }
}
