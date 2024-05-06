using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Texture2DUtility
{
    public static Texture2D CloneTexture(Texture2D src)
    {
        Texture2D texture = new Texture2D(src.width, src.height, src.format, src.mipmapCount > 1);
        Graphics.CopyTexture(src, texture);

        return texture;
    }
}
