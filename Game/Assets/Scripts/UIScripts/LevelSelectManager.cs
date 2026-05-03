using System.IO;
using System.Linq;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    [Tooltip("The LevelSelect prefab")]
    public GameObject buttonPrefab;

    [Tooltip("Parent transform for the spawned buttons")]
    public Transform container;

    void Start()
    {
        if (buttonPrefab == null || container == null)
        {
            Debug.LogWarning("LevelSelectManager: buttonPrefab or container not assigned.");
            return;
        }

        var jsonFiles = Directory.GetFiles(Application.streamingAssetsPath, "*.json")
            .Select(path => new { path, fileName = Path.GetFileName(path) })
            .Select(f =>
            {
                try
                {
                    var data = JsonUtility.FromJson<LevelData>(File.ReadAllText(f.path));
                    return (data, f.fileName);
                }
                catch
                {
                    Debug.LogWarning($"LevelSelectManager: could not parse {f.fileName}, skipping.");
                    return (null, f.fileName);
                }
            })
            .Where(t => t.data != null)
            .OrderBy(t => t.data.levelIndex)
            .ToList();

        foreach (var (data, fileName) in jsonFiles)
        {
            var go = Instantiate(buttonPrefab, container);
            var btn = go.GetComponent<LevelSelect>();
            if (btn == null)
            {
                Debug.LogWarning("LevelSelectManager: buttonPrefab is missing a LevelSelect component.");
                continue;
            }
            var preview = LevelPreviewGenerator.Generate(data);
            btn.SetLevel(data.levelIndex, data.name, fileName, preview);
        }
    }
}
