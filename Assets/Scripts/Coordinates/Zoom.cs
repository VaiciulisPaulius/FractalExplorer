using UnityEngine;

public class Zoom : MonoBehaviour
{
    double zoomFactor = 2;

    ScreenConverter screenConverter;
    FractalState state;

    void Start()
    {
        state = GetComponent<FractalGenerator>().currentState;
        screenConverter = GetComponent<FractalGenerator>().screenConverter;
    }
    public void zoom()
    {
        Vector3 mousePos = Input.mousePosition;

        state.minX /= zoomFactor;
        state.maxX /= zoomFactor;
        state.minY /= zoomFactor;
        state.maxY /= zoomFactor;

        double Xoffset = screenConverter.ScreenToScaledX((int)mousePos.x);
        double Yoffset = screenConverter.ScreenToScaledY((int)mousePos.y);

        state.minX += Xoffset;
        state.maxX += Xoffset;
        state.minY += Yoffset;
        state.maxY += Yoffset;
    }
}
