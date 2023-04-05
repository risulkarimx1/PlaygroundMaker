using UnityEngine;

namespace Project.Scripts.EditorScene.Interaction
{
    public class CameraRig : MonoBehaviour
    {
        [SerializeField] private GameObject targetObject;

        public GameObject TargetObject => targetObject;
    }
}