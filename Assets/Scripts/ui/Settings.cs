using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_InputField _name;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _vibrationButton;
    [SerializeField] private Button _SoundsButton;
    [SerializeField] private Button _avatarButton;

    [SerializeField] private Sprite _off;
    [SerializeField] private Sprite _on;

    [SerializeField] private GameObject _home;
    [SerializeField] private GameObject _view;

    [SerializeField] private AvatarManager _avatarManager;

    private void OnEnable()
    {

        _name.text = PlayerPrefs.GetString("Name", "User Name");

        _avatarManager.SetSavedPicture();

        if(!PlayerPrefs.HasKey("Vibration"))
        {
            PlayerPrefs.SetInt("Vibration",1);
            
        }
        if (!PlayerPrefs.HasKey("Sounds"))
        {
            PlayerPrefs.SetInt("Sounds", 1);
        }

        if(PlayerPrefs.GetInt("Vibration") == 0)
        {
            _vibrationButton.gameObject.GetComponent<Image>().sprite = _off;
        }
        else if(PlayerPrefs.GetInt("Vibration") == 1)
        {
            _vibrationButton.gameObject.GetComponent<Image>().sprite = _on;
        }

        if (PlayerPrefs.GetInt("Sounds") == 0)
        {
            _SoundsButton.gameObject.GetComponent<Image>().sprite = _off;
        }
        else if (PlayerPrefs.GetInt("Sounds") == 1)
        {
            _SoundsButton.gameObject.GetComponent<Image>().sprite = _on;
        }
    }
    private void Start()
    {
        _closeButton.onClick.AddListener(Close);
        _vibrationButton.onClick.AddListener(Vibration);
        _SoundsButton.onClick.AddListener(Sounds);
        _avatarButton.onClick.AddListener(_avatarManager.PickFromGallery);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(Close);
        _vibrationButton.onClick.RemoveListener(Vibration);
        _SoundsButton.onClick.RemoveListener(Sounds);
        _avatarButton.onClick.RemoveListener(_avatarManager.PickFromGallery);
    }

    private void Close()
    {
        PlayerPrefs.SetString("Name", _name.text);
        _home.SetActive(false);
        _home.SetActive(true);
        _view.SetActive(false);
    }
    private void Vibration()
    {
        if (PlayerPrefs.GetInt("Vibration") == 0)
        {
            PlayerPrefs.SetInt("Vibration", 1);
            _vibrationButton.gameObject.GetComponent<Image>().sprite = _on;
        }
        else if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            PlayerPrefs.SetInt("Vibration", 0);
            _vibrationButton.gameObject.GetComponent<Image>().sprite = _off;
        }
    }
    private void Sounds()
    {
        if (PlayerPrefs.GetInt("Sounds") == 0)
        {
            PlayerPrefs.SetInt("Sounds", 1);
            _SoundsButton.gameObject.GetComponent<Image>().sprite = _on;
        }
        else if (PlayerPrefs.GetInt("Sounds") == 1)
        {
            PlayerPrefs.SetInt("Sounds", 0);
            _SoundsButton.gameObject.GetComponent<Image>().sprite = _off;
        }
    }
}
