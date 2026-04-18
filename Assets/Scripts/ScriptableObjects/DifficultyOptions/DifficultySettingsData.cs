using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettingsData", menuName = "Immuno Defender/DifficultySettings")]
public class DifficultySettingsData : ScriptableObject
{
    [Header("General Components")]
    public string title;
    public string description;
    public string features;
    [Header("Map Size Settings")]
    public int width;
    public int height;
    [Range(1, 100)] public int groundPercentage;
    public int minPathSize;
    //[Header("Enemy Settings")]
    [Header("Initial Currency")]
    public int initialCurrency;
}
