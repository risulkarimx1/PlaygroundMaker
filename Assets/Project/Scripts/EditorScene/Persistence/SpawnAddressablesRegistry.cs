using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.EditorScene.Persistence
{
    [Serializable]
    public class SpawnableItem
    {
        public string name;
        public string assetAddress;
        public Sprite thumbnail;
    }
    
    [CreateAssetMenu(fileName = "SpawnAddressablesRegistry", menuName = "Project/Spawn Addressables Registry")]
    public class SpawnAddressablesRegistry : ScriptableObject
    {
        [SerializeField] private SpawnableItem[] spawnableObjects;

        private Dictionary<string, string> _addressToNameMap = new();

        public SpawnableItem[] SpawnableObjects => spawnableObjects;

        private void OnEnable()
        {
            _addressToNameMap = new Dictionary<string, string>();
            
            for (int i = 0; i < spawnableObjects.Length; i++)       
            {
                _addressToNameMap.Add(spawnableObjects[i].assetAddress, spawnableObjects[i].name);
            }
        }

        public string GetNameFromAddress(string address)
        {
            return _addressToNameMap[address];
        }
    }
}