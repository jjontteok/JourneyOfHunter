using UnityEngine;

public class TransformTargetSkill : ActiveSkill, ITargetSkill, IMovingSkill, IDirectionSkill
{
    protected SkillColliderController _coll;
    Transform _target;
    protected Vector3 _direction;

    public Vector3 Direction { get => _direction; }

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(status, _skillData);
    }

    // 방향 설정 + 타겟 설정
    public override bool ActivateSkill(Vector3 pos)
    {
        if (IsTargetExist(pos, SkillData.isPlayerSkill))
        {
            base.ActivateSkill(pos);
            _coll.transform.localPosition = Vector3.zero;
            SetDirection();

            return true;
        }

        return false;
    }

    private void Update()
    {
        MoveSkillCollider();
    }

    public virtual bool IsTargetExist(Vector3 pos, bool isPlayerSkill)
    {
        // 수동 모드일 땐 타겟 유무 상관없이 그냥 발사
        if (SkillData.isPlayerSkill && !PlayerManager.Instance.IsAuto)
            return true;
        _target = Util.GetNearestTarget(pos, _skillData.targetDistance, isPlayerSkill)?.transform;
        //Debug.Log($"Current Target: {_target.name}\npostion:{_target.position}");
        return _target != null;
    }

    public virtual void MoveSkillCollider()
    {
        if (Vector3.Distance(transform.position, _coll.transform.position) < _skillData.targetDistance)
        {
            _coll.transform.Translate(_direction * _skillData.speed * Time.deltaTime, Space.World);
        }
    }

    public void SetDirection()
    {
        // 플레이어 스킬이 아니면(==몬스터 스킬이면) 타겟을 향해 설정
        if (!SkillData.isPlayerSkill)
        {
            Vector3 dir = _target.position - transform.position;
            dir.y = 0;
            _direction = dir.normalized;
        }
        else
        {
            // 자동 모드면 가까운 적을 향해 방향 설정
            if (PlayerManager.Instance.IsAuto)
            {
                //타겟 방향으로 스킬 방향 설정
                //스킬이 땅으로 박히지 않도록 높이 맞춰주기
                Vector3 dir = _target.position - transform.position;
                dir.y = 0;
                _direction = dir.normalized;
            }
            // 수동 모드면 현재 플레이어가 바라보는 방향으로 설정
            else
            {
                _direction = PlayerManager.Instance.Player.transform.TransformDirection(Vector3.forward);
            }
        }
        transform.rotation = Quaternion.LookRotation(_direction);
    }
}
