using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateController : Singleton<GameStateController>
{
    public GameSettingsSO _gameSettingsSO;

    public UnityEvent<bool> OnDeleteCharacterRequest;
    public UnityEvent<TraitsController> OnTraitsUpdated;

    private GameState _gameState;
    private TraitsController _traitControllers;

    private void Awake()
    {
        _gameState = new GameState(_gameSettingsSO);
        _traitControllers = new TraitsController();
    }

    public void UpdateTrais(List<CharacterSO> characters)
    {
        _traitControllers.UpdateTraits(characters);

        OnTraitsUpdated?.Invoke(_traitControllers);
    }

    public void TriggerCharacterToDeleteRequest()
    {
        OnDeleteCharacterRequest?.Invoke(true);
    }

    public void TriggerCharacterToDeleteCancel()
    {
        OnDeleteCharacterRequest?.Invoke(false);
    }

    public void BuyShopRefresh()
    {
        int cost = _gameState.GetRefreshShopCost();
        _gameState.RmvGold(cost);
    }

    public void BuyUpgrade()
    {
        int cost = _gameState.GetUpgradeCost();
        _gameState.RmvGold(cost);
    }

    public bool CanBuyShopRefresh()
    {
        return _gameState.Gold >= _gameState.GetRefreshShopCost();
    }

    public bool CanBuyUpgrade()
    {
        return _gameState.Gold >= _gameState.GetUpgradeCost();
    }

    public bool IsMouseInShopArea { get; set; }

    public GameState GameState => _gameState;
}
