using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerObject", menuName = "Immuno Defender/Tower Data")]
public class TowerDataSO : ScriptableObject
{
    public string towerName;
    public Sprite towerIcon;
    public int cost;
    //public GameObject towerPrefab;
    //public float range;
    //public float fireRate;
}
