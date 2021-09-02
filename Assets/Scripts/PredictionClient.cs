using System;
using UnityEngine;

public class PredictionClient : MonoBehaviour
{
    private PredictionRequester predictionRequester;

    private void Start() => InitializeServer();

    public void InitializeServer()
    {
        predictionRequester = new PredictionRequester();
        predictionRequester.Start();
    }

    public void Predict(float[] input, Action<float[]> onOutputReceived, Action<Exception> fallback)
    {
        predictionRequester.SetOnTextReceivedListener(onOutputReceived, fallback);
        predictionRequester.SendInput(input);
    }

    private void OnDestroy()
    {
        predictionRequester.Stop();
    }
}