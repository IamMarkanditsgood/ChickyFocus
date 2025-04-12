using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasickScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _closeButton;


    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public virtual void Subscribe()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(Hide);
        }
    }

    public virtual void Unsubscribe() 
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveListener(Hide);
        }
    }

    public virtual void Show()
    {
        _view.SetActive(true);
    }

    public virtual void Hide()
    {
        _view.SetActive(false);
    }
}
