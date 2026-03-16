using UnityEngine;

[CreateAssetMenu(fileName = "MacrophageImmuneCellData", menuName = "Immuno Defender/Immune Cells/Macrophage Immune Cell Data")]
public class MacrophageImmuneCellData : AttackerImmuneCellData
{
    [Header("Delete Toxic Waste")]
    public float deleteToxicWasteCooldown;
}

