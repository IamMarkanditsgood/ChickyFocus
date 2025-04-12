using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Home : BasickScreen
{
    public ChickenConfig[] ChickenConfigs;
    public GameData GameData;

    public Button _profile;
    public Button _shop;
    public Button _rules;
    public BasickScreen _profileScreen;
    public BasickScreen _shopScreen;
    public BasickScreen _rulesScreen;
    public BasickScreen _winScreen;
    public BasickScreen _loseScreen;

    public Button _start;
    public Sprite _startGame;
    public Sprite _stopGame;

    public Button _nextChicken;
    public Button _previousChicken;

    public TMP_Text _timerText;
    public TMP_Text _pauseText;
    public TMP_Text _rewardText;

    public Image[] _chickensCard;


    public Image _egg;
    public Sprite[] _eggSatates;

    ChickenConfig currentChicken;
    private int _currentChicken;
    private bool _isGameStarted;

    public override void Subscribe()
    {
        base.Subscribe();
        _profile.onClick.AddListener(Profile);
        _shop.onClick.AddListener(Shop);
        _rules.onClick.AddListener(Rules);
        _start.onClick.AddListener(StartGame);
        _previousChicken.onClick.AddListener(Previous);
        _nextChicken.onClick.AddListener(Next);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _profile.onClick.RemoveListener(Profile);
        _shop.onClick.RemoveListener(Shop);
        _rules.onClick.RemoveListener(Rules);
        _start.onClick.RemoveListener(StartGame);
        _previousChicken.onClick.RemoveListener(Previous);
        _nextChicken.onClick.RemoveListener(Next);
    }

    public override void Show()
    {
        base.Show();
        currentChicken = GetChicken(GameData.currentChicken);
        SetScreen();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void SetScreen()
    {
        if (_isGameStarted || PlayerPrefs.GetInt("GameGoing") == 1)
        {
            PlayerPrefs.SetInt("GameGoing", 0);
            _loseScreen.Show();
            _isGameStarted = false;
            StopAllCoroutines();
        }

        _start.gameObject.GetComponent<Image>().sprite = _startGame;

        _timerText.text = FormatTime(currentChicken.hatchingTime);
        _pauseText.text = FormatTime(currentChicken.pauseTime);

        _chickensCard[0].enabled = true;
        _chickensCard[2].enabled = true;

        if (_currentChicken > 0)
        {
            _chickensCard[0].sprite = ChickenConfigs[_currentChicken - 1].menuDefaultCard;
        }
        else
        {
            _chickensCard[0].enabled = false;
        }
        _chickensCard[1].sprite = ChickenConfigs[_currentChicken].menuSelectCard;
        if (_currentChicken < ChickenConfigs.Length - 1)
        {
            _chickensCard[2].sprite = ChickenConfigs[_currentChicken + 1].menuDefaultCard;
        }
        else
        {
            _chickensCard[2].enabled = false;
        }

        _egg.sprite = _eggSatates[0];
    }


    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    private ChickenConfig GetChicken(ChickenTypes chickenType)
    {
        for(int i =0;i < ChickenConfigs.Length; i++)
        {
            if(chickenType == ChickenConfigs[i].chickenType)
            {
                _currentChicken = i;
                return ChickenConfigs[i];
            }
        }
        return null;
    }

    private void Profile()
    {
        _profileScreen.Show();
    }
    private void Shop()
    {
        _shopScreen.Show();
    }
    private void Rules()
    {
        _rulesScreen.Show();
    }


    private void Next()
    {
        if(_currentChicken < ChickenConfigs.Length -1)
        {
            _currentChicken++;
            currentChicken = ChickenConfigs[_currentChicken];
        }
        SetScreen();
    }
    private void Previous()
    {
        if (_currentChicken > 0)
        {
            _currentChicken--;
            currentChicken = ChickenConfigs[_currentChicken];
        }
        SetScreen();
    }
    private void StartGame()
    {
        if (_isGameStarted)
        {
            SetScreen();
        }
        else
        {
            _start.gameObject.GetComponent<Image>().sprite = _stopGame;
            _isGameStarted = true;
            StartCoroutine(GameTimer());
        }
    }
    private IEnumerator GameTimer()
    {
        int timer = currentChicken.hatchingTime / 2;
        int time = 0;

        while (time < timer)
        {
            yield return new WaitForSeconds(1);
            time++;
            _timerText.text = FormatTime(currentChicken.hatchingTime - time);
        }

        // Pause
        timer = currentChicken.pauseTime;
        time = 0;

        while (time < timer)
        {
            yield return new WaitForSeconds(1);
            time++;
            _pauseText.text = FormatTime(timer - time);
        }

        // Game
        timer = currentChicken.hatchingTime;
        time = currentChicken.hatchingTime / 2;

        while (time < timer)
        {
            yield return new WaitForSeconds(1);
            time++;
            _timerText.text = FormatTime(currentChicken.hatchingTime - time);
        }

        WinGame();
    }

    private void WinGame()
    {
        _isGameStarted = false;

        _rewardText.text = currentChicken.reward.ToString();
        _winScreen.Show();

        int coins = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Coins);
        coins += currentChicken.reward;
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Coins, coins);

        int totalCoins = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalCoins);
        totalCoins += currentChicken.reward;
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.TotalCoins, totalCoins);

        int totalTime = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalTime);
        totalTime = totalTime + currentChicken.hatchingTime + currentChicken.pauseTime;
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.TotalTime, totalTime);

        SetScreen();
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            StopGame();
        }
        else
        {
            Debug.Log("Гра знову активна");
        }
    }

    private void OnApplicationQuit()
    {
            StopGame();
    }
    private void StopGame()
    {
        if (_isGameStarted)
            PlayerPrefs.SetInt("GameGoing", 1);
    }
}
