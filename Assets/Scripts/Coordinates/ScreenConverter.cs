using UnityEngine;

public class ScreenConverter
{
    FractalState scaledRef;

    public ScreenConverter(FractalState scaledRef)
    {
        this.scaledRef = scaledRef;
    }

    public Vector2 ScreenToScaled(Vector2 screen)
    {
        double scaledX = ScreenToScaledX((int)screen.x);
        double scaledY = ScreenToScaledY((int)screen.y);

        return new Vector2((float)scaledX, (float)scaledY);
    }
    public double ScreenToScaledX(int x)
    {
        double scaledX = (x - 0) * ((scaledRef.maxX - scaledRef.minX) /
            (FractalInfo.Instance.resolution.x - 0)) + scaledRef.minX;

        return scaledX;
    }
    public double ScreenToScaledY(int y)
    {
        double scaledY = (y - 0) * ((scaledRef.maxY - scaledRef.minY) /
            (FractalInfo.Instance.resolution.y - 0)) + scaledRef.minY;

        return scaledY;
    }
}
