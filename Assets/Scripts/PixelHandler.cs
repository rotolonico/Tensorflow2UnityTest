using UnityEngine;
using UnityEngine.UI;

public class PixelHandler : MonoBehaviour
{
    private Image image;

    private void Start() => image = GetComponent<Image>();

    public float GetColor() => image.color.r;

    public void ChangePixelColor(float color)
    {
        if (color >= 1) color = 1;
        else if (color <= 0) color = 0;
        
        image.color = new Color(color, color, color);
    }
}
