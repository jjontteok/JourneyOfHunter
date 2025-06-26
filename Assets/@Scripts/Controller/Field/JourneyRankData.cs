using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "JourneyRankData", menuName = "Scriptable Objects/JourneyRankData")]
public class JourneyRankData : ScriptableObject
{
    public int index;
    public Define.JourneyRankType rankType;
    public string name;
    public float minJourneyExp;
    public float maxJourneyExp;
    public Color textColor;
    public Sprite rankImage;
}
