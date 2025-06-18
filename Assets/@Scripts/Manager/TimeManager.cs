using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>, IEventSubscriber
{
    public event Action<float> OnNamedMonsterTimeChanged;
    public event Action OnGainedRecordTimeChanged;
    public event Action<Define.TimeOfDayType, float> OnDayTimeChanged;

    WaitForSeconds _time = new WaitForSeconds(1f);

    private Dictionary<float, Define.TimeOfDayType> _timeTransitions;

    float _monsterTime;
    float _dayTime;
    float _duration;

    public float Duration
    {
        get { return _duration; }
    }

    //임시 플래그 변수
    bool _isPlaying = true;

    protected override void Initialize()
    {
        base.Initialize();
        _timeTransitions = new()
        {
            { 30f, Define.TimeOfDayType.Morning },
            { 40f, Define.TimeOfDayType.Noon },
            { 10f, Define.TimeOfDayType.Evening },
            { 20f, Define.TimeOfDayType.Night }
        };
        _duration = GetTime(GetNextType(Define.TimeOfDayType.Noon));
    }

    private void OnEnable()
    {
        StartGainedRecord();
        StartDay();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        DungeonManager.Instance.OnSpawnNamedMonster += StartNamedMonsterStage;
    }
    #endregion

    public void StartNamedMonsterStage()
    {
        _monsterTime = 100;
        StartCoroutine(NamedMonsterTimer());
    }

    public void StartGainedRecord()
    {
        StartCoroutine(GainedRecordTimer());
    }

    public void StartDay()
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
                _duration = GetTime(GetNextType(newTimeOfDay)) - GetTime(newTimeOfDay);
                if(newTimeOfDay == Define.TimeOfDayType.Noon)
                {
                    _dayTime = 0f;
                    _duration = GetTime(GetNextType(newTimeOfDay));
                }                        
                OnDayTimeChanged?.Invoke(newTimeOfDay, _duration);
            }  
        }
    }

    float GetTime(Define.TimeOfDayType type)
    {
        return _timeTransitions.FirstOrDefault(x => x.Value == type).Key;
    }

    Define.TimeOfDayType GetNextType(Define.TimeOfDayType type)
    {
        return type switch
        {
            Define.TimeOfDayType.Noon => Define.TimeOfDayType.Evening,
            Define.TimeOfDayType.Evening => Define.TimeOfDayType.Night,
            Define.TimeOfDayType.Night => Define.TimeOfDayType.Morning,
            Define.TimeOfDayType.Morning => Define.TimeOfDayType.Noon,
            _ => 0
        };
    }
    IEnumerator NamedMonsterTimer()
    {
        while (_monsterTime > 0)
        {
            yield return _time;
            _monsterTime -= 1;
            if(_monsterTime <= 0)
                break;
            OnNamedMonsterTimeChanged?.Invoke(_monsterTime);
        }
    }
}
