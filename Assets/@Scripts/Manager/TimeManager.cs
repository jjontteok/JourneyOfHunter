
using System;
using System.Collections;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public event Action<float> OnTimeChanged;
    //public event Action<float> OnGainedRecordTimeChanged;

    float _gainedRecordTime;
    float _monsterTime;



    private void OnEnable()
    {
        _monsterTime = 180;
        _gainedRecordTime = 0;
        StartCoroutine(NamedMonsterTimer());
    }

    IEnumerator NamedMonsterTimer()
    {
        while (_monsterTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _gainedRecordTime += 1;
            _monsterTime -= 1;
            OnTimeChanged?.Invoke(_monsterTime);
        }
    }
}
