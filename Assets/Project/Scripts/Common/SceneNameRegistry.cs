using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Common
{
    public interface ISceneNameRegistry
    {
        UniTask AddFileNameAsync(string fileName);
        UniTask<List<string>> GetFileNamesAsync();
        UniTask RemoveFileNameAsync(string fileName);
        
        UniTask<bool> HasFileNameAsync(string jsonFileName);
    }

    public class SceneNameRegistry : ISceneNameRegistry
    {
        private readonly string _registryFilePath;
        private List<string> _fileNames;

        public SceneNameRegistry()
        {
            _registryFilePath = Path.Combine(Application.persistentDataPath, Constants.SceneNameRegistryFileName);
            LoadFileNames().Forget();
        }

        public async UniTask AddFileNameAsync(string fileName)
        {
            if (!_fileNames.Contains(fileName))
            {
                _fileNames.Add(fileName);
                await SaveFileNamesAsync();
            }
            else
            {
                Debug.Log($"File name {fileName} - is already added");
            }
        }

        public async UniTask<List<string>> GetFileNamesAsync()
        {
            if (_fileNames == null)
            {
                await LoadFileNames();
            }
            return _fileNames;
        }

        public async UniTask RemoveFileNameAsync(string fileName)
        {
            _fileNames.Remove(fileName);
            await SaveFileNamesAsync();

            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private async UniTask LoadFileNames()
        {
            if (File.Exists(_registryFilePath))
            {
                var json = await File.ReadAllTextAsync(_registryFilePath);
                var sceneFileNameList = JsonUtility.FromJson<SceneFileNameList>(json);
                _fileNames = sceneFileNameList.fileNames;
            }
            else
            {
                _fileNames = new List<string>();
                await SaveFileNamesAsync();
            }
        }

        private async UniTask SaveFileNamesAsync()
        {
            var sceneFileNameList = new SceneFileNameList { fileNames = _fileNames };
            var json = JsonUtility.ToJson(sceneFileNameList);

            await using var streamWriter = new StreamWriter(_registryFilePath, false, Encoding.UTF8);
            await streamWriter.WriteAsync(json);
            Debug.Log($"File name registered at {_registryFilePath}");
        }
        public async UniTask<bool> HasFileNameAsync(string jsonFileName)
        {
            return _fileNames.Contains(jsonFileName);
        }
        
    }

    [System.Serializable]
    public class SceneFileNameList
    {
        public List<string> fileNames;
    }
}
