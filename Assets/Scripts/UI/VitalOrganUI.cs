using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VitalOrganUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TMP_Text healthBarText;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float per = currentHealth / maxHealth;
        healthBarFill.fillAmount = per;
        healthBarText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
