using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClockManager : MonoBehaviour
{
    public TMP_Text clockTimer;

    void Update()
    {
        clockTimer.text = "Time : " + Mathf.Round(Time.timeSinceLevelLoad) + " s";
    }
}
