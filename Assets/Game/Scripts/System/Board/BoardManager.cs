using System;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private BoardConfigurationSO _boardConfigurationSO;
    [SerializeField] private BoardLevelProgressionSO _boardLevelProgressionSO;

    [Header("References")]
    [SerializeField] private BoardController _boardController;

    [Header("Events")]
    public UnityEvent OnBoardLevelUp;
    public UnityEvent OnBoardUpdated;

    private BoardLevelController _boardLevelController;


    private void Awake()
    {
        _boardLevelController = new BoardLevelController(_boardLevelProgressionSO);

        _boardController.OnBoardUpdated.AddListener(() => OnBoardUpdated?.Invoke());
    }

    private void Start()
    {
        _boardController.InitBoard(_boardConfigurationSO);
        _boardController.SetMaxCharactersInBoard(_boardLevelController.GetLevelData().MaxCharactersAmount);
    }

    public bool AddExperience(int experience)
    {
        var hasLevelUp = _boardLevelController.AddExperience(experience);
        
        if (hasLevelUp)
        {
            OnBoardLevelUp?.Invoke();

            _boardController.SetMaxCharactersInBoard(_boardLevelController.GetLevelData().MaxCharactersAmount);
        }

        return hasLevelUp;
    }

    public bool IsBenchFull()
    {
        return _boardController.IsBenchFull();
    }

    public bool IsBoardFull()
    {
        return _boardLevelController.GetLevelData().MaxCharactersAmount <= _boardController.CharactersOnBoard.Count;
    }

    public bool IsMaxLevel()
    {
        return _boardLevelController.IsMaxLevel();
    }

    public void AddCharacter(CharacterSO characterSO)
    {
        _boardController.CreateCharacter(characterSO);
    }

    public float GetLevelProgress()
    {
        if (IsMaxLevel())
            return 1f;

        var levelData = _boardLevelController.GetLevelData();

        return (float)_boardLevelController.CurrentExperience / levelData.Cost;
    }

    public int GetCurrentLevel()
    {
        return _boardLevelController.CurrentLevel;
    }

    public int GetExperienceToNextLevel()
    {
        return _boardLevelController.ExperienceToNextLevel;
    }

    public int GetCurrentExperience()
    {
        return _boardLevelController.CurrentExperience;
    }

    public int GetCharactersInBoardAmount()
    {
        return _boardController.CharactersOnBoard.Count;
    }

    internal int GetMaxCharactersInBoardAmount()
    {
        return _boardLevelController.GetLevelData().MaxCharactersAmount;
    }
}
