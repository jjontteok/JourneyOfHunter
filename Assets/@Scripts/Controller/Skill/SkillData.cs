using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public float coolTime;
    public float durationTime;
    public float castingTime;
    public float targetDistance;
    public float damage;
    public float speed;
    public GameObject skillPrefab;
    public GameObject hitEffectPrefab;
    public SkillType skillType;
}

public enum SkillType
{
    Target,
    NonTarget,
    Buff
}
