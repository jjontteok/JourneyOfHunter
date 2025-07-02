using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI_Merchant : MonoBehaviour
{
    public event Action OnExitButtonClicked;

    [SerializeField] Button _exitButton;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }
}
