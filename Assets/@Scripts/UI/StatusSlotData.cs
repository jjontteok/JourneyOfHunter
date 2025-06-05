using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusSlotData", menuName = "Scriptable Objects/StatusSlotData")]
public class StatusSlotData : ScriptableObject
{
    public static Action<string, int> OnUpgradeStatus;
    public int level;
    public string statusName;
    public int currentStatusCount;
    public int upgradeCost;

    public int Level
    {
        set
        {
            level = value;
            OnUpgradeStatus?.Invoke(statusName, level);
        }
    }
}
