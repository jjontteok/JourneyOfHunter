using System.Collections;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    WaitForSeconds _deActiveTime = new WaitForSeconds(1.0f);
    private Vector3 _originPos;

    public Vector3 OriginPos 
    { 
        get { return _originPos;  }
        set { _originPos = value; }  
    }

    private void OnEnable()
    {
        StartCoroutine(DeActivateTime());
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_originPos);
        transform.position = screenPos;
        UpEffect();
    }

    //위로 올라가는 텍스트 효과
    void UpEffect()
    {
        _originPos += Vector3.up * Time.deltaTime;
    }

    IEnumerator DeActivateTime()
    {
        yield return _deActiveTime;
        gameObject.SetActive(false);
    }
}
