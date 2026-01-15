using System.Collections.Generic;

public class BoardLevelController
{
    private int _currentExperience;
    private int _currentLevel;

    private List<BoardLevelData> _boardLevelsData;

    public BoardLevelController(BoardLevelProgressionSO boardLevelProgressionSO)
    {
        _currentExperience = 0;
        _currentLevel = 0;

        _boardLevelsData = boardLevelProgressionSO.LevelsData;
    }

    public bool AddExperience(int experience)
    {
        var hasLeveledUp = false;

        _currentExperience += experience;
        
        while (_currentLevel < _boardLevelsData.Count && _currentExperience >= _boardLevelsData[_currentLevel].Cost)
        {
            _currentExperience -= _boardLevelsData[_currentLevel].Cost;
            _currentLevel++;

            hasLeveledUp = true;
        }

        return hasLeveledUp;
    }

    public int CurrentLevel => _currentLevel;

    public int CurrentExperience => _currentExperience;

    public int ExperienceToNextLevel => IsMaxLevel() ? 0 : _boardLevelsData[_currentLevel].Cost;

    public BoardLevelData GetLevelData() => IsMaxLevel() ? _boardLevelsData[^1] : _boardLevelsData[_currentLevel];

    public bool IsMaxLevel() => _currentLevel >= _boardLevelsData.Count;
}
