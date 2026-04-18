using UnityEngine;
using System.Collections;

public abstract class AnimationsUI : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField][Range(1f, 2f)] protected float onMouseScaleFactor;
    [SerializeField] protected float verticalOffset;
    [SerializeField] protected float animationDuration;
    [SerializeField][Range(1f, 2f)] protected float selectedScaleFactor;
    [SerializeField][Range(1f, 2f)] protected float hardSelectedScaleFactor;
    [SerializeField] protected float selectedUpOffset;
    [SerializeField] protected float hardSelectedUpOffset;
    [SerializeField] protected int selectedOrderInLayer;

    //animacion
    protected Vector3 originalScale;
    protected Vector3 originalPos;
    protected Quaternion originalRot;
    protected RectTransform rectTransform;
    protected Coroutine currentAnimationCoroutine;
    protected int originalOrderInLayer;

    protected void AnimateHover(bool isHovering)
    {
        Vector3 targetScale = isHovering ? originalScale * onMouseScaleFactor : originalScale;
        Vector3 targetPos = isHovering ? originalPos + new Vector3(0f, verticalOffset, 0f) : originalPos;
        StartAnimation(targetScale, targetPos, originalRot, false);
    }

    protected void AnimateSelected(bool isHardSelected)
    {
        Vector3 targetScale = originalScale * selectedScaleFactor;
        Vector3 targetPos = originalPos + new Vector3(0f, selectedUpOffset, 0f);
        StartAnimation(targetScale, targetPos, Quaternion.identity, isHardSelected);
    }

    protected void AnimateDeselected()
    {
        StartAnimation(originalScale, originalPos, originalRot, false);
    }

    private void StartAnimation(Vector3 targetScale, Vector3 targetPos, Quaternion targetRot, bool isHardAnimation)
    {
        if (currentAnimationCoroutine != null) StopCoroutine(currentAnimationCoroutine);
        currentAnimationCoroutine = StartCoroutine(AnimateItemUI(targetScale, targetPos, targetRot, isHardAnimation));
    }

    private IEnumerator AnimateItemUI(Vector3 targetScale, Vector3 targetPos, Quaternion targetRot, bool isHardAnimation)
    {
        float time = 0f;
        Vector3 startScale = rectTransform.localScale;
        Vector3 startPos = rectTransform.localPosition;
        Quaternion startRot = rectTransform.localRotation;

        if (isHardAnimation) //antes de hacer la animacion especificada, hace la misma animación pero con los valores aumentados para terminar en los valores de la animacion reales
        {
            Vector3 hardTargetScale = originalScale * hardSelectedScaleFactor;
            Vector3 hardTargetPos = originalPos + new Vector3(0f, hardSelectedUpOffset, 0f);

            while (time < animationDuration)
            {
                //porcentaje de animacion
                float per = time / animationDuration;
                //movimiento suave con aceleracion y frenado
                float smooth = Mathf.SmoothStep(0f, 1f, per);

                rectTransform.localScale = Vector3.Lerp(startScale, hardTargetScale, smooth);
                rectTransform.localPosition = Vector3.Lerp(startPos, hardTargetPos, smooth);
                rectTransform.localRotation = Quaternion.Lerp(startRot, targetRot, smooth);

                time += Time.deltaTime;
                yield return null;
            }

            startScale = rectTransform.localScale;
            startPos = rectTransform.localPosition;
            time = 0f;
        }

        while (time < animationDuration)
        {
            //porcentaje de animacion
            float per = time / animationDuration;
            //movimiento suave con aceleracion y frenado
            float smooth = Mathf.SmoothStep(0f, 1f, per);

            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, smooth);
            rectTransform.localPosition = Vector3.Lerp(startPos, targetPos, smooth);
            rectTransform.localRotation = Quaternion.Lerp(startRot, targetRot, smooth);

            time += Time.deltaTime;
            yield return null;
        }

        //al finalizar, valores exactos
        rectTransform.localScale = targetScale;
        rectTransform.localPosition = targetPos;
        rectTransform.localRotation = targetRot;
        currentAnimationCoroutine = null;
    }
}
