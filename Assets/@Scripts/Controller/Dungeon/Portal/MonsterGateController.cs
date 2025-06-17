using System.Collections;
using UnityEngine;

public class MonsterGateController : MonoBehaviour
{
    ParticleSystem _particleSystem;

    private void OnEnable()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        StartCoroutine(SetGate());
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
