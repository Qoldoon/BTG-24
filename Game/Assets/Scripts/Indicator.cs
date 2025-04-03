using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Indicator : MonoBehaviour
{
    Image _image;
    private bool _isRunning;
    private float _currentTime;
    private float _time;
    Color _color;
    Coroutine _fillCoroutine;
    Coroutine _fadeCoroutine;

    void Start()
    {
        _image = GetComponent<Image>();
        _color = _image.color;
    }
    
    public void Fill(float duration)
    {
        if(_isRunning) return;
        Settle();
        _image.enabled = true;
        _isRunning = true;
        _fillCoroutine = StartCoroutine(FillOverTime(duration));
    }

    private IEnumerator FillOverTime(float duration)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            _image.fillAmount = elapsedTime / duration;
            
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        _image.fillAmount = 1;
        _isRunning = false;
        FadeOut(0.4f);
    }

    private void FadeOut(float duration = 1)
    {
        _fadeCoroutine = StartCoroutine(FadeOutOverTime(duration));
    }

    private IEnumerator FadeOutOverTime(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
        
            Color c = _image.color;
            c.a = 1 - t;
            _image.color = c;
            
            yield return null;
        }
        _image.enabled = false;
        _image.color = _color;
    }

    void Settle()
    {
        if(_fillCoroutine != null)
            StopCoroutine(_fillCoroutine);
        if(_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _image.color = _color;
        _image.enabled = false;
        _isRunning = false;
    }
    public void Stop()
    {
        StopCoroutine(_fillCoroutine);
        _image.color = new Color(1, 0.7f, 0, 1);
        FadeOut(0.2f);
        _isRunning = false;
    }
}
