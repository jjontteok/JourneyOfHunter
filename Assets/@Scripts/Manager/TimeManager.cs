
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public event Action<float> OnNamedMonsterTimeChanged;
    public event Action OnGainedRecordTimeChanged;
    public event Action<Define.TimeOfDayType> OnDayTimeChanged;

    WaitForSeconds _time = new WaitForSeconds(1f);

    private Dictionary<float, Define.TimeOfDayType> _timeTransitions;

    float _monsterTime;
    float _dayTime;

    //임시 플래그 변수
    bool _isPlaying = true;

    protected override void Initialize()
    {
        base.Initialize();
        _timeTransitions = new()
        {
            { 20f, Define.TimeOfDayType.Morning },
            { 30f, Define.TimeOfDayType.Noon },
            { 10f, Define.TimeOfDayType.Evening },
            { 15f, Define.TimeOfDayType.Night }
        };
    }

    private void OnEnable()
    {
        _monsterTime = 180;
        StartGainedRecord();
        StartDay();
    }

    public void StartNamedMonsterStage()
    {
        StartCoroutine(NamedMonsterTimer());
    }

    public void StartGainedRecord()
    {
        StartCoroutine(GainedRecordTimer());
    }

    void StartDay()
    {
        StartCoroutine(DayTimer());
    }

    IEnumerator GainedRecordTimer()
    {
        while (_isPlaying)
        {
            yield return _time;
            OnGainedRecordTimeChanged?.Invoke();
        }
    }

    IEnumerator DayTimer()
    {
        while (_isPlaying)
        {
            yield return _time;
            _dayTime += 1f;

            if(_timeTransitions.TryGetValue(_dayTime, out var newTimeOfDay))
            {
                OnDayTimeChanged?.Invoke(newTimeOfDay);
                if(newTimeOfDay == Define.TimeOfDayType.Noon)
                {
                    _dayTime = 0f;
                }        
            }  
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
