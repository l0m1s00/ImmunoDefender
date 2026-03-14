using UnityEngine;

[CreateAssetMenu(fileName = "NeutrophilImmuneCellData", menuName = "Immuno Defender/Immune Cells/Neutrophil Immune Cell Data")]
public class NeutrophilImmuneCellData : AttackerImmuneCellData
{
    public float maxDamageCapacity; //cantidad total de dano que puede infligir antes de morir
}
