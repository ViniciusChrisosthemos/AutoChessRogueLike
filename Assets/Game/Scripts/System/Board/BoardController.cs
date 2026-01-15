using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class BoardController : MonoBehaviour
{

    [Header("References")]
    public Transform BoardParent;
    public Transform BenchParent;
    public GameObject CellPrefab;
    public Transform CharacterParent;
    public CharacterMovementController CharacterControllerPrefab;

    [Header("Events")]
    public UnityEvent OnBoardUpdated;

    private List<CharacterMovementController> _characterControllers;
    private ReferencePosition[][] _boardPositions;
    private ReferencePosition[][] _benchPositions;

    private BoardConfigurationSO _boardConfiguration;
    private int _maxCharactersOnBoard = 0;

    public void InitBoard(BoardConfigurationSO boardConfigurationSO)
    {
        _boardConfiguration = boardConfigurationSO;

        var rows = boardConfigurationSO.BoardRows;
        var columns = boardConfigurationSO.BoardColumns;

        _characterControllers = new List<CharacterMovementController>();
        _boardPositions = new ReferencePosition[rows][];

        for (int i = 0; i < rows; i++)
        {
            _boardPositions[i] = new ReferencePosition[columns];
        }

        var offset = new Vector3(
            _boardConfiguration.CellSizeX / 2,
            0,
            _boardConfiguration.CellSizeZ / 2
        );

        for (int y= 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var cell = CreateCell(x, y,
                    _boardConfiguration.CellSpacingX,
                    _boardConfiguration.CellSpacingY,
                    _boardConfiguration.CellSizeX,
                    0.1f,
                    _boardConfiguration.CellSizeZ,
                    offset,
                    BoardParent);

                cell.name = $"Board-Cell_{x}_{y}";

                _boardPositions[y][x] = new ReferencePosition(cell.transform.position, false);
            }
        }

        var benchRows = _boardConfiguration.BenchRows;
        var benchColumns = _boardConfiguration.BenchColumns;

        _benchPositions = new ReferencePosition[benchRows][];

        for (int y = 0; y < benchRows; y++)
        {
            _benchPositions[y] = new ReferencePosition[benchColumns];
            for (int x = 0; x < benchColumns; x++)
            {
                var cell = CreateCell(x, y,
                    _boardConfiguration.CellSpacingX,
                    _boardConfiguration.CellSpacingY,
                    _boardConfiguration.CellSizeX,
                    0.1f,
                    _boardConfiguration.CellSizeZ,
                    offset,
                    BenchParent);

                cell.name = $"Bench-Cell_{x}_{y}";

                _benchPositions[y][x] = new ReferencePosition(cell.transform.position, false);
            }
        }
    }

    private GameObject CreateCell(int x, int y, float spacingX, float spacingZ, float sizeX, float sizeY, float sizeZ, Vector3 offset, Transform parent)
    {
        var cell = Instantiate(CellPrefab, parent);

        cell.transform.localPosition = new Vector3(x * sizeX, sizeY, y * sizeZ);
        cell.transform.localPosition += new Vector3(spacingX * x, 0, spacingZ * y);
        cell.transform.localPosition += offset;
        cell.transform.localRotation = Quaternion.identity;
        cell.transform.localScale = new Vector3(sizeX, sizeY, sizeZ) * 0.95f;

        return cell;
    }
    
    public (int, int) GetPosition(Vector3 position, ReferencePosition[][] grid, Transform parent)
    {
        var rows = grid.Length;
        var columns = grid[0].Length;

        var width = (_boardConfiguration.CellSizeX * columns + _boardConfiguration.CellSpacingX * (columns - 1));
        var height = (_boardConfiguration.CellSizeZ * rows + _boardConfiguration.CellSpacingY * (rows - 1));

        var x = Mathf.FloorToInt(columns * ((position.x - parent.position.x) / width));
        var y = Mathf.FloorToInt(rows * ((position.z - parent.position.z) / height));

        Debug.Log($"{rows} {columns}  {width} {height}  {x} {y}   {position}   {parent.position}    {columns * ((position.x - parent.position.x) / width)} {rows * ((position.z - parent.position.z) / height)}");

        if (x < 0 || y < 0 || x >= columns || y >= rows)
        {
            return (-1, -1);
        }

        return (x, y);
    }

    public (int, int) GetBenchPosition(Vector3 position)
    {
        return GetPosition(position, _benchPositions, BenchParent);
    }

    public (int, int) GetBoardPosition(Vector3 position)
    {
        return GetPosition(position, _boardPositions, BoardParent);
    }

    public void MoveCharacter(CharacterMovementController characterController, Vector3 newPosition)
    {
        //Debug.Log($"{characterController.CharacterRuntime.InBench}  {newPosition}");

        var hasChanged = false;
        var newBoardPos = GetBoardPosition(newPosition);

        if (newBoardPos != (-1, -1))
        {
            //Debug.Log($"New Board Pos   {newBoardPos}");
            if (!_boardPositions[newBoardPos.Item2][newBoardPos.Item1].Occuped)
            {
                Debug.Log("Move Character");

                Debug.Log($"    {CharactersOnBoard.Count}  {IsBoardFull()} ");
                if (characterController.CharacterRuntime.InBench && IsBoardFull())
                {
                    characterController.SetPosition(characterController.OldPosition);
                }
                else
                {
                    _boardPositions[newBoardPos.Item2][newBoardPos.Item1].Occuped = true;

                    ClearOldPosition(characterController, characterController.OldPosition);

                    characterController.SetPosition(_boardPositions[newBoardPos.Item2][newBoardPos.Item1].Position);

                    if (characterController.CharacterRuntime.InBench)
                    {
                        characterController.CharacterRuntime.InBench = false;
                        hasChanged = true;
                    }
                }
            }
            else
            {
                characterController.SetPosition(characterController.OldPosition);
            }
        }
        else
        {
            var newBenchPos = GetBenchPosition(newPosition);

            //Debug.Log($"New Bench Pos   {newBenchPos}");

            if (newBenchPos != (-1, -1))
            {
                if (!_benchPositions[newBenchPos.Item2][newBenchPos.Item1].Occuped)
                {
                    _benchPositions[newBenchPos.Item2][newBenchPos.Item1].Occuped = true;

                    ClearOldPosition(characterController, characterController.OldPosition);

                    characterController.SetPosition(_benchPositions[newBenchPos.Item2][newBenchPos.Item1].Position);

                }
                else
                {
                    characterController.SetPosition(characterController.OldPosition);
                }


                if (!characterController.CharacterRuntime.InBench)
                {
                    characterController.CharacterRuntime.InBench = true;
                    hasChanged = true;
                }
            }
            else
            {
                characterController.SetPosition(characterController.OldPosition);
            }
        }

        if (hasChanged)
        {
            TriggerBoardUpdate();
        }
    }

    private void ClearOldPosition(CharacterMovementController characterController, Vector3 oldPosition)
    {
        //Debug.Log($"==>   {oldPosition}");

        if (characterController.CharacterRuntime.InBench)
        {
            var oldBenchPos = GetBenchPosition(oldPosition);

            //Debug.Log($"    BenchPos   {oldBenchPos}    {BenchParent.position}");

            _benchPositions[oldBenchPos.Item2][oldBenchPos.Item1].Occuped = false;
        }
        else
        {
            var oldBoardPos = GetBoardPosition(oldPosition);

            //Debug.Log($"    BoardPos   {oldBoardPos}    {BoardParent.position}  {characterController.transform.position} {characterController.OldPosition}");

            _boardPositions[oldBoardPos.Item2][oldBoardPos.Item1].Occuped = false;
        }
    }

    public void CreateCharacter(CharacterSO characterSO)
    {
        var characterController = Instantiate(CharacterControllerPrefab, CharacterParent);

        var benchPos = GetAvailableBenchPosition();

        characterController.transform.position = _benchPositions[benchPos.Item2][benchPos.Item1].Position;
        _benchPositions[benchPos.Item2][benchPos.Item1].Occuped = true;

        characterController.SetCharacter(this, characterSO, true, _benchPositions[benchPos.Item2][benchPos.Item1].Position);

        _characterControllers.Add(characterController);

        CheckCharacterCopies(characterSO);
    }

    private void CheckCharacterCopies(CharacterSO characterSO)
    {
        var copiesControllers = _characterControllers.FindAll(c => c.CharacterRuntime.CharacterData == characterSO);

        var starsGroup = copiesControllers.GroupBy(c => c.CharacterRuntime.Stars).OrderBy(item => item.Key);

        bool hasUpgraded = false;

        foreach (var group in starsGroup)
        {
            if (group.Count() >= 3)
            {
                var characters = group.Where(item => item.CharacterRuntime.InBench).ToList();
                characters.AddRange(group.Where(item => !item.CharacterRuntime.InBench).ToList());

                var charactersToDelete = characters.Take(2).ToList();
                var characterToUpgrade = characters.Last();

                charactersToDelete.ForEach(c => DeleteCharacter(c));

                characterToUpgrade.CharacterRuntime.LevelUp();

                hasUpgraded = true;

                break;
            }
        }

        if (hasUpgraded)
        {
            CheckCharacterCopies(characterSO);

            OnBoardUpdated?.Invoke();
        }
    }

    public bool IsBenchFull()
    {
        for (int i = 0; i < _benchPositions.Length; i++)
        {
            for (int j = 0; j < _benchPositions[0].Length; j++)
            {
                if (!_benchPositions[i][j].Occuped) return false;
            }
        }

        return true;
    }

    public bool IsBoardFull()
    {
        return CharactersOnBoard.Count >= _maxCharactersOnBoard;
    }

    public (int, int) GetAvailableBenchPosition()
    {
        for (int i = 0; i < _benchPositions.Length; i++)
        {
            for (int j = 0; j < _benchPositions[0].Length; j++)
            {
                if (!_benchPositions[i][j].Occuped) return (j, i);
            }
        }

        return (-1, -1);
    }

    public void DeleteCharacter(CharacterMovementController characterMovementController)
    {
        ClearOldPosition(characterMovementController, characterMovementController.OldPosition);
        _characterControllers.Remove(characterMovementController);
        Destroy(characterMovementController.gameObject);
    }

    private void TriggerBoardUpdate()
    {
        Debug.Log("Trigger Board Update");

        foreach (var c in _characterControllers)
        {
                       Debug.Log($" - Character  {c.CharacterRuntime.CharacterData.name}   InBench: {c.CharacterRuntime.InBench}");
        }

        var characters = _characterControllers.Select(c => c.CharacterRuntime.CharacterData).Distinct().ToList();
        var charactersInBoard = _characterControllers.Where(c => !c.CharacterRuntime.InBench).Select(c => c.CharacterRuntime.CharacterData).ToList();

        Debug.Log($"Trigger Board Update {charactersInBoard.Count}");

        GameStateController.Instance.UpdateTrais(charactersInBoard);

        OnBoardUpdated?.Invoke();
    }

    public void SetMaxCharactersInBoard(int maxCharactersInBoard)
    {
        _maxCharactersOnBoard = maxCharactersInBoard;
    }

    public List<CharacterMovementController> CharactersOnBoard => _characterControllers.Where(c => !c.CharacterRuntime.InBench).ToList();

    public class ReferencePosition
    {
        public Vector3 Position;
        public bool Occuped;

        public ReferencePosition(Vector3 position, bool occuped)
        {
            Position = position;
            Occuped = occuped;
        }
    }
}
