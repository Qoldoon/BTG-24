using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private static readonly int FaceColor = Shader.PropertyToID("_FaceColor");
    public UISlot[] slots;
    public Image fadeImage;
    public GameObject MenuPanel;
    
    public TMP_Text infoText;
    public float fps { get; private set; }
    private int _frameCount;
    private float _fpsTimer;

    public bool MenuUp;
    private void Awake()
    {
        if (fadeImage != null)
        {
            fadeImage.color = ColorScheme.SecondaryColor;
            fadeImage.enabled = true;
        }
        slots = GetComponentsInChildren<UISlot>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        _frameCount++;
        _fpsTimer += Time.unscaledDeltaTime;
        if (_fpsTimer >= 1f)
        {
            fps = _frameCount / _fpsTimer;
            _frameCount = 0;
            _fpsTimer = 0f;
            if (infoText != null) infoText.text = $"{fps:0} FPS";
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
            infoText.enabled = !infoText.enabled;

        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (MenuUp)
            ExitMenu();
        else
            EnterMenu();
    }
    public void EnterMenu()
    {
        MenuUp = true;
        if (fadeImage != null)
        {
            fadeImage.enabled = true;
            fadeImage.color = ColorScheme.TertiaryColor.MultiplyAlpha(0.5f);
        }
        if (MenuPanel != null) MenuPanel.SetActive(true);
        
        transform.parent.GetComponent<PlayerController>().freeze = true;
    }

    public void ExitMenu()
    {
        MenuUp = false;
        if (fadeImage != null) fadeImage.enabled = false;
        if (MenuPanel != null) MenuPanel.SetActive(false);
        transform.parent.GetComponent<PlayerController>().freeze = false;
    }

    public void Abandon()
    {
        Destroy(GameObject.Find("SelectedItems"));
        SceneManager.LoadScene("LevelSelect");
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color startColor = ColorScheme.TertiaryColor.MultiplyRGB(0.8f);
        Color endColor = ColorScheme.TertiaryColor.MultiplyAlpha(0);
        
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 1);
            if (fadeImage != null)
            {
                fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            }
            yield return null;
        }
        
        if (fadeImage != null)
        {
            fadeImage.color = endColor;
            fadeImage.enabled = false;
        }
    }
    public void SetIcons(PlayerInventory playerInventory)
    {
        for (int i = 0; i < playerInventory.slots.Count; i++)
        {
            slots[i].icon.sprite = playerInventory.slots[i].itemIcon;
            slots[i].icon.enabled = true;
        }
    }
    public void SetIcon(int index,  Sprite sprite)
    {
        slots[index].icon.sprite = sprite;
        slots[index].icon.enabled = true;
    }

    public void ClearIcon(int index)
    {
        slots[index].icon.sprite = null;
        slots[index].icon.enabled = false;
    }
    public void Select(int index)
    {
        Unselect();
        slots[index].ToggleOn();
    }

    public void Unselect()
    {
        foreach (UISlot slot in slots)
        {
            slot.ToggleOff();
        }
    }
    public void Unselect(int index)
    {
        slots[index].ToggleOff();
    }
    
    public void ShowDeathBlur()
    {
        if (transform.Find("DeathBlur") != null) return;

        GameObject blurObj = new GameObject("DeathBlur");
        blurObj.transform.SetParent(transform, false);

        Image blurImage = blurObj.AddComponent<Image>();
        blurImage.raycastTarget = false;
        blurImage.color = ColorScheme.HexToRGB("#292121");
        Sprite[] blurSprites = Resources.LoadAll<Sprite>("Textures/Blur");
        if (blurSprites.Length > 0) blurImage.sprite = blurSprites[0];

        RectTransform blurRect = blurObj.GetComponent<RectTransform>();
        blurRect.anchorMin = Vector2.zero;
        blurRect.anchorMax = Vector2.one;
        blurRect.offsetMin = Vector2.zero;
        blurRect.offsetMax = Vector2.zero;
    }

    public void TitleText(string text)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(this.transform, false);
        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = 100;
        textComponent.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Saira_Condensed-Bold SDF");
        textComponent.enableVertexGradient = true;
        textComponent.colorGradient = new VertexGradient(new Color(156, 31, 56));
        textComponent.color = new Color(156, 31, 56);
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.textWrappingMode = TextWrappingModes.NoWrap;
        textComponent.ForceMeshUpdate();
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        
        Vector2 startPos = new Vector2(0, 0);

        textRect.anchoredPosition = startPos;
    }
    
    public void SubTitleText(string text)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(this.transform, false);
        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = 90;
        textComponent.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Saira_Condensed-Regular SDF");
        textComponent.enableVertexGradient = true;
        textComponent.colorGradient = new VertexGradient(new Color(156, 31, 56));
        textComponent.color = new Color(156, 31, 56);
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.textWrappingMode = TextWrappingModes.NoWrap;
        textComponent.ForceMeshUpdate();
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        
        Vector2 startPos = new Vector2(0, -85);

        textRect.anchoredPosition = startPos;
    }

}
