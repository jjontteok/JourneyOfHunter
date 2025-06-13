
using System;
using System.Collections;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public event Action<float> OnNamedMonsterTimeChanged;
    public event Action OnGainedRecordTimeChanged;

    WaitForSeconds _time = new WaitForSeconds(1f);

    float _monsterTime;

    //임시 플래그 변수
    bool _isPlaying = true;

    private void OnEnable()
    {
        _monsterTime = 180;
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
        while (_isPlaying)
        {
            yield return _time;
            OnGainedRecordTimeChanged?.Invoke();
        }
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
