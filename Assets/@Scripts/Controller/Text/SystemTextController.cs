using UnityEngine;
using TMPro;
using System.Collections;
using System;

// * 시스템 정보 텍스트 컨트롤러 
//- 아이템, 재화 등 획득 정보 표기
//- 던전 관련 상태 변화 등 다양한 상태 정보 표기 
public class SystemTextController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    public TMP_Text Text { get { return _text; } }

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterFadeOut());
    }

    public void SetText(string text)
    {
       _text.text = text;
    }

    public void UpEffect()
    {
        transform.position += Vector3.up * 50;
    }

    IEnumerator DeactivateAfterFadeOut()
    {
        Color color = _text.color;
        color.a = 1.0f;
        _text.color = color;

        yield return new WaitForSeconds(1.0f);

        while (color.a > 0.0f)
        {
            color.a -= Time.deltaTime / 1.5f;
            _text.color = color;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
