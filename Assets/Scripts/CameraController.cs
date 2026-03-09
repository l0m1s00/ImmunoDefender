using System.Xml.Serialization;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float edgeLimit;

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
        /*if (Inputs.Instance.isLeftClicking)*/ MoveCamera();
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
        Vector2 mousePos = Inputs.Instance.mousePosition;
        Vector3 moveDir = Vector3.zero;

        //1. Movimiento moviendo el raton a los limites de la pantalla
        if (mousePos.y >= Screen.height - edgeLimit) moveDir.y += 1f;
        if (mousePos.y <= edgeLimit) moveDir.y -= 1f;
        if (mousePos.x >= Screen.width - edgeLimit) moveDir.x += 1f;
        if (mousePos.x <= edgeLimit) moveDir.x -= 1f;

        //2. Movimiento arrastrando en direccion contraria - requiere <if (Inputs.Instance.isLeftClicking) MoveCamera();>
        //if (mousePos.y >= Screen.height - edgeLimit) moveDir.y -= 1f;
        //if (mousePos.y <= edgeLimit) moveDir.y += 1f;
        //if (mousePos.x >= Screen.width - edgeLimit) moveDir.x -= 1f;
        //if (mousePos.x <= edgeLimit) moveDir.x += 1f;

        if (moveDir == Vector3.zero) return; //no hay movimiento de camara

        moveDir.Normalize();

        //para que la velocidad de desplazamiento de la camara sea proporcional zoom, mas zoom menor velocidad
        //calcular valor de 0 a 1 en base al zoom min y max, 0 minZoom 1 maxZoom
        float zoomPer = Mathf.InverseLerp(minCamSize, maxCamSize, mainCamera.orthographicSize);
        //calcular velocidad dependiendo del zoom
        float speed = Mathf.Lerp(minMoveSpeed, maxMoveSpeed, zoomPer);

        transform.position += moveDir * speed * Time.deltaTime;
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
