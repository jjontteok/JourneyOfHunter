using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI_Setting : MonoBehaviour
{
    [SerializeField] Button _exitButton;
    [SerializeField] Button _quitButton;
    [SerializeField] Slider _sliderBGM;
    [SerializeField] Slider _sliderVFX;
    [SerializeField] Slider _sliderClick;

    public event Action OnExitButtonClicked;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);

        _sliderBGM.onValueChanged.AddListener(value => AudioManager.Instance.SetBGMVolume(value));
        _sliderVFX.onValueChanged.AddListener(value => AudioManager.Instance.SetVFXVolume(value));
        _sliderClick.onValueChanged.AddListener(value => AudioManager.Instance.SetClickVolume(value));
    }

    void OnEnable()
    {
        if (AudioManager.Instance.MasterMixer == null)
            return;
        if(AudioManager.Instance.MasterMixer.GetFloat(Define.BGM,out float currentBGM))
        {
            float linear = Mathf.Pow(10f, currentBGM);
            _sliderBGM.SetValueWithoutNotify(linear);
        }
        if (AudioManager.Instance.MasterMixer.GetFloat(Define.VFX, out float currentVFX))
        {
            float linear = Mathf.Pow(10f, currentVFX / 12f);
            _sliderVFX.SetValueWithoutNotify(linear);
        }
        if (AudioManager.Instance.MasterMixer.GetFloat(Define.Click, out float currentClick))
        {
            float linear = Mathf.Pow(10f, currentClick / 10f);
            _sliderClick.SetValueWithoutNotify(linear);
        }
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
        AudioManager.Instance.PlayClickSound();
    }

    void OnQuitButtonClick()
    {
        AudioManager.Instance.PlayClickSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
