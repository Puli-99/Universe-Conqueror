using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    [SerializeField] Transform shipTransform;  // Transform de la nave
    [SerializeField] RectTransform crosshair;  // Referencia al RectTransform del crosshair en el canvas
    [SerializeField] Canvas canvas;           // Canvas donde está el crosshair
    [SerializeField] float depthDistance = 50f;

    void Update()
    {
        UpdateCrosshairPosition();
    }

    void UpdateCrosshairPosition()
    {

        Vector3 forwardDirection = shipTransform.forward.normalized;
        Vector3 crosshairWorldPosition = shipTransform.position + forwardDirection * depthDistance;
        // Convierte la posición del crosshair al espacio del Canvas
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            Camera.main.WorldToScreenPoint(crosshairWorldPosition),
            canvas.worldCamera,
            out canvasPosition
        );

        // Asigna la nueva posición al crosshair
        crosshair.anchoredPosition = canvasPosition;
    }
}