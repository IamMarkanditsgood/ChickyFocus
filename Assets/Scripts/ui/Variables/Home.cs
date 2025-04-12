using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Home : BasickScreen
{
    public ChickenConfig[] ChickenConfigs;
    public GameData _gameData;

    public Button _profile;
    public Button _shop;
    public Button _rules;
    public BasickScreen _profileScreen;
    public BasickScreen _shopScreen;
    public BasickScreen _rulesScreen;
    public BasickScreen _winScreen;
    public BasickScreen _loseScreen;
    public BasickScreen _pauseScreen;

    public Button _start;
    public Sprite _startGame;
    public Sprite _stopGame;



    public TMP_Text _timerText;
    public TMP_Text _pauseText;
    public TMP_Text _pausePopupText;
    public TMP_Text _rewardText;

    public Image[] _chickensCard;


    public Image _egg;
    public Sprite[] _eggSatates;

    public Transform content;
    public Button chickenCardPref;
    public List<Button> _chickenCard = new List<Button>();

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
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _profile.onClick.RemoveListener(Profile);
        _shop.onClick.RemoveListener(Shop);
        _rules.onClick.RemoveListener(Rules);
        _start.onClick.RemoveListener(StartGame);
    }

    public override void Show()
    {
        if(!_gameData.IsBought(ChickenTypes.RhodeIslandRed))
        {
            _gameData.boughtChickens.Add(ChickenTypes.RhodeIslandRed);
        }
        base.Show();
        currentChicken = GetChicken(_gameData.currentChicken);
        SetScreen();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void SetScreen()
    {
        for (int i = 0; i < _chickenCard.Count; i++)
        {
            _chickenCard[i].gameObject.SetActive(true);
        }
        if (_isGameStarted || SaveManager.PlayerPrefs.LoadInt("GameGoing") == 1)
        {
            SaveManager.PlayerPrefs.SaveInt("GameGoing", 0);
            _loseScreen.Show();
            _isGameStarted = false;
            StopAllCoroutines();
        }

        _start.gameObject.GetComponent<Image>().sprite = _startGame;

        _timerText.text = FormatTime(currentChicken.hatchingTime);
        _pauseText.text = FormatTime(currentChicken.pauseTime);
        _pausePopupText.text = FormatTime(currentChicken.pauseTime);

        _egg.sprite = _eggSatates[0];

        SetBoughtChickens();
    }

    public void SetBoughtChickens()
    {
        foreach (var card in _chickenCard)
        {
            Destroy(card.gameObject);
        }
        _chickenCard.Clear();

        for (int i = 0; i < _gameData.boughtChickens.Count; i++)
        {
            ChickenConfig chicken = GetChickenWithoutCurrentIndex(_gameData.boughtChickens[i]);
            Button button = Instantiate(chickenCardPref, content);
            _chickenCard.Add(button);

            button.gameObject.GetComponent<Image>().sprite = chicken.menuDefaultCard;
            if (i == _currentChicken)
            {
                button.gameObject.GetComponent<Image>().sprite = chicken.menuSelectCard;
            }
        }

        for (int i = 0; i < _chickenCard.Count; i++)
        {
            int index = i;
            _chickenCard[index].onClick.AddListener(() => Choose(index));
        }

        if(_isGameStarted)
        {
            for (int i = 0; i < _chickenCard.Count; i++)
            {
                if (i != _currentChicken)
                {
                    _chickenCard[i].gameObject.SetActive(false);
                }
            }
        }
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
    private ChickenConfig GetChickenWithoutCurrentIndex(ChickenTypes chickenType)
    {
        for (int i = 0; i < ChickenConfigs.Length; i++)
        {
            if (chickenType == ChickenConfigs[i].chickenType)
            {
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


    private void Choose(int index)
    {
        _currentChicken = index;
        currentChicken = ChickenConfigs[_currentChicken];
        _gameData.currentChicken = currentChicken.chickenType;
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
        for (int i = 0; i < _chickenCard.Count;i++)
        {
            if (i != _currentChicken)
            {
                _chickenCard[i].gameObject.SetActive(false);
            }
        }


        int timer = currentChicken.hatchingTime / 2;
        int time = 0;

        while (time < timer)
        {
            yield return new WaitForSeconds(1);
            time++;
            _timerText.text = FormatTime(currentChicken.hatchingTime - time);
        }
        _egg.sprite = _eggSatates[1];
        // Pause
        _pauseScreen.Show();    
        timer = currentChicken.pauseTime;
        time = 0;

        while (time < timer)
        {
            yield return new WaitForSeconds(1);
            time++;
            _pauseText.text = FormatTime(timer - time);
            _pausePopupText.text = FormatTime(timer - time);
        }
        _egg.sprite = _eggSatates[2];
        // Game
        _pauseScreen.Hide();
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
        for (int i = 0; i < _chickenCard.Count; i++)
        {
            _chickenCard[i].gameObject.SetActive(true);
        }

        _isGameStarted = false;

        _rewardText.text = currentChicken.reward.ToString();
        _winScreen.Show();

        int coins = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Coins);
        coins += currentChicken.reward;
        _gameData.coins = coins;

        int totalCoins = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalCoins);
        totalCoins += currentChicken.reward;
        _gameData.totalCoins = totalCoins;

        int totalTime = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalTime);
        totalTime = totalTime + currentChicken.hatchingTime + currentChicken.pauseTime;
        _gameData.totalTime = totalTime;


        _gameData.openedChickens[currentChicken.chickenType]++;

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
            SaveManager.PlayerPrefs.SaveInt("GameGoing", 1);
    }
}
