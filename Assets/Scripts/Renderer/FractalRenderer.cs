using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalRenderer : AFractalRenderer
{
    Texture2D texture;

    SpriteRenderer spriteRenderer;
    Sprite sprite;
    
    [Range(0.01f, 0.5f)] public float updateRate = 0.05f;

    [NonSerialized] public FractalInfo.Resolution resolution;


    private Queue<PixelRowData> pixelQueue = new Queue<PixelRowData>();

    bool trackRenderFinish;

    public override void AddToQueue(PixelRowData data)
    {
        lock (pixelQueue)
        {
            pixelQueue.Enqueue(data);
        }
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitRenderer();
        SubscribeToEvents();
        StartCoroutine(UpdateTexture());
    }
    void InitRenderer()
    {
        resolution = new FractalInfo.Resolution();
        resolution.x = Screen.width;
        resolution.y = Screen.height;

        FractalInfo.Instance.resolution = resolution;

        texture = new Texture2D(FractalInfo.Instance.resolution.x, FractalInfo.Instance.resolution.y);
        texture.filterMode = FilterMode.Point;

        Rect rectangle = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = Vector2.one * 0.5f;
        int pixelsPerUnit = texture.width;

        sprite = Sprite.Create(texture, rectangle, pivot, pixelsPerUnit);
        spriteRenderer.sprite = sprite;
    }
    void SubscribeToEvents()
    {
        ThreadPoolManager.Instance.OnJobCompleted += () => {
            trackRenderFinish = true;
        };
    }
    IEnumerator UpdateTexture()
    {
        while (true)
        {
            RenderFractal();
            yield return new WaitForSeconds(updateRate);
        }
    }
    public override void RenderFractal()
    {
        lock (pixelQueue)
        {
            while (pixelQueue.Count > 0)
            {
                PixelRowData rowData = pixelQueue.Dequeue();
                texture.SetPixels(0, rowData.y, rowData.colors.Length, 1, rowData.colors);
            }
            texture.Apply();

            if (trackRenderFinish)
            {
                trackRenderFinish = false;
                OnRenderFinish?.Invoke();
            }
        }
    }
    public override Texture2D GetRender()
    {
        return texture;
    }
    public override void SetRender(Texture2D texture)
    {
        this.texture.SetPixels(texture.GetPixels());
    }
    private void EmptyTexture()
    {
        Color[] colors = new Color[texture.width * texture.height];
        for(int i = 0; i < texture.width * texture.height; i++)
        {
            colors[i] = Color.black;
        }
        texture.SetPixels(0, 0, texture.width, texture.height, colors);
        texture.Apply();
    }
}
