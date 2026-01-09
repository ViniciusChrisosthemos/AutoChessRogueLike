using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterShopController : MonoBehaviour
{
    [SerializeField] private CharacterPool _characterPool;
    [SerializeField] private CharacterPoolParametersSO _poolParameters;
    [SerializeField] private CharacterPoolAmountsSO _poolAmountsSO;

    private List<CharacterSO> _characters = new List<CharacterSO>();

    private void Awake()
    {
        _characterPool = new CharacterPool();
        _characterPool.InitPool(_poolParameters, _poolAmountsSO);
    }

    public List<CharacterSO> RefreshShop()
    {
        _characters = _characterPool.GetSample(5);

        return _characters;
    }

    public List<ProbabilityHolder<CostSO>> GetProbabilities()
    {
        return _poolParameters.CharacterAmounts;
    }
}
