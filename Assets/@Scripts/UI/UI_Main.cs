using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    public event Action OnStartButtonClicked;
    public event Action<string> OnPlayerNameInputted;

    [SerializeField] private Button _startButton;
    [SerializeField] private TMP_InputField _inputeField;

    private string _playerName;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        _startButton.interactable = _inputeField.text != "" ? true : false;
    }

    void Initialize()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
    }

    void OnStartButtonClick()
    {
        OnPlayerNameInputted?.Invoke(_inputeField.text);
        OnStartButtonClicked?.Invoke();
        AudioManager.Instance.PlayClick();
    }
}
