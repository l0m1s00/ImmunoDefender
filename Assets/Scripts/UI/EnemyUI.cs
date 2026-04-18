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

    private void Start()
    {
        investigationCircle.SetActive(false);
        outline.enabled = false;
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
}
