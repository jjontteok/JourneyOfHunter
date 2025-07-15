using System;
using System.Collections;
using UnityEngine;

public class TreasureBoxController : MonoBehaviour, IDamageable, IDelayAutoMoving
{
    AudioSource _rewardSound;

    ParticleSystem[] _particles;

    Animator _animator;

    ParticleSystem[] _treasureEffectList;
    GameObject _openEffect;
    Define.ItemValue _treasureRank;
    int _hitCount;
    int _count;
    Define.ItemValue _initEffectColor;

    bool _isObtained;

    public Define.ItemValue TreasureRank
    {
        get { return _treasureRank; }
        set
        {
            _treasureRank = value;
            _count = (int)_treasureRank * 3;

            _initEffectColor = Define.ItemValue.Common;
            SetTreasureEffect(_initEffectColor);
        }
    }

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _treasureEffectList = GetComponentsInChildren<ParticleSystem>();

        _openEffect = Instantiate(ObjectManager.Instance.TreasureBoxOpenEffectResource, transform);
        _particles = _openEffect.GetComponentsInChildren<ParticleSystem>();

        _animator = GetComponent<Animator>();
        _rewardSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PopupUIManager.Instance.ActivateTreasureAppearText();
        _openEffect.SetActive(false);
        _hitCount = 0;
        _isObtained = false;
    }

    // * 보물상자 이펙트의 색을 설정하는 메서드 
    void SetTreasureEffect(Define.ItemValue treasureRank)
    {
        foreach (var particle in _treasureEffectList)
        {
            ParticleSystem.MainModule main = particle.main;
            main.startColor = Define.EffectColorList[treasureRank];
        }
    }

    // * 플레이어가 보물상자를 때릴 때 실행되는 함수
    void UpHitCount()
    {
        if (_isObtained) return;

        _hitCount++;                    //때린 횟수 증가
        if (_hitCount % 3 == 0)         //3번 때릴 때마다 상자 색 변경
        {
            if (_hitCount >= _count)    //때린 횟수가 현재 상자 랭크와 같다면
            {
                OpenTreasureBox();      //보물 상자 오픈
            }
            else
            {
                SetTreasureEffect(++_initEffectColor);
            }
        }
    }

    // * 보물상자를 열었을 때 실행되는 함수
    void OpenTreasureBox()
    {
        _isObtained = true;
        PlayOpenAnimation();
        FieldManager.Instance.RewardSystem.GainReward(transform.position + Vector3.up * 2);
        FieldManager.Instance.IsClear = true;
        StartCoroutine(CoSetAutoMoving());
        _rewardSound.Play();
       
    }

    public IEnumerator CoSetAutoMoving()
    {
        yield return new WaitForSeconds(2f);
        PlayerManager.Instance.IsAutoMoving = true;
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

    public void GetDamage(float damage)
    {
        UpHitCount();
    }

    // 보물상자는 대미지가 아닌 타격 횟수이므로 여기선 필요 없긴함
    public float CalculateFinalDamage(float damage, float def)
    {
        return damage;
    }
}
