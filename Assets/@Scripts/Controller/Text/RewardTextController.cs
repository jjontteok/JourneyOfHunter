using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RewardTextController : MonoBehaviour
{
    WaitForSeconds _deActiveTime = new WaitForSeconds(1.0f);
    private Vector3 _originPos;

    public Vector3 OriginPos
    {
        get { return _originPos; }
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

    public void UpdateTextPos()
    {
        _originPos += Vector3.up * 0.5f;
        Debug.Log($"오리진 포스 : {_originPos}");
    }

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
