using UnityEngine;

[CreateAssetMenu(fileName = "VitalOrganData", menuName = "Immuno Defender/Vital Organ Data")]
public class VitalOrganData : ScriptableObject
{
    public int health;
    public string vitalOrganName;
    public GameObject vitalOrganPrefab;
}
