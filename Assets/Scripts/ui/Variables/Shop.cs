using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : BasickScreen
{
    public Home home;
    public GameData gameData;
    public ChickenConfig[] chickenConfigs;

    public Button next;
    public Button prev;
    public Button buy;
    public TMP_Text price;

    public GameObject purchased;

    public TMP_Text _coins;

    public Image _chicken;

    private int currentChicken;

    public override void Subscribe()
    {
        base.Subscribe();
        next.onClick.AddListener(Next);
        prev.onClick.AddListener(Previous);
        buy.onClick.AddListener(Buy);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        next.onClick.RemoveListener(Next);
        prev.onClick.RemoveListener(Previous);
        buy.onClick.RemoveListener(Buy);
    }

    public override void Show()
    {
        base.Show();
        SetScreen();
    }

    public override void Hide()
    {
        base.Hide();
        home.SetBoughtChickens();
    }

    private void SetScreen()
    {
        _coins.text = gameData.coins.ToString();
        _chicken.sprite = chickenConfigs[currentChicken].shopCard;

        if (chickenConfigs[currentChicken].price == 0 || gameData.IsBought(chickenConfigs[currentChicken].chickenType))
        {
            purchased.SetActive(true);
            buy.gameObject.SetActive(false);
        }
        else
        {
            purchased.SetActive(false);
            buy.gameObject.SetActive(true);
            price.text = chickenConfigs[currentChicken].price.ToString();
        }
    }

    private void Next()
    {
        if(currentChicken < chickenConfigs.Length -1)
        {
            currentChicken++;
            SetScreen();
        }
    }

    private void Previous()
    {
        if(currentChicken > 0)
        {
            currentChicken--;
            SetScreen();
        }
    }

    private void Buy()
    {
        if(gameData.coins >= chickenConfigs[currentChicken].price && !gameData.IsBought(chickenConfigs[currentChicken].chickenType))
        {
            gameData.coins -= chickenConfigs[currentChicken].price;
            gameData.boughtChickens.Add(chickenConfigs[currentChicken].chickenType);
            gameData.unlockedChicken++;
            SetScreen();
        }
    }
}
