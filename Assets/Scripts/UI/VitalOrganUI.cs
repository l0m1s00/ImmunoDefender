using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VitalOrganUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarValueFill;
    [SerializeField] private Image healthBarShadowFill;
    [SerializeField] private TMP_Text healthBarText;
    [SerializeField] private TMP_Text nameText;
    [Header("Settings")]
    [SerializeField] private float shadowDelayCooldown;
    [SerializeField] private float shadowShrinkSpeed;
    private float shadowDelayTimer;

    private void Start()
    {
        healthBarShadowFill.fillAmount = 1f;
    }

    private void Update()
    {
        if (healthBarShadowFill.fillAmount > healthBarValueFill.fillAmount)
        {
            shadowDelayTimer -= Time.deltaTime;

            if (shadowDelayTimer <= 0f) //igualar valores
            {
                healthBarShadowFill.fillAmount = Mathf.MoveTowards(
                    healthBarShadowFill.fillAmount,
                    healthBarValueFill.fillAmount,
                    shadowShrinkSpeed * Time.deltaTime
                );
            }
        }
    }

    public void InitializeVisuals(VitalOrganStats stats)
    {
        healthBarValueFill.fillAmount = 1f;
        nameText.text = stats.vitalOrganName;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float per = currentHealth / maxHealth;
        healthBarValueFill.fillAmount = per;
        healthBarText.text = $"{currentHealth} / {maxHealth}";
        shadowDelayTimer = shadowDelayCooldown;
    }
}
