using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : Data
{
    public float CoolTime;
    public float DurationTime;
    public float TargetDistance;
    public float Damage;
    public float Speed;
    [Tooltip("DirectionNonTarget 스킬의 각도")]
    public float Angle;                     //DirectionNonTarget 각도
    public GameObject ConnectedSkillPrefab;
    public GameObject HitEffectPrefab;
    public bool TargetExistence;
    public bool IsPlayerSkill;
    public bool IsPassive;
    public Vector3 Offset;

    public Define.SkillAttribute SkillAttribute;
    public Define.StatusType BuffStatus;
    public float BuffAmount;

    public bool IsUltimate;
    public string SkillAnimationName;
}
