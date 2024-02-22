using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoad : MonoBehaviour
{
    public static string image;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().sprite = LoadNewSprite(image);
    }
    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {

        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }
    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           
            if (Tex2D.LoadImage(FileData))           
                return Tex2D;                 
        }
        return null;                     
    }
}
