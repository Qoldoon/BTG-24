using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    public RawImage image;
    public Image icon;
    void Start()
    {
        image = GetComponent<RawImage>();
        icon = GetComponentInChildren<Image>();
    }

}
