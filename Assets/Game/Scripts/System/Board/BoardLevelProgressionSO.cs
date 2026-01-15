using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardLevelProgression_", menuName = "ScriptableObjects/Board/Board Level Progress")]
public class BoardLevelProgressionSO : ScriptableObject
{
    [SerializeField] private List<BoardLevelData> _levelsData;

    public List<BoardLevelData> LevelsData => _levelsData;
}
