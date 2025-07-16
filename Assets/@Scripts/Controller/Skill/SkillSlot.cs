using System;
using System.Collections;
using UnityEngine;

/// SkillSlot
public class SkillSlot : MonoBehaviour
{
    // 스킬 슬롯에는 액티브형 스킬만 저장
    protected ActiveSkill _skill;
    protected PlayerController _player;
    public ActiveSkill Skill { get { return _skill; } }

    // 스킬 슬롯 생성 시 스킬 아이콘 슬롯에 등록하는 이벤트
    public Action<SkillData> OnGenerateSlot;
    // 스킬 발동 시 스킬 아이콘 슬롯에 쿨타임 적용하는 이벤트
    public Action<float> OnActivateSkill;
    // 스킬 제거 시 스킬 아이콘 슬롯에서 사라지게 적용하는 이벤트
    public Action OnRemoveSkill;

    // 슬롯에 등록된 스킬의 사용 가능 여부
    public bool IsActivatePossible { get; set; }

    public SkillData SkillData
    {
        get { return _skill.SkillData; }
    }

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _player = PlayerManager.Instance.Player;
    }

    // 슬롯 생성 시 스킬 등록
    public bool SetSkill(SkillData data)
    {
        IsActivatePossible = true;

        Skill skill;
        GameObject skillResource;
        if (ObjectManager.Instance.PlayerSkillResourceList.TryGetValue(data.name, out skillResource))
        {
            skill = skillResource.GetComponent<Skill>();
        }
        else
        {
            Debug.Log("Cannot Find Skill Resource named " + data.Name);
            Destroy(gameObject);
            return false;
        }
        _skill = Instantiate(skill) as ActiveSkill;
        _skill.Initialize(_player.PlayerData);

        // 질풍참 처럼 캐릭터가 함께 이동하는 스킬
        var swift = _skill.GetComponent<ICharacterMovingSkill>();
        if (swift != null)
        {
            swift.OnSkillActivated += _player.ProcessPlayerCollision;
        }

        // 기본공격 처럼 스킬 발동 시 캐릭터가 타겟 향해 회전하는 스킬
        var rotation = _skill.GetComponent<IRotationSkill>();
        if (rotation != null)
        {
            rotation.OnActivateSkill += _player.Rotate;
        }
        _skill.gameObject.SetActive(false);

        OnGenerateSlot?.Invoke(data);
        PopupUI_SkillInventory.OnCurrentSkillIconSet?.Invoke(SkillManager.Instance.CurrentSkillIcons());
        return true;
    }

    protected IEnumerator CoStartCoolTime()
    {
        float realCoolTime = _skill.SkillData.CoolTime;
        Debug.Log($"Real Cool Time: {realCoolTime}");
        realCoolTime *= 1 - (_player.PlayerStatus.GetCoolTimeDecrease() + Util.GetCoolTimeDecreaseByDayType(_skill.SkillData)) / 100;
        Debug.Log($"Reduced Cool Time: {realCoolTime}");

        OnActivateSkill?.Invoke(realCoolTime);
        yield return new WaitForSeconds(realCoolTime);
        IsActivatePossible = true;
    }

    public virtual void ActivateSlotSkill()
    {
        if (IsActivatePossible)
        {
            ProcessSkill();
        }
    }

    // 스킬 발동 & 쿨타임 시작
    void ProcessSkill()
    {
        if (_skill.ActivateSkill(transform.position))
        {
            IsActivatePossible = false;
            StartCoroutine(CoStartCoolTime());
        }

    }

    public void DestroySkillSlot()
    {
        OnRemoveSkill?.Invoke();
        PopupUI_SkillInventory.OnCurrentSkillIconSet?.Invoke(SkillManager.Instance.CurrentSkillIcons());
        Destroy(_skill.gameObject);
        Destroy(gameObject);
    }
}