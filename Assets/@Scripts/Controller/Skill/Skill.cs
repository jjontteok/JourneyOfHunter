using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected SkillData _skillData;
    protected WaitForSeconds _skillCoolTime;
    protected WaitForSeconds _skillDurationTime;
    protected PlayerController _playerController;

    public SkillData SkillData {  get { return _skillData; } }

    public virtual void Initialize(Status status)
    {
        _skillCoolTime = new WaitForSeconds(_skillData.coolTime);
        _skillDurationTime = new WaitForSeconds(_skillData.durationTime);
        _playerController = FindAnyObjectByType<PlayerController>();
    }

    //실제로 스킬 활성화
    public abstract void ActivateSkill(Transform target = null, Vector3 pos = default);


    protected IEnumerator DeActivateSkill()
    {
        yield return _skillDurationTime;
        gameObject.SetActive(false);
    }
}
