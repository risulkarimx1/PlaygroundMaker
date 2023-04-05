using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.MenuScene
{
    public class SceneNameItemFactory : PlaceholderFactory<SceneNameItem>
    {
    }

    public class SceneNameItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sceneNameText;
        [SerializeField] private Button loadButton;

        public void Initialize(string sceneName, Transform parent, UnityAction onLoad)
        {
            var t = transform;
            t.SetParent(parent);
            t.localScale = Vector3.one;

            var pattern = @"^(.+)\.json$";
            var match = Regex.Match(sceneName, pattern);
            sceneName = match.Groups[1].Value;

            sceneNameText.text = sceneName;
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(onLoad);
        }
    }
}