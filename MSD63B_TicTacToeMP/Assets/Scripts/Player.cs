using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public string nickname;
    public Fruit.FruitType assignedFruit;
    public Id id;

    public enum Id
    {
        Player1=1,
        Player2=2
    }
}
