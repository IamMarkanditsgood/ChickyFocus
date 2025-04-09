using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rules : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

    [SerializeField] private GameObject _home;
    [SerializeField] private GameObject _view;

    private void Start()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(Close);
    }
    private void Close()
    {
        _home.SetActive(false);
        _home.SetActive(true);
        _view.SetActive(false);
    }
}
