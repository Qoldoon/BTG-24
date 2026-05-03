using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public int level;
    public Text name;
    public Text score;
    
    public Image previewImage;
    
    [SerializeField] private LevelData previewData;
    
    private string _jsonFile;

    MainMenu mainMenu;

    private void Awake()
    {
        mainMenu = FindAnyObjectByType<MainMenu>();
        Refresh();
    }
    
    public void SetLevel(int levelIndex, string displayName, string jsonFile, Sprite preview = null)
    {
        level     = levelIndex;
        _jsonFile = jsonFile;
        if (mainMenu == null)
            mainMenu = FindAnyObjectByType<MainMenu>();
        Refresh(displayName, preview);
    }

    void Refresh(string displayName = null, Sprite preview = null)
    {
        if (name  != null) name.text  = displayName ?? $"Level {level}";
        if (score != null) score.text = $"Highscore: {ScoreManager.ShowHighscore(level)}";

        if (previewImage != null)
        {
            Sprite sprite = preview;
            
            if (sprite == null && previewData != null && previewData.width > 0
                && previewData.entities != null && previewData.entities.Length > 0)
                sprite = LevelPreviewGenerator.Generate(previewData);
            
            if (sprite == null)
            {
                var asset = Resources.Load<TextAsset>($"LevelPreviews/level_{level}_layout");
                if (asset != null)
                {
                    var data = JsonUtility.FromJson<LevelData>(asset.text);
                    if (data != null && data.width > 0)
                        sprite = LevelPreviewGenerator.Generate(data);
                }
            }
            
            if (sprite == null)
                sprite = Resources.Load<Sprite>($"LevelPreviews/level_{level}");

            if (sprite != null)
            {
                previewImage.sprite         = sprite;
                previewImage.preserveAspect = false;
            }
        }
    }

    public void Load()
    {
        LevelSession.levelFile = _jsonFile;
        mainMenu.PlayLevel(level);
    }
}
