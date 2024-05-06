using UnityEngine;
using UnityEngine.EventSystems;

public class FractalBehaviourHandler : MonoBehaviour
{
    Zoom fractalZoom;
    AGenerator fractalGenerator;

    private void Start()
    {
        fractalZoom = GetComponent<Zoom>();
        fractalGenerator = GetComponent<AGenerator>();
    }
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.CompareTag("Fractal"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (fractalGenerator.currentState.generatedTexture != null) fractalGenerator.AddToHistory();
                    fractalZoom.zoom();
                    fractalGenerator.Generate();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    fractalGenerator.Undo();
                }
            }
        }
    }
}
