using System.Xml.Serialization;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float edgeLimit;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomStep; //cantidad de zoom por movimiento de rueda
    [SerializeField] private float minCamSize;
    [SerializeField] private float maxCamSize;
    [SerializeField] private float zoomSmooth; //menor = mas rapido

    private Camera camera;
    private float targetCamSize;
    private float zoomVelocity;

    private void Awake()
    {
        camera = Camera.main;
        targetCamSize = camera.orthographicSize;
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
            targetCamSize -= Mathf.Sign(scroll) * zoomStep;
            targetCamSize = Mathf.Clamp(targetCamSize, minCamSize, maxCamSize);
        }

        if (Mathf.Abs(camera.orthographicSize - targetCamSize) > 0.01f)
        {
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, targetCamSize, ref zoomVelocity, zoomSmooth);
        }
    }

    private void MoveCamera()
    {
        Vector2 mousePos = Inputs.Instance.mousePosition;
        Vector3 moveDir = Vector3.zero;

        if (mousePos.y >= Screen.height - edgeLimit) moveDir.y += 1f;
        if (mousePos.y <= edgeLimit) moveDir.y -= 1f;
        if (mousePos.x >= Screen.width - edgeLimit) moveDir.x += 1f;
        if (mousePos.x <= edgeLimit) moveDir.x -= 1f;

        if (moveDir == Vector3.zero) return; //no hay movimiento de camara

        moveDir.Normalize();
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private Vector3 ClampCameraPosition(Vector3 targetPos)
    {
        //determinar cuanto "ve" la camara
        float camHeight = camera.orthographicSize;
        float camWidth = camera.orthographicSize * camera.aspect;

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
