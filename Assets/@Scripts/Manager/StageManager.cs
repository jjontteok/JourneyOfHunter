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

    #region IEventSubscriber
    public void Subscribe()
    {
        throw new System.NotImplementedException();
    }
    #endregion
    #region IDeactivateObject
    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
