using System.Collections;
using UnityEditor;
using UnityEngine;

public class MeteorSkill : AreaTargetSkill, IDelayedDamageSkill
{
    [SerializeField] GameObject _meteorObject;
    [SerializeField] float _delay;

    public IEnumerator CoActivateDelayedCollider()
    {
        // 딜레이 타임 후 콜라이더 활성화 -> 대미지
        yield return new WaitForSeconds(_delay);
        _meteorObject.SetActive(false);
        _coll.gameObject.SetActive(true);
        //EditorApplication.isPaused = true;
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
}
