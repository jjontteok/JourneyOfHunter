using System;
using System.Collections;
using UnityEngine;

public class MonsterGateController : MonoBehaviour
{
    ParticleSystem _particleSystem;
    public Action<int> OnGateOpen;
    private int _index;

    private void OnEnable()
    {
        //float randomAngle = Random.Range(-150, -120);
        //transform.rotation = Quaternion.AngleAxis(randomAngle, Vector3.right);
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        StartCoroutine(SetGate());
    }

    public void SetRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir - transform.position, Vector3.down);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    //IEnumerator CreateGate()
    //{
    //    Vector3 targetScale = new Vector3(3, 3, 3);
    //    while (true)
    //    {
    //        yield return null;
    //        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 0.5f);
    //        if (Vector3.Distance(transform.localScale, targetScale) <= 1f)
    //            break;
    //    }
    //}

    //public void StartDestroyGate()
    //{
    //    StartCoroutine(SetGate(false));
    //}

    IEnumerator SetGate()
    {
        Vector3 targetScale = new Vector3(3, 3, 3);
        while(true)
        {
            yield return null;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 1f);
            if(Vector3.Distance(transform.localScale, targetScale) <= 1f)
                break;
        }
        OnGateOpen?.Invoke(_index);
        targetScale = new Vector3(0.01f, 0.01f, 0.01f);
        while (true)
        {
            yield return null;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 1f);
            if (Vector3.Distance(transform.localScale, targetScale) <= 1f)
                break;
        }
        gameObject.SetActive(false);
    }
}
