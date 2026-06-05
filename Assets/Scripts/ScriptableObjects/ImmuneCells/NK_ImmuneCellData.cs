using UnityEngine;

[CreateAssetMenu(fileName = "NK_ImmuneCellData", menuName = "Immuno Defender/Immune Cells/NK Immune Cell Data")]
public class NK_ImmuneCellData : AttackerImmuneCellData
{
    [Header("Farm Analysis")]
    //public float analysisAmount;
    //public float analysisInterval;
    public float analysisTime;
    [Header("Info Panel State Texts")]
    public string noCommandText;
    public string movingToFarmText;
    public string analizingFarmText;
    [Header("Farm Analysis Result Icons")]
    public Sprite goodAnalysisIcon;
    public Sprite badAnalysisIcon;
}
