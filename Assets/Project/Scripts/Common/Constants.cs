namespace Project.Scripts.Common
{
    public static class Constants
    {
        public const string GameSceneName = "GameScene";
        public const string MenuSceneName = "MenuScene";
        public const string EditorSceneName = "EditorScene";
        public const int GroundLayer = 1 << 6;
        public const int SpawnableLayer = 1 << 7;

        public const string EditorSceneCameraConfigPath = "Data/CameraConfig";
        public const string SpawnableItemRegistryPath = "Data/SpawnAddressablesRegistry";
        public const float TransformCommandTolerance = 0.01f;
        
        public const float SceneScale = 1000;
        public const float HighlightHeightFromGround = 0.01f;
        public const string PlayerIsWalkingAnimationKey = "IsWalking";
        public const int FakeDelayBecauseIloveIt = 1000;
        public const string SceneNameRegistryFileName = "SceneNameRegistry.json";
    }
}