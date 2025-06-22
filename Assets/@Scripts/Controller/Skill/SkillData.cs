using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public float coolTime;
    public float durationTime;
    public float targetDistance;
    public float damage;
    public float speed;
    public float angle;                     //DirectionNonTarget 각도
    public float MP;
    public GameObject connectedSkillPrefab;
    public GameObject hitEffectPrefab;
    public bool targetExistence;
    public bool isPlayerSkill;
    public bool isPassive;
    public Vector3 offset;

    public Define.StatusType buffStatus;
    public float buffAmount;
}
