using UnityEngine;

public class FractalState
{

    public double minY;
    public double maxY;
    public double minX;
    public double maxX;

    public Texture2D generatedTexture;
    public int iterationsUsed;

    public FractalState(double minX, double minY, double maxX, double maxY, int iterationsUsed)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
        this.iterationsUsed = iterationsUsed;
    }
    public FractalState(double minX, double minY, double maxX, double maxY, int iterationsUsed, Texture2D texture)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
        this.iterationsUsed = iterationsUsed;
        generatedTexture = texture;
    }

    public FractalState Clone()
    {
        FractalState clone = new FractalState(minX, minY, maxX, maxY, iterationsUsed, Texture2DUtility.CloneTexture(generatedTexture));
        return clone;
    }
}
