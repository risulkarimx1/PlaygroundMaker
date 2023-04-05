using UnityEngine;

namespace Project.Scripts.EditorScene.Interaction
{
    public class MouseInputHandler : BaseInputHandler
    {
        public override Vector3 TouchPosition => Input.mousePosition;

        public MouseInputHandler()
        {
            InitializeStreams(
                () => Input.GetMouseButtonDown(0),
                () => Input.GetMouseButton(0),
                () => Input.GetMouseButtonUp(0)
            );
        }
    }
}