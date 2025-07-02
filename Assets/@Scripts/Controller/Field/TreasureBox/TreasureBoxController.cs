using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TreasureBoxController : MonoBehaviour
{

    Dictionary<Define.TreasureRewardType, string> _treasureRewardList;
    ParticleSystem[] _particles;

    Define.TreasureRewardType[]  _treasureRewardArray;
    Animator _animator;

    GameObject _openEffect;
    bool _isObtained;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _openEffect = Instantiate(ObjectManager.Instance.TreasureBoxOpenEffectResource, transform);
        _particles = _openEffect.GetComponentsInChildren<ParticleSystem>();

        _treasureRewardArray = (Define.TreasureRewardType[])Enum.GetValues(typeof(Define.TreasureRewardType));
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _openEffect.SetActive(false);
        _isObtained = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            if (!_isObtained)
            {
                _isObtained = true;
                OpenTreasureBox();
            }
        }
    }

    // * 보물상자를 열었을 때 실행되는 함수
    void OpenTreasureBox()
    {
        PlayOpenAnimation();
        
        Vector3 pos = transform.position + Vector3.up;
        TextManager.Instance.ActivateRewardText(pos, "은화", 100);
        TextManager.Instance.ActivateRewardText(pos, "젬", 10);
        TextManager.Instance.ActivateRewardText(pos, "여정의 증표", 10);
    }

    void PlayOpenAnimation()
    {
        _animator.SetTrigger(Define.Open);

        _openEffect.SetActive(true);
        foreach (var particle in _particles)
        {
            particle.GetComponent<ParticleSystem>().Play();
        }
    }
}
