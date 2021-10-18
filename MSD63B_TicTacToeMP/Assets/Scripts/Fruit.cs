using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private FruitType fruitType;

    public Fruit()
    {
        this.fruitType = FruitType.None;
    }

    public void SetFruit(FruitType fruitType)
    {
        this.fruitType = fruitType;
    }

    public FruitType GetFruit()
    {
        return this.fruitType;
    }

    public enum FruitType
    {
        None,
        Strawberry,
        Apple
    }

}
