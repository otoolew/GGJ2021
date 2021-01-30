using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Color Swatch", fileName = "newColorSwatch")]
public class ColorSwatch : ScriptableObject
{
    [Header("Color Name")]
    [SerializeField] private string colorName;
    public string ColorName { get => colorName; set => colorName = value; }

    [SerializeField] private Color color;
    public Color Color { get => color; set => color = value; }

    public Color GetColor()
    {
        return color;
    }
    public string GetRGB()
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }
    public string GetRGBA()
    {
        return ColorUtility.ToHtmlStringRGBA(color);
    }

    public void ApplyToImage(Image image)
    {
        image.color = color;
    }
    public void ApplyToText(Image image)
    {
        image.color = color;
    }
}
