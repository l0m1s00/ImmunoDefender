using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private VitalOrganData liverVitalOrganData;

    private VitalOrgan vitalOrganOnScene = null;

    private void OnEnable()
    {
        Enemy.OnAttackVitalOrgan += AttackVitalOrgan;
        NegativeStates.OnAttackVitalOrgan += AttackVitalOrgan;
    }

    private void OnDisable()
    {
        Enemy.OnAttackVitalOrgan -= AttackVitalOrgan;
        NegativeStates.OnAttackVitalOrgan -= AttackVitalOrgan;
    }

    private void Start()
    {
        InitializeVitalOrgan();
    }

    private void InitializeVitalOrgan()
    {
        //tomar un organo vital para posicionarlo en el mapa
        float x = DataLibrary.Instance.GetSelectedDifficultyData().width - 2.5f;
        float y = DataLibrary.Instance.GetSelectedDifficultyData().height / 2;
        Vector2 pos = new Vector2(x, y);
        GameObject newVitalOrgan = PoolEntitiesManager.Instance.SpawnVitalOrgan(liverVitalOrganData);
        newVitalOrgan.transform.position = pos;

        vitalOrganOnScene = newVitalOrgan.GetComponent<VitalOrgan>();
        vitalOrganOnScene.Initialize(liverVitalOrganData);
    }

    private void AttackVitalOrgan(float damage)
    {
        vitalOrganOnScene.TakeDamage(damage);
    }
}
