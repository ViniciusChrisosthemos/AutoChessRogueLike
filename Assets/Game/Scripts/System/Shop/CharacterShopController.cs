using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
        var gameState = GameStateController.Instance.GameState;

        var cost = gameState.GetRefreshShopCost();
        gameState.RmvGold(cost);

        _characters = _characterPool.GetSample(5);

        return _characters;
    }

    public bool CanRefreshShop()
    {
        var gameState = GameStateController.Instance.GameState;
        
        return gameState.Gold >= gameState.GetRefreshShopCost();
    }

    public bool CanUpgradeShop()
    {
        var gameState = GameStateController.Instance.GameState;
        
        return gameState.Gold >= gameState.GetUpgradeCost();
    }

    public List<ProbabilityHolder<CostSO>> GetProbabilities()
    {
        return _poolParameters.CharacterAmounts;
    }

    public void BuyExperience()
    {
        var gameState = GameStateController.Instance.GameState;

        int cost = gameState.GetUpgradeCost();
        gameState.RmvGold(cost);
    }
}
