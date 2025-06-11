
using System;
using System.Collections;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public event Action<float> OnNamedMonsterTimeChanged;
    public event Action<float> OnGainedRecordTimeChanged;

    WaitForSeconds _time = new WaitForSeconds(1f);

    float _gainedRecordTime;
    float _monsterTime;

    private void OnEnable()
    {
        _monsterTime = 180;
        _gainedRecordTime = 0;
        StartGainedRecord();
    }

    public void StartNamedMonsterStage()
    {
        StartCoroutine(NamedMonsterTimer());
    }

    public void StartGainedRecord()
    {
        StartCoroutine(GainedRecordTimer());
    }

    IEnumerator GainedRecordTimer()
    {
        yield return _time;
        _gainedRecordTime += 1;
        OnGainedRecordTimeChanged?.Invoke(_gainedRecordTime);
    }

    IEnumerator NamedMonsterTimer()
    {
        while (_monsterTime > 0)
        {
            yield return _time;
            _monsterTime -= 1;
            OnNamedMonsterTimeChanged?.Invoke(_monsterTime);
        }
    }
}
