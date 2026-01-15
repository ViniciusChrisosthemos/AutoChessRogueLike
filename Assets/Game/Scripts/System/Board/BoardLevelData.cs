using System;
using UnityEngine;



[Serializable]
public class BoardLevelData
{
    [SerializeField] private int _cost;
    [SerializeField] private int _maxCharactersAmount;

    public int Cost => _cost;
    public int MaxCharactersAmount => _maxCharactersAmount;
}