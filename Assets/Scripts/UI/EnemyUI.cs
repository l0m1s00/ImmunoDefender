using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarFill;
    //[SerializeField] private SpriteRenderer outline;
    [SerializeField] private GameObject outline;

    [Header("BLymphocyte Investigation Components")]
    [SerializeField] private GameObject investigationCircle;
    [SerializeField] private Image investigationFill;

    [Header("Hit Flash Settings")]
    [SerializeField] private Transform visualRoot; //padre que contiene los distintos spriteRenderers que conforman el sprite
    [SerializeField] private Material whiteFlashMaterial;
    private float flashDuration = 0.1f;

    private SpriteRenderer[] allSprites;
    private Material[] originalMaterials;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        investigationCircle.SetActive(false);
        outline.SetActive(false);

        allSprites = visualRoot.GetComponentsInChildren<SpriteRenderer>();
        originalMaterials = new Material[allSprites.Length];
        for (int i = 0; i < allSprites.Length; i++)
        {
            originalMaterials[i] = allSprites[i].material;
        }
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
        outline.SetActive(true);
    }

    public void DeselectEnemy()
    {
        outline.SetActive(false);
    }

    public void HitFlash()
    {
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);

        if (gameObject.activeInHierarchy)
        {
            flashCoroutine = StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        //cambiar el material de los sprites a blanco
        for (int i = 0; i < allSprites.Length; i++)
        {
            if (allSprites[i] != null)
            {
                allSprites[i].material = whiteFlashMaterial;
            }
        }

        yield return new WaitForSeconds(flashDuration);

        //restaurar el sprite
        for (int i = 0; i < allSprites.Length; i++)
        {
            if (allSprites[i] != null)
            {
                allSprites[i].material = originalMaterials[i];
            }
        }
    }

    public void ResetUI()
    {
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);

        for (int i = 0; i < allSprites.Length; i++)
        {
            if (allSprites[i] != null)
            {
                allSprites[i].material = originalMaterials[i];
            }
        }
    }
}
