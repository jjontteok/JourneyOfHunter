using System.Collections;
using UnityEngine;

public class MeteorSkill : AreaTargetSkill, IDelayedDamageSkill
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
        StartCoroutine(ActiveTrueSelf());
    }

    public IEnumerator CoActivateDelayedCollider()
    {
        // 딜레이 타임 후 콜라이더 활성화 -> 대미지
        yield return new WaitForSeconds(_delay);
        _meteorObject.SetActive(false);
        _coll.gameObject.SetActive(true);


        GameObject effect = Instantiate(_skillData.hitEffectPrefab);
        effect.name = $"{_skillData.hitEffectPrefab.name} effect";

        RaycastHit ray;
        Physics.Raycast(transform.position + Vector3.up * 5, Vector3.down, out ray, SkillData.offset.y);
        effect.transform.position = ray.point;
        Debug.Log(ray.point);
        Destroy(effect, 0.5f);
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        if (base.ActivateSkill(pos))
        {
            _meteorObject.SetActive(true);
            // 콜라이더 꺼주고
            _coll.gameObject.SetActive(false);
            StartCoroutine(CoActivateDelayedCollider());
            return true;
        }

        return false;
    }

    IEnumerator ActiveTrueSelf()
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
}
