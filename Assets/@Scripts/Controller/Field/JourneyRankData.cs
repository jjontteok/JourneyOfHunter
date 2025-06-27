using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "JourneyRankData", menuName = "Scriptable Objects/JourneyRankData")]
public class JourneyRankData : ScriptableObject
{
    public int Index;
    public Define.JourneyRankType RankType;
    public string Name;
    public float MinJourneyExp;
    public float MaxJourneyExp;
    public Color TextColor;
    public Sprite RankImage;
}
