using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    void Update()
    {
        DisableToggle();
    }

    void DisableToggle()
    {
        var component = GetComponent<Toggle>();

        if (GameObject.Find("SelectedItems").GetComponent<SelectedItems>().count < 3) { component.enabled = true; return; } 
        if (component.isOn) return;
        component.enabled = false;

    }
}
