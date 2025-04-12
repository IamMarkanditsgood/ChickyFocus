using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GameData : MonoBehaviour
{
    public ChickenTypes currentChicken;
    public int coins;
    public int unlockedChicken;
    public int totalCoins;
    public int totalTime;

    public Dictionary<ChickenTypes, int> openedChickens = new Dictionary<ChickenTypes, int>
    {
        { ChickenTypes.RhodeIslandRed, 0 },
        { ChickenTypes.Leghorn, 0 },
        { ChickenTypes.PlymouthRock, 0 },
        { ChickenTypes.Australorp, 0 },
        { ChickenTypes.Orpington, 0 },
        { ChickenTypes.Sussex, 0 },
        { ChickenTypes.Brahma, 0 },
        { ChickenTypes.Silkie, 0 },
        { ChickenTypes.Wyandotte, 0 },
        { ChickenTypes.GreenLeggedPartridgeHen, 0 },
    };

    public List<ChickenTypes> boughtChickens = new List<ChickenTypes>();

    public void LoadData()
    {
        currentChicken = SaveManager.PlayerPrefs.LoadEnum(GameSaveKeys.CurrentChicken, ChickenTypes.RhodeIslandRed);
        coins = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Coins, 0);
        unlockedChicken = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.UnlockedChicken, 0);
        totalCoins = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalCoins, 0);
        totalTime = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalTime, 0);

        boughtChickens = SaveManager.PlayerPrefs.LoadEnumList<ChickenTypes>(GameSaveKeys.BoughtChickens);

        LoadOpenedChickens();
    }

    public void SaveData()
    {
        SaveManager.PlayerPrefs.SaveEnum(GameSaveKeys.CurrentChicken, currentChicken);
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Coins, coins);
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.UnlockedChicken, unlockedChicken);
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.TotalCoins, totalCoins);
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.TotalTime, totalTime);
        SaveManager.PlayerPrefs.SaveEnumList(GameSaveKeys.BoughtChickens, boughtChickens);

        SaveOpenedChickens();
    }

    private void LoadOpenedChickens()
    {
        foreach (var key in openedChickens.Keys.ToList())
        {
            // Використовуємо ім'я enum як ключ у PlayerPrefs
            string prefsKey = key.ToString();
            
            // Завантажуємо значення, за замовчуванням 0
            int savedValue = SaveManager.PlayerPrefs.LoadInt(prefsKey, 0);

            // Оновлюємо словник
            openedChickens[key] = savedValue;
        }
    }



    private void SaveOpenedChickens()
    {
        foreach (var pair in openedChickens)
        {
            SaveManager.PlayerPrefs.SaveInt(pair.Key.ToString(), pair.Value);
        }
    }

    public bool IsBought(ChickenTypes chicken)
    {
        foreach(var config in boughtChickens)
        {
            if(config == chicken)
            {
                return true;    
            }
        }
        return false;
    }
}
