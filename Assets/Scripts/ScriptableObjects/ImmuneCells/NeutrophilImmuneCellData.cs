using UnityEngine;

[CreateAssetMenu(fileName = "NeutrophilImmuneCellData", menuName = "Immuno Defender/Immune Cells/Neutrophil Immune Cell Data")]
public class NeutrophilImmuneCellData : AttackerImmuneCellData
{
    public int maxAttacksCapacity; //cantidad total de ataques que puede realizar antes de morir
}
