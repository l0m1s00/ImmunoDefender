using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private SpriteRenderer outline;
    [Header("BLymphocyte Investigation Components")]
    [SerializeField] private GameObject investigationCircle;
    [SerializeField] private Image investigationFill;
    [Header("Hit Flash Settings")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Material whiteFlashMaterial;
    private float flashDuration = 0.1f;

    private Material originalMaterial;
    private Coroutine flashCoroutine;

    private void Start()
    {
        investigationCircle.SetActive(false);
        outline.enabled = false;
        originalMaterial = sprite.material;
    }

    public void UpdateHealthBar(float healthPer)
    {
        healthBarFill.fillAmount = healthPer;
    }

    public void UpdateInvestigationFill(float amount)
    {
        if (!investigationCircle.activeSelf) investigationCircle.SetActive(true);

        investigationFill.fillAmount = amount;
    }

    public void DeactivateInvestigationFill()
    {
        investigationCircle.SetActive(false);
    }

    public void SelectEnemy()
    {
        outline.enabled = true;
    }

    public void DeselectEnemy()
    {
        outline.enabled = false;
    }

    public void HitFlash()
    {
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        sprite.material = whiteFlashMaterial;
        yield return new WaitForSeconds(flashDuration);
        sprite.material = originalMaterial;
    }

    public void ResetUI()
    {
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);

        sprite.material = originalMaterial;
    }
}
