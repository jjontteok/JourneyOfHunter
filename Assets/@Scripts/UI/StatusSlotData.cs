using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StatusSlotData", menuName = "Scriptable Objects/StatusSlotData")]
public class StatusSlotData : ScriptableObject
{
    public Define.StatusType statusType;
    public int level;
    public string statusName;
    public int currentStatusCount;
    public int upgradeCost;

    public int Level
    {
        get => level;
        set
        {
            level = value;
        }
    }
}
