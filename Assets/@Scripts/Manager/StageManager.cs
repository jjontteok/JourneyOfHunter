using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>, IEventSubscriber, IDeactivateObject
{
    private List<StageInfo> _stageInfos;

    protected override void Initialize()
    {
        base.Initialize();
        _stageInfos = new List<StageInfo>();
    }

    public void Subscribe()
    {
        throw new System.NotImplementedException();
    }
    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }

}

public struct StageInfo
{
    public string StageName;
    public int StageCount;
    public string NormalMonsterName;
    public string NamedMonsterName;
}
