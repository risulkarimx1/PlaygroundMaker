using UnityEngine;

namespace Project.Scripts.EditorScene.Interaction
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Project/Editor Scene Camera Config")]
    public class EditorSceneCameraConfig : ScriptableObject
    {
        [SerializeField] private float rotationSpeed = 5.0f;
        [SerializeField] private float pinchZoomSpeed = 5.0f;
        [SerializeField] private float smoothDamping = 1.0f;

        public float RotationSpeed => rotationSpeed;

        public float PinchZoomSpeed => pinchZoomSpeed;

        public float SmoothDamping => smoothDamping;
    }
}