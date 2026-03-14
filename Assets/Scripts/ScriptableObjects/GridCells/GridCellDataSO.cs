using UnityEngine;

[CreateAssetMenu(fileName = "GridCellObject", menuName = "Immuno Defender/Grid Cell")]
public class GridCellDataSO : ScriptableObject
{
    public enum CellType { 
        Path,
        Ground,
    }

    public CellType cellType;
    public GameObject cellPrefab;
    public int zRotation;
}
