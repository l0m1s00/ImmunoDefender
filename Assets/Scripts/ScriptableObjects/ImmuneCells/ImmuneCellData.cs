using UnityEngine;

public abstract class ImmuneCellData : ScriptableObject
{
    [Header("General")]
    public GameObject immuneCellPrefab;
    public string cellName;
    public float speed;
}
