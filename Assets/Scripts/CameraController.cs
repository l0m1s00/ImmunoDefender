using System.Xml.Serialization;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Movement Settings
    private Vector3 dragOrigin;
    private bool isDragging = false;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomStep; //cantidad de zoom por movimiento de rueda
    [SerializeField] private float minCamSize;
    [SerializeField] private float maxCamSize;
    [SerializeField] private float zoomSmooth; //menor = mas rapido

    private Camera mainCamera;
    private float camSize;
    private float zoomVelocity;

    private void Awake()
    {
        mainCamera = Camera.main;
        camSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        CameraZoom();
        MoveCamera();
        transform.position = ClampCameraPosition(transform.position);
    }

    private void CameraZoom()
    {
        float scroll = Inputs.Instance.scrollDelta;

        if (scroll != 0f)
        {
            camSize -= Mathf.Sign(scroll) * zoomStep;
            camSize = Mathf.Clamp(camSize, minCamSize, maxCamSize);
        }

        if (Mathf.Abs(mainCamera.orthographicSize - camSize) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, camSize, ref zoomVelocity, zoomSmooth);
        }
    }

    private void MoveCamera()
    {
        if (Inputs.Instance.isWheelClicking)
        {
            if (!isDragging)
            {
                dragOrigin = mainCamera.ScreenToWorldPoint(Inputs.Instance.mousePosition);
                isDragging = true;
            }
            else
            {
                Vector3 currentMousePos = mainCamera.ScreenToWorldPoint(Inputs.Instance.mousePosition);
                Vector3 mouseDifference = dragOrigin - currentMousePos;
                mouseDifference.z = 0f;
                transform.position += mouseDifference;
            }
        }
        else
        {
            isDragging = false;
        }
    }

    private Vector3 ClampCameraPosition(Vector3 targetPos)
    {
        //determinar cuanto "ve" la camara
        float camHeight = mainCamera.orthographicSize;
        float camWidth = mainCamera.orthographicSize * mainCamera.aspect;

        //definir limites del mapa
        float minX = -0.5f;
        float maxX = GridManager.Instance.GetMapWidht() - 0.5f;
        float minY = -0.5f;
        float maxY = GridManager.Instance.GetMapHeight() - 0.5f;

        //para que el borde de la pantalla sea el borde del mapa
        float clampX;
        float clampY;

        //si la camara se encuentra en los margenes del mapa y se intenta hacer zoomout, se empuja hacia el centro para que no se vea fuera de los limites
        if (camWidth * 2f > (maxX - minX))
        {
            clampX = minX + (maxX - minX) / 2f;
        }
        else
        {
            clampX = Mathf.Clamp(targetPos.x, minX + camWidth, maxX - camWidth);
        }
        if (camHeight * 2f > (maxY - minY))
        {
            clampY = minY + (maxY - minY) / 2f;
        }
        else
        {
            clampY = Mathf.Clamp(targetPos.y, minY + camHeight, maxY - camHeight);
        }

        return new Vector3(clampX, clampY, targetPos.z);
    }
}
