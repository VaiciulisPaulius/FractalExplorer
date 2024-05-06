using System;
using UnityEngine;

public abstract class AFractalRenderer : MonoBehaviour
{
    public Action OnRenderFinish;
    public struct PixelRowData
    {
        public Color[] colors;
        public int y;
    }
    public abstract void RenderFractal();
    public abstract void AddToQueue(PixelRowData data);
    public abstract Texture2D GetRender();
    public abstract void SetRender(Texture2D texture);
}
