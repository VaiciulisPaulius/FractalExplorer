using UnityEngine;
using System;

public class FractalGenerator : AGenerator
{
    AFractalRenderer fractalRenderer;
    public ScreenConverter screenConverter;

    void Start()
    {
        InitGenerator();
        SubscribeToListeners();
        Generate();
    }
    void InitGenerator()
    {
        InitDimensions();
        fractalRenderer = GetComponent<AFractalRenderer>();
        screenConverter = new ScreenConverter(currentState);
        iterationInverse = 1 / (float) maxIterations;
    }
    void InitDimensions()
    {
        double minY = -1.5;
        double maxY = 1.5;

        double minX = minY * Camera.main.aspect;
        double maxX = maxY * Camera.main.aspect;

        currentState = new FractalState(minX, minY, maxX, maxY, maxIterations);
    }
    void SubscribeToListeners()
    {
        fractalRenderer.OnRenderFinish += () => {
            GetRenderedImageFromCurrentState();
            if (previousGenerations.Count == 0) AddToHistory();
        }; 
    }
    public override void Undo()
    {
        if (previousGenerations.Count == 1) return;
        FractalState state = previousGenerations.Pop();

        currentState.minX = state.minX;
        currentState.maxX = state.maxX;
        currentState.minY = state.minY;
        currentState.maxY = state.maxY;

        if (state.iterationsUsed == maxIterations)
        {
            currentState.generatedTexture = state.generatedTexture;
            fractalRenderer.SetRender(currentState.generatedTexture);
        }
        else
        {
            state.iterationsUsed = currentState.iterationsUsed;
            Generate(state);
        }
    }
    public override void Generate()
    {
        iterationInverse = 1 / (float)currentState.iterationsUsed;
        int key = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

        for (int y = 0; y < FractalInfo.Instance.resolution.y; y++)
        {
            ThreadPoolManager.Instance.QueueJob(FractalWorker, new object[] { y, currentState }, FractalInfo.Instance.resolution.y, key);
        }
    }
    public override void Generate(FractalState state)
    {
        iterationInverse = 1 / (float)state.iterationsUsed;
        int key = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        for (int y = 0; y < FractalInfo.Instance.resolution.y; y++)
        {
            ThreadPoolManager.Instance.QueueJob(FractalWorker, new object[] { y, state }, FractalInfo.Instance.resolution.y, key);
        }
    }
    void GetRenderedImageFromCurrentState()
    {
        Texture2D Clonedtexture = Texture2DUtility.CloneTexture(fractalRenderer.GetRender());
        currentState.generatedTexture = Clonedtexture;
    }
    void FractalWorker(object state)
    {
        object[] states = state as object[];

        int y = Convert.ToInt32(states[0]);
        FractalState fractalState = states[1] as FractalState;

        ScreenConverter scaledRef = new ScreenConverter(fractalState);
        FractalInfo.Resolution res = FractalInfo.Instance.resolution;

        double scaledY = scaledRef.ScreenToScaledY(y);

        Color[] colors = new Color[res.x];
        for (int x = 0; x < res.x; x++)
        {
            double scaledX = scaledRef.ScreenToScaledX(x);
            colors[x] = FractalCalculator.Calculate(scaledX, scaledY, fractalState.iterationsUsed, iterationInverse);
        }

        AFractalRenderer.PixelRowData pixelRowData = new AFractalRenderer.PixelRowData();
        pixelRowData.colors = colors;
        pixelRowData.y = y;

        fractalRenderer.AddToQueue(pixelRowData);
    }
}
