using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PopupUI_JourneyInfo : MonoBehaviour
{
    [SerializeField] TMP_Text _rankText;
    [SerializeField] TMP_Text _journeyExpText;
    [SerializeField] TMP_Text _minJourneyExpText;
    [SerializeField] TMP_Text _maxJourneyExpText;
    [SerializeField] Image _rankImage;
    [SerializeField] Image _journeyGaugeBarImage;

    PlayerData _currentPlayerData;      //플레이어 데이터
    JourneyRankData _journeyRankData;   //플레이어 데이터의 여정 랭크 데이터
    float _currentJourneyExp;
    int _currentJourneyRank;

    Color _warningColor = new Color(0.896f, 0.139f, 0.139f, 1);

    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        PlayerManager.Instance.Player.OnJourneyExpChanged += UpdateJourneyExp;
        PlayerManager.Instance.Player.OnJourneyRankChanged += UpdateJourneyRank;
    }

    private void OnDisable()
    {
        if(PlayerManager.Instance.Player != null)
        {
            PlayerManager.Instance.Player.OnJourneyExpChanged -= UpdateJourneyExp;
            PlayerManager.Instance.Player.OnJourneyRankChanged -= UpdateJourneyRank;
        }
    }
    void Initialize()
    {
        _journeyRankData = PlayerManager.Instance.Player.PlayerData.JourneyRankData;
        _currentPlayerData = PlayerManager.Instance.Player.PlayerData;
        _currentJourneyExp = _currentPlayerData.JourneyExp;
        _currentJourneyRank = _journeyRankData.index;

        _journeyGaugeBarImage.fillAmount = _currentJourneyExp / _journeyRankData.maxJourneyExp;
        _journeyExpText.text = _currentJourneyExp.ToString(); //모험 게이지
        _journeyExpText.color = Color.white;
        UpdateJourneyUI();
    }

    // * 모험 게이지를 업데이트 하는 함수
    void UpdateJourneyExp(float journeyExp)
    {
        _currentJourneyExp += journeyExp; //증가 EXP량 더해줌
        _journeyExpText.text = _currentJourneyExp.ToString();
        _journeyExpText.color = Color.white;

        //EXP가 점진적으로 차오르도록
        StartCoroutine(StartJourneyExp(_currentJourneyExp));
    }

    // * PlayerController의 OnJourneyRankChanged이벤트 구독하는 함수
    void UpdateJourneyRank(int medal)
    {
        if (_currentJourneyRank != medal)
        {
            _currentJourneyRank = medal;
            _journeyRankData = Instantiate(ObjectManager.Instance.JourneyRankResourceList[medal.ToString()]);
            _journeyGaugeBarImage.fillAmount = 0;
            UpdateJourneyUI();
        }
    }

    //랭크 변경 시 실행되는 함수
    void UpdateJourneyUI()
    {
        _rankText.text = _journeyRankData.name;
        _rankText.color = _journeyRankData.textColor;
        _maxJourneyExpText.text = _journeyRankData.maxJourneyExp.ToString();
        _rankImage.sprite = _journeyRankData.rankImage;
        _journeyGaugeBarImage.color = _journeyRankData.textColor;
    }

    IEnumerator StartJourneyExp(float journeyExp)
    {
        float t = 0;
        float end = journeyExp / _journeyRankData.maxJourneyExp;
        float duration = (end - _journeyGaugeBarImage.fillAmount) * 2;
        float start = _journeyGaugeBarImage.fillAmount;
        while (t < duration)
        {
            t += Time.deltaTime;
            _journeyGaugeBarImage.fillAmount = Mathf.Lerp(start, end, t);
            yield return null;
        }
    }
}
