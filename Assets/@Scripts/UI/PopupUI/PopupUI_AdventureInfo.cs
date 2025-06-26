using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PopupUI_AdventureInfo : MonoBehaviour
{
    [SerializeField] TMP_Text _medalText;
    [SerializeField] TMP_Text _adventureText;
    [SerializeField] TMP_Text _minAdventureText;
    [SerializeField] TMP_Text _maxAdventureText;
    [SerializeField] Image _medalImage;
    [SerializeField] Image _adventureGaugeBarImage;

    int _currentMedal;
    float _currentAdventure;
    float _maxAdventure;

    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        PlayerManager.Instance.Player.OnAdventureValueChanged += UpdateAdventure;
    }

    private void OnDisable()
    {
        if(PlayerManager.Instance.Player != null)
            PlayerManager.Instance.Player.OnAdventureValueChanged -= UpdateAdventure;
    }

    void Initialize()
    {
        _currentAdventure = PlayerManager.Instance.Player.Adventure;
        _currentMedal = PlayerManager.Instance.Player.PlayerData.AdventureMedal;
        _maxAdventure = Define.MedalList[_currentMedal].Item3;

        _adventureGaugeBarImage.fillAmount = _currentAdventure / _maxAdventure;
        _adventureText.text = _currentAdventure.ToString();
        _medalText.text = Define.MedalList[_currentMedal].Item1;
        _minAdventureText.text = Define.MedalList[_currentMedal].Item2.ToString();
        _maxAdventureText.text = _maxAdventure.ToString();
    }

    // * 모험 게이지와 메달을 업데이트 하는 함수
    void UpdateAdventure(float adventure, int medal)
    {
        _adventureText.text = adventure.ToString();

        //메달이 업데이트 되면 
        if (_currentMedal != medal)
        {
            _currentMedal = medal;
            _maxAdventure = Define.MedalList[medal].Item3;
            _medalText.text = Define.MedalList[medal].Item1;
            _minAdventureText.text = Define.MedalList[medal].Item2.ToString();
            _maxAdventureText.text = _maxAdventure.ToString();
        }
        _adventureGaugeBarImage.fillAmount = adventure / _maxAdventure;
    }
}
