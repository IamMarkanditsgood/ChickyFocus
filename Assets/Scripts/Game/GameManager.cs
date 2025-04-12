using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData;
    public Home _home;
    void Start()
    {
        gameData.LoadData();
        _home.Show();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            gameData.SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        gameData.SaveData();
    }
}
