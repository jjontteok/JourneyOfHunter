using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

// * GameManager 스크립트
//- 
public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        Initialize();
        EventSubscribeAll();
        DeactivateObjectsAll();
        Settings();
    }

    // * 초기화 메서드
    protected override void Initialize()
    {
        base.Initialize();

        ObjectManager.Instance.CreateManager();
        ObjectManager.Instance.ResourceLoad();

        UIManager.Instance.CreateManager();

        PoolManager.Instance.CreateManager();

        DungeonManager.Instance.CreateManager();

        PopupUIManager.Instance.CreateManager();

        SpawnManager.Instance.CreateManager();

        DamageTextManager.Instance.CreateManager();

        SkillManager.Instance.CreateManager();

        TimeManager.Instance.CreateManager();

        EnvironmentManager.Instance.CreateManager();
    }

    // * 이벤트 구독 메서드
    private void EventSubscribeAll()
    {
        DungeonManager.Instance.Subscribe();
        PopupUIManager.Instance.Subscribe();
        DamageTextManager.Instance.Subscribe();
        SpawnManager.Instance.Subscribe();
        EnvironmentManager.Instance.Subscribe();
    }

    // * 오브젝트 비활성화 메서드
    private void DeactivateObjectsAll()
    {
        DungeonManager.Instance.Deactivate();
        PopupUIManager.Instance.Deactivate();
        SpawnManager.Instance.Deactivate();
        DamageTextManager.Instance.Deactivate();
    }
    
    private void Settings()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.PlayerTag), LayerMask.NameToLayer(Define.PlayerSkillLayer));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MonsterTag), LayerMask.NameToLayer(Define.MonsterSkillLayer));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MonsterAttackRangeLayer), LayerMask.NameToLayer(Define.MonsterSkillLayer));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MonsterAttackRangeLayer), LayerMask.NameToLayer(Define.PlayerSkillLayer));
    }    
}
