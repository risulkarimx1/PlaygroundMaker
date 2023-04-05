using UnityEngine;

namespace Project.Scripts.EditorScene
{
    public class EditorSceneSignals
    {
        public class SpawnableObjectSelectedSignal
        {
            public GameObject SpawnableObject { get; set; }
        }

        public class GroundSelectedSignal
        {
            public Vector3 MousePosition { get; set; }
        }   
        public class TouchedOnUiSignal
        {
            
        } 
        
        public class SelectionReleaseSignal
        {
        
        }
        
        public class ObjectSpawnedSignal
        {
            public GameObject SpawnedObject { get; set; }
            public string AssetAddress { get; set; }
        }
        
        public class ObjectDestroyedSignal
        {
            public GameObject DestroyedObject { get; set; }
        }

        public class HideInspectorSignal
        {
        }

        public class SpawnUiVisibilitySignal
        {
            public bool IsVisible { get; set; }
        }
    }
}