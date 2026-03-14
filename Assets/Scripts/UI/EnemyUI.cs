using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarFill;

    public void UpdateHealthBar(float healthPer)
    {
        healthBarFill.fillAmount = healthPer;
    }
}
