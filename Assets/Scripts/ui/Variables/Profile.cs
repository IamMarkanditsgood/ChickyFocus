using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : BasickScreen
{
    public AvatarManager avatarManager;
    public GameData gameData;
    public TMP_Text _name;
    public TMP_InputField _nameInput;

    public TMP_Text _unlockenChicken;
    public TMP_Text _totalCoins;
    public TMP_Text _totalTime;

    public ChickenTypes[] chickenTypes;
    public TMP_Text[] _amounts;

    public Button editName;
    public Button editAvatar;
    public Button shop;
    public Shop ShopScreen;

    public override void Subscribe()
    {
        base.Subscribe();

        editName.onClick.AddListener(EditName);
        editAvatar.onClick.AddListener(EditAvatar);
        shop.onClick.AddListener(Shop);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        editName.onClick.RemoveListener(EditName);
        editAvatar.onClick.RemoveListener(EditAvatar);
        shop.onClick.RemoveListener(Shop);
    }

    public override void Show()
    {
        base.Show();
        SetScreen();
    }

    public override void Hide()
    {
        base.Hide();
        PlayerPrefs.SetString("Name", _nameInput.text);
    }

    private void SetScreen()
    {
        avatarManager.SetSavedPicture();
        _name.text = PlayerPrefs.GetString("Name", "UserName");
        _nameInput.text = PlayerPrefs.GetString("Name", "UserName");

        _unlockenChicken.text = gameData.boughtChickens.Count + "/10";
        _totalCoins.text = gameData.totalCoins.ToString();
        _totalTime.text = FormatTime(gameData.totalTime).ToString();

        SetChickenAmount();
    }

    private void SetChickenAmount()
    {
        for(int i = 0;i < chickenTypes.Length; i++)
        {
            _amounts[i].text = gameData.openedChickens[chickenTypes[i]].ToString();
        }
    }

    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void EditName()
    {
        if(_nameInput.gameObject.activeInHierarchy)
        {
            _nameInput.gameObject.SetActive(false);
            _name.gameObject.SetActive(true);
            _name.text = _nameInput.text;
            PlayerPrefs.SetString("Name", _nameInput.text);
        }
        else
        {
            _nameInput.gameObject.SetActive(true);
            _name.gameObject.SetActive(false);
        }
    }
    private void EditAvatar()
    {
        avatarManager.PickFromGallery();
    }
    private void Shop()
    {
        Hide();
        ShopScreen.Show();
    }

}
