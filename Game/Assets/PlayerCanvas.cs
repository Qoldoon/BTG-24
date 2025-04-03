using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCanvas : MonoBehaviour
{
    
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void CreateText(string text)
    {
        GameObject textObj = new GameObject("FloatingText");
        textObj.transform.SetParent(this.transform, false);
        
        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = 24;
        textComponent.color = Color.white;
        textComponent.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();

        float randomX = Random.Range(-20, 20);
        float randomY = Random.Range(-20, 20);
        Vector2 startPos = new Vector2(randomX, randomY);

        textRect.anchoredPosition = startPos;
        
        StartCoroutine(AnimateText(textRect, textComponent, startPos));
    }

    private IEnumerator AnimateText(RectTransform rect, TextMeshProUGUI text, Vector2 startPos)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 1;
            
            float newY = Mathf.Lerp(startPos.y, startPos.y + 100, t);
            float newX = startPos.x + (float)Math.Sin(t * 10f) * 20f;
            rect.anchoredPosition = new Vector2(newX, newY);
            
            Color color = text.color;
            color.a = Mathf.Lerp(1f, 0f, t);
            text.color = color;
            
            yield return null;
        }
        
        Destroy(rect.gameObject);
    }
}
