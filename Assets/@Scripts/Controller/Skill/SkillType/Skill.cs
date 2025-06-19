using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected SkillData _skillData;
    protected WaitForSeconds _skillCoolTime;
    protected WaitForSeconds _skillDurationTime;
    protected PlayerController _player;

    public SkillData SkillData {  get { return _skillData; } }

    public virtual void Initialize(Status status)
    {
        _skillCoolTime = new WaitForSeconds(_skillData.coolTime);
        _skillDurationTime = new WaitForSeconds(_skillData.durationTime);
        _player = PlayerManager.Instance.Player;
    }

    //실제로 스킬 활성화
    //패시브 스킬의 경우, 스테이지 진입 시 적용되므로 활성화만 있으면 됨
    public abstract bool ActivateSkill(Vector3 pos);

    protected abstract IEnumerator DeActivateSkill();
}
