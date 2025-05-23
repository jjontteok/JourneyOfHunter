using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected SkillData _skillData;
    protected WaitForSeconds _skillCoolTime;
    protected WaitForSeconds _skillDurationTime;

    public SkillData SkillData {  get { return _skillData; } }

    public virtual void Initialize()
    {
        _skillCoolTime = new WaitForSeconds(_skillData.coolTime);
        _skillDurationTime = new WaitForSeconds(_skillData.durationTime);
    }

    //실제로 스킬 활성화
    public abstract void ActivateSkill(Transform target = null, Vector3 pos = default);

    //스킬 시전 후 해당 스킬 비활성화
    protected IEnumerator DeActivateSkill()
    {
        yield return _skillDurationTime;
        gameObject.SetActive(false);
    }
}
