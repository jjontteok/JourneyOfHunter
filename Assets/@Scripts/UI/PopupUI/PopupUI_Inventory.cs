using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI_Inventory : MonoBehaviour
{
    [SerializeField] Button _exitButton;

    public event Action OnExitButtonClicked;

    private void Awake()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }
}
