using System.Collections;
using UnityEngine;

public class MeteorSkill : TransformTargetSkill, IDelayedDamageSkill, IPositioningSkill
{
    [SerializeField] GameObject _meteorObject;
    [SerializeField] float _delay;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        _coll.OnOffHitEffect(false);
    }

    private void OnEnable()
    {
        _meteorObject.transform.localScale = Vector3.one * 0.5f;
        StartCoroutine(CoControlScale());
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = GetCastPosition(pos);
        _coll.transform.localPosition = Vector3.zero;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PlayerSkillLayer), LayerMask.NameToLayer(Define.MonsterTag), true);
        SetDirection();
        _meteorObject.SetActive(true);
        // 콜라이더 꺼주고
        //_coll.gameObject.SetActive(false);
        StartCoroutine(CoActivateDelayedCollider());

        StartCoroutine(DeActivateSkill());
        return true;
    }

    public IEnumerator CoActivateDelayedCollider()
    {
        // 딜레이 타임 후 콜라이더 활성화 -> 대미지
        yield return new WaitForSeconds(_delay);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PlayerSkillLayer), LayerMask.NameToLayer(Define.MonsterTag), false);
        _meteorObject.SetActive(false);
        //_coll.gameObject.SetActive(true);


        GameObject effect = Instantiate(_skillData.HitEffectPrefab);
        effect.name = $"{_skillData.HitEffectPrefab.name} effect";

        RaycastHit ray;
        //Physics.Raycast(transform.position, _direction, out ray, 100, LayerMask.NameToLayer(Define.GroundTag));
        Physics.Raycast(transform.position, _direction, out ray, 100, LayerMask.GetMask(Define.GroundTag));
        effect.transform.position = ray.point;
        Destroy(effect, 0.5f);
    }

    IEnumerator CoControlScale()
    {
        Vector3 targetScale = Vector3.one * 10;
        while (true)
        {
            yield return null;
            _meteorObject.transform.localScale = Vector3.Lerp(_meteorObject.transform.localScale, targetScale, Time.deltaTime);
            if (Vector3.Distance(_meteorObject.transform.localScale, targetScale) <= 1f)
                break;
        }
    }

    public Vector3 GetCastPosition(Vector3 pos)
    {
        return pos + _skillData.Offset;
    }

    public override void SetDirection()
    {
        _direction = (_player.transform.position - transform.position).normalized;
    }
}
