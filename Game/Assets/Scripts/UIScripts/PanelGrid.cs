using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PanelGrid : MonoBehaviour
{
    private List<Toggle> toggles = new();
    public List<GameObject> weapons;
    [SerializeField] private GameObject buttonPrefab;
    private Color goodColor = ColorScheme.SecondaryColor;
    private Color badColor = ColorScheme.PrimaryAccentColor;
    [SerializeField] public SelectedItems selection;

    private void Start()
    {
        foreach (var item in weapons)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, transform);
            
            var buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = item.name;
            }
            
            var button = buttonInstance.GetComponent<Toggle>();
            button.onValueChanged.AddListener(_ => OnButtonClick(item));
            toggles.Add(button);
        }
    }

    void OnButtonClick(GameObject clickedObject)
    {
        selection.TryAdd(clickedObject);
        foreach (var t in toggles)
        {
            var buttonColor = t.gameObject.GetComponent<ButtonColor>();
            if (t.isOn)
            {
                buttonColor.selectedColor = ColorScheme.ColorOption.Secondary;
                if(toggles.Count(x => x.isOn) > 3)
                    buttonColor.selectedColor = ColorScheme.ColorOption.Accent;
            }
            else buttonColor.selectedColor = ColorScheme.ColorOption.Tertiary;
            buttonColor.UpdateColor();
        }
    }
}
