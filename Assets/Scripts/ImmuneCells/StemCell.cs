using UnityEngine;

public class StemCell : ImmuneCell
{
    protected StemCellStats stemCellStats => (StemCellStats)basicStats;

    public override void Initialize(Tower parentTower, Enemy target, ImmuneCellData data)
    {
        base.Initialize(parentTower, target, data);

        if (data is StemCellData stemCellData)
        {
            basicStats = new StemCellStats(stemCellData);
        }
    }

    public override void Reset()
    {
        base.Reset();
    }
}
