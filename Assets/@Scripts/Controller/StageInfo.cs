using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageInfo : ScriptableObject
{
    public int StageCount;
    public int ClearCount;

    public string NormalMonsterName;
    public string NamedMonsterName;
}
