using UnityEngine;

[CreateAssetMenu(fileName = "GridCellObject", menuName = "Immuno Defence/Grid Cell")]
public class GridCellObject : ScriptableObject
{
    public enum CellType { 
        Path,
        Ground,
    }

    public CellType cellType;
    public GameObject cellPrefab;
    public int zRotation;
}
