using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    void Update()
    {
        DisableButton();
    }
    void DisableButton()
    {
        var component = GetComponent<Button>();

        if (GameObject.Find("SelectedItems").GetComponent<SelectedItems>().Count > 3) { component.interactable = false; return; }
        component.interactable = true;

    }
}
