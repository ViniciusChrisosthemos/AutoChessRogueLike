using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateController : Singleton<GameStateController>
{
    [Header("Data")]
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    [Header("References")]
    [SerializeField] private BoardManager _boardManager;

    [Header("Events")]
    public UnityEvent<bool> OnDeleteCharacterRequest;
    public UnityEvent<TraitsController> OnTraitsUpdated;
    public UnityEvent<int, int> OnCharactersInBoardChanged;

    private GameState _gameState;
    private TraitsController _traitControllers;

    private void Awake()
    {
        _gameState = new GameState(_gameSettingsSO);
        _traitControllers = new TraitsController();
        
        _boardManager.OnBoardLevelUp.AddListener(TriggerCharactersInBoardChanged);
        _boardManager.OnBoardUpdated.AddListener(TriggerCharactersInBoardChanged);
    }

    private void Start()
    {
        TriggerCharactersInBoardChanged();
    }

    private void TriggerCharactersInBoardChanged()
    {
        OnCharactersInBoardChanged?.Invoke(_boardManager.GetCharactersInBoardAmount(), _boardManager.GetMaxCharactersInBoardAmount());
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

    public void BuyExperience()
    {
        int cost = _gameState.GetExperienceCost();
        _gameState.RmvGold(cost);

        AddExperience(_gameState.GetExperienceValue());
    }

    public void AddExperience(int exp)
    {
        _boardManager.AddExperience(exp);
    }

    public bool CanBuyShopRefresh()
    {
        return _gameState.Gold >= _gameState.GetRefreshShopCost();
    }

    public bool CanBuyExperience()
    {
        var hasGold = _gameState.Gold >= _gameState.GetExperienceCost();
        var isNotMaxLevel = !_boardManager.IsMaxLevel();

        return hasGold && isNotMaxLevel;
    }

    public bool IsBoardFull()
    {
        return _boardManager.IsBoardFull();
    }

    public bool IsBenchFull()
    {
        return _boardManager.IsBenchFull();
    }

    public bool CanBuyCharacter(CharacterSO characterSO)
    {
        var hasGold = _gameState.CanBuyCharacter(characterSO);
        var hasSpace = !IsBenchFull();

        return hasGold && hasSpace;
    }

    public void BuyCharacter(CharacterSO characterSO)
    {
        _gameState.BuyCharacter(characterSO);

        _boardManager.AddCharacter(characterSO);
    }

    public int GetCurrentLevel()
    {
        return _boardManager.GetCurrentLevel();
    }

    public int GetCurrentExperience()
    {
        return _boardManager.GetCurrentExperience();
    }

    public int GetExperienceToNextLevel()
    {
        return _boardManager.GetExperienceToNextLevel();
    }

    public float GetLevelProgress()
    {
        return _boardManager.GetLevelProgress();
    }

    public bool IsMaxLevel()
    {
        return _boardManager.IsMaxLevel();
    }

    public bool IsMouseInShopArea { get; set; }

    public GameState GameState => _gameState;
}
