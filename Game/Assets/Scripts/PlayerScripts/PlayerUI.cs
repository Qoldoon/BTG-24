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
    
    public void TitleText(string text)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(this.transform, false);
        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = 100;
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
