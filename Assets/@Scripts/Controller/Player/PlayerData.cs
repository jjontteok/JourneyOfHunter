using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData" ,menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string Name;             //이름
    public int Level;               //레벨
    public float Exp;               //경험치
    public float Atk;               //공격력
    public float Def;               //방어력
    public float Damage;            //기본대미지
    public float HP;                //체력
    public float HPRecoveryPerSec;  //체력회복
    public float MP;                //마나
    public float MPRecoveryPerSec;  //마나회복
    public float CoolTimeDecrease;  //쿨타임 감소
    public int UnlockedSkillSlotCount;  //현재 오픈된 스킬 슬롯 개수
}
