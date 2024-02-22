using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{

    Slider slider;

    public float FillSpeed = 1f;
    float targetProgress = 0;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < targetProgress)
            slider.value += FillSpeed * Time.deltaTime;
        if (slider.value == 1) 
            slider.value = 0;
    }
    public void FillInTime(float time)
    {
        FillSpeed = 1/time;
        targetProgress = 1;
    }
}
