using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private TextMeshProUGUI descriptionText;

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
            button.GetComponent<Description>().SetDescription(item.GetComponent<Item>().Description);
            toggles.Add(button);
        }

        Descriptions();
    }

    void Descriptions()
    {
        foreach (var button in toggles)
        {
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.gameObject.AddComponent<EventTrigger>();
            
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            pointerEnter.callback.AddListener((eventData) => SetDescription(button.GetComponent<Description>().value));
            trigger.triggers.Add(pointerEnter);
            
            EventTrigger.Entry pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            pointerExit.callback.AddListener((eventData) => ResetDescription());
            trigger.triggers.Add(pointerExit);
        }
    }
    
    private void SetDescription(string description)
    {
        if (descriptionText != null)
            descriptionText.text = description;
    }

    private void ResetDescription()
    {
        if (descriptionText != null)
            descriptionText.text = "";
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
