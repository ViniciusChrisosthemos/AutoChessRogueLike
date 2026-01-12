using System;
using UnityEngine;

public class GameState
{
    private GameSettingsSO _gameSettings;

    public int Gold { get; private set; } = 0;

    public GameState(GameSettingsSO gameSettings)
    {
        _gameSettings = gameSettings;
        
        Gold = _gameSettings.StartingGold;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void RmvGold(int amount)
    {
        Gold -= amount;
    }

    public int GetRefreshShopCost()
    {
        return _gameSettings.ShopRefreshCost;
    }

    public int GetUpgradeCost()
    {
        return _gameSettings.UpgradeCost;
    }

    public bool CanBuyCharacter(CharacterSO characterSO)
    {
        return Gold >= characterSO.Cost.Cost;
    }

    public void BuyCharacter(CharacterSO characterSO)
    {
        RmvGold(characterSO.Cost.Cost);
    }
}
