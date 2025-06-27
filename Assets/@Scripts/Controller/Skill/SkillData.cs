using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;
    public Sprite SkillIcon;
    public float CoolTime;
    public float DurationTime;
    public float TargetDistance;
    public float Damage;
    public float Speed;
    public float Angle;                     //DirectionNonTarget 각도
    public float MP;
    public GameObject ConnectedSkillPrefab;
    public GameObject HitEffectPrefab;
    public bool TargetExistence;
    public bool IsPlayerSkill;
    public bool IsPassive;
    public Vector3 Offset;

    public Define.StatusType BuffStatus;
    public float BuffAmount;
}
