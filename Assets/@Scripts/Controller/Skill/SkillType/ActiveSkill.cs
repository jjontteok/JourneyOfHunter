using UnityEngine;

public class ActiveSkill : Skill
{
    protected Animator _animator;
    protected SkillColliderController _coll;

    protected override void ActivateSkill(Transform target)
    {
        // 타겟형 스킬인 경우, 방향 설정 및 콜라이더 위치 조정
        if (target != null)
        {
            //타겟 방향으로 스킬 방향 설정
            Vector3 dir = (target.position - _player.transform.position).normalized;
            _player.Rotate(dir);

            _coll.SetColliderDirection(Vector3.forward);
        }
        //플레이어 위치에 스킬 활성화
        transform.localPosition = Vector3.zero;
        _coll.transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
        // particle system인 경우
        gameObject.GetComponent<ParticleSystem>()?.Play();

        StartCoroutine(DeActivateSkill()); //스킬 시전 후 스킬 비활성화
    }

    public override void Initialize(SkillData data)
    {
        base.Initialize(data);
        _animator = _player.GetComponent<Animator>();
        SkillColliderController[] colls = GetComponentsInChildren<SkillColliderController>();
        foreach (var coll in colls)
            Debug.Log(coll.name);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.speed, _skillData.damage, _skillData.targetDistance, _skillData.castingTime, _skillData.hitEffectPrefab);
    }
}
