using System.Numerics;
using UnityEngine;

public static class FractalCalculator
{
    public static Color Calculate(double xCoord, double yCoord, int it, float invIt)
    {
        Complex C = new Complex(xCoord, yCoord);
        Complex iter = C;

        for (int i = 0; i <= it; i++)
        {
            double realSqr = iter.Real * iter.Real;
            double imgSqr = iter.Imaginary * iter.Imaginary;

            if (realSqr + imgSqr >= 4)
            {
                return Color.Lerp(Color.black, Color.white, invIt * i);
            }

            iter = Complex.Add(Complex.Multiply(iter, iter), C);
        }
        return Color.black;
    }
}
