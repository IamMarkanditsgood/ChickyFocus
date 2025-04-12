using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Chicken", menuName = "ScriptableObjects/Chicken", order = 1)]
public class ChickenConfig : ScriptableObject
{
    public ChickenTypes chickenType;
    public int hatchingTime;
    public int pauseTime;
    public int price;
    public int reward;

    public Sprite menuDefaultCard;
    public Sprite menuSelectCard;

    public Sprite shopCard;
}
