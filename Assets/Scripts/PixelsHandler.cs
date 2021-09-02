using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PixelsHandler : MonoBehaviour
{
    public Slider radius;
    public Slider strength;
    public Toggle colorSwitch;
    public Toggle constantPrediction;

    private List<PixelHandler> pixels = new List<PixelHandler>();
    
    public GameObject pixelPrefab;
    public Transform pixelContainer;
    
    public PredictionClient client;

    public TextMeshProUGUI predictionText;
    private string prediction;

    private void Start()
    {
        for (var i = 0; i < 28*28; i++)
        {
            var newPixel = Instantiate(pixelPrefab, transform.position, Quaternion.identity);
            newPixel.transform.SetParent(pixelContainer, true);
            pixels.Add(newPixel.GetComponent<PixelHandler>());
        }
    }

    private void Update()
    {
        predictionText.text = prediction;

        if (!Input.GetMouseButton(0)) return;

        var mousePosition = Input.mousePosition;
        var pixelsToChange = Physics2D.OverlapCircleAll(mousePosition, radius.value * 5)
            .Where(p => p.GetComponent<PixelHandler>() != null).Select(p => p.GetComponent<PixelHandler>()).ToArray();
        
        if (constantPrediction.isOn && pixelsToChange.Length != 0) Predict();
        
        foreach (var pixel in pixelsToChange)
        {
            var pixelColor = pixel.GetColor();
            
            if (colorSwitch.isOn) pixel.ChangePixelColor(pixelColor - strength.value);
            else pixel.ChangePixelColor(pixelColor + strength.value);
        }
    }

    public void Reset()
    {
        foreach (var pixel in pixels) pixel.ChangePixelColor(colorSwitch.isOn ? 1 : 0);
        prediction = "";
    }

    public void PredictUI()
    {
        if (constantPrediction.isOn) return;
        Predict();
    }
    
    private void Predict()
    {
        var input = ReadPixels();
        client.Predict(input, output =>
        {
            var outputMax = output.Max();
            var maxIndex = Array.IndexOf(output, outputMax);
            prediction = "Prediction: " + Convert.ToChar(64 + maxIndex);
        }, error =>
        {
            // TODO: when i am not lazy
        });
    }

    public float[] ReadPixels() => pixels.Select(pixel => pixel.GetColor()).ToArray();
}