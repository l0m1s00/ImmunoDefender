using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldConfirmationButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Button Settings")]
    public float holdConfirmationTime;
    [Range(1f, 2f)] public float maxScaleMult;
    [HideInInspector] public UnityEvent onHoldCompleted;

    private bool isPointerDown = false;
    private float holdTimer = 0f;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isActionCompleted = false;

    private void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale * maxScaleMult;
    }

    private void Update()
    {
        if (isPointerDown && !isActionCompleted)
        {
            holdTimer += Time.deltaTime;

            float progress = holdTimer / holdConfirmationTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);

            if (holdTimer >= holdConfirmationTime)
            {
                isActionCompleted = true;
                onHoldCompleted?.Invoke();
                ResetButton();
            }
        }
    }

    //completar accion
    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        isActionCompleted = false;
        holdTimer = 0f;
    }

    //cancelar accion
    public void OnPointerUp(PointerEventData eventData)
    {
        ResetButton();
    }

    //raton sale del boton
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetButton();
    }

    private void ResetButton()
    {
        isPointerDown = false;
        holdTimer = 0f;
        transform.localScale = originalScale;
    }

    //seguridad si el panel desaparece, boton se resetea
    private void OnDisable()
    {
        ResetButton();
    }
}
