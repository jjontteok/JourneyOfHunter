using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "JourneyRankData", menuName = "Scriptable Objects/JourneyRankData")]
public class JourneyRankData : ScriptableObject
{
    public int index;
    public Define.JourneyRankType rankType;
    public string name;
    public float minAdventure;
    public float maxAdventure;
    public Color textColor;
    public Sprite rankImage;
}
