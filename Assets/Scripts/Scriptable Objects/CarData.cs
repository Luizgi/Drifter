using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Car Data", menuName = "carData")]
public class CarData : ScriptableObject
{
    [SerializeField] int carUniqueID = 0;
    [SerializeField] Sprite carUISprite;
    [SerializeField]  GameObject carPrefab;


    public int CarUniqueID
    {
        get { return carUniqueID; }
    }

    public Sprite CarUISprite
    {
        get { return carUISprite; }
    }

    public GameObject CarPrefab
    {
        get { return carPrefab; }
    }
}
