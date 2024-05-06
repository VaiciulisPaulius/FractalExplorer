using UnityEngine;

public sealed class FractalInfo
{
    private static FractalInfo instance = null;
    public static FractalInfo Instance
    {
        get
        {
            if (instance == null)
            {
                Resolution resolution = new Resolution();

                resolution.x = Screen.width;
                resolution.y = Screen.height;

                instance = new FractalInfo(resolution);
            }
            return instance;
        }
    }

    public FractalInfo(Resolution resolution)
    {
        this.resolution = resolution;
    }
    [System.Serializable]
    public struct Resolution
    {
        public int x, y;
    }
    public Resolution resolution;
}
