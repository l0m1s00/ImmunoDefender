using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float smoothTime;
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSmoothTime;
    [SerializeField] private float rotationOffset;

    private Vector2 targetPos = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private float rotationVelocity;

    private RectTransform rectTransform;
    private RectTransform parentCanvasRect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null) parentCanvasRect = canvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        CursorMovement();
        CursorRotation();
    }

    private void CursorMovement()
    {
        targetPos = Inputs.Instance.mousePosition;
        Vector2 mouseScreenPos = Inputs.Instance.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvasRect,
            mouseScreenPos,
            null,
            out targetPos);

        rectTransform.anchoredPosition = Vector2.SmoothDamp(
            rectTransform.anchoredPosition, 
            targetPos, 
            ref velocity, 
            smoothTime, 
            Mathf.Infinity, 
            Time.unscaledDeltaTime);
    }

    private void CursorRotation()
    {
        Vector2 direction = targetPos - rectTransform.anchoredPosition;

        if (direction.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetAngle += rotationOffset;
            float currentAngle = rectTransform.eulerAngles.z;
            float smoothedAngle = Mathf.SmoothDampAngle(
                currentAngle, 
                targetAngle, 
                ref rotationVelocity, 
                rotationSmoothTime, 
                Mathf.Infinity, 
                Time.unscaledDeltaTime);

            transform.rotation = Quaternion.Euler(0f, 0f, smoothedAngle);
        }
    }
}
