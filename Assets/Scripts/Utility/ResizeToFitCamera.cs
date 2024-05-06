using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeToFitCamera : MonoBehaviour
{
    private void Start()
    {
        Resize();
    }

    private void Resize()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the object.");
            return;
        }

        // Get the camera and its viewport size
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene.");
            return;
        }

        // Get the sprite size
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // Calculate the scaling factors
        float scale = ScreenInfo.GetCameraWidth() / spriteWidth;

        // Apply the scaling
        transform.localScale *= scale;
    }
}
