using UnityEngine;
using UnityEngine.UI;

public class PanelColor : MonoBehaviour
{
    #region Component
    [SerializeField] private Image backgroundImage;
    public Image BackgroundImage { get => backgroundImage; set => backgroundImage = value; }
    #endregion

    [SerializeField] private ColorSwatch colorSwatch;
    public ColorSwatch ColorSwatch { get => colorSwatch; set => colorSwatch = value; }

    private void Start()
    {
        if (colorSwatch != null)
        {
            SetImageColor(colorSwatch.GetRGB());
        }
    }

    public void SetImageColor(string value)
    {
        if (backgroundImage != null)
        {
            string colorValue = "#" + value;
            if (ColorUtility.TryParseHtmlString(colorValue, out Color color))
            {
                colorSwatch.ApplyToImage(backgroundImage);
            }
        }

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
        }

        if (colorSwatch != null)
        {
            SetImageColor(colorSwatch.GetRGBA());
        }
    }
#endif
}
