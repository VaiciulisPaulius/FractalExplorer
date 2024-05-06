using UnityEngine;

public static class ScreenInfo
{
    public static float GetCameraHeight()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene.");
            return 0f;
        }

        return 2f * mainCamera.orthographicSize;
    }

    public static float GetCameraWidth()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene.");
            return 0f;
        }

        float cameraHeight = GetCameraHeight();
        return cameraHeight * mainCamera.aspect;
    }

    public static float GetScreenHeight()
    {
        return Screen.height;
    }

    public static float GetScreenWidth()
    {
        return Screen.width;
    }

    public static float GetAspectRatio()
    {
        return (float)Screen.width / Screen.height;
    }
    public static Vector2 ScreenToWorldCoords(Vector2 pos)
    {
        return Camera.main.ScreenToWorldPoint(pos);
    }
    public static Vector2 GetRealCoordsFromScreen(Vector2 pos)
    {
        Vector2 worldCoord = ScreenToWorldCoords(pos);
        return worldCoord;
    }
}
