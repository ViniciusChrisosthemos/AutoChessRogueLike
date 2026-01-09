using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public BoardConfigurationSO BoardConfiguration;
    public Transform BoardParent;
    public Transform BenchParent;

    private List<CharacterMovementController> _characterControllers;
    private ReferencePosition[][] _boardPositions;
    private ReferencePosition[][] _benchPositions;

    public GameObject CellPrefab;

    public Transform CharacterParent;
    public CharacterMovementController CharacterControllerPrefab;

    private void Start()
    {
        var rows = BoardConfiguration.BoardRows;
        var columns = BoardConfiguration.BoardColumns;

        _characterControllers = new List<CharacterMovementController>();

        InitBoard(rows, columns);
    }

    private void InitBoard(int rows, int colums)
    {
        _boardPositions = new ReferencePosition[rows][];

        for (int i = 0; i < rows; i++)
        {
            _boardPositions[i] = new ReferencePosition[colums];
        }

        var offset = new Vector3(
            BoardConfiguration.CellSizeX / 2,
            0,
            BoardConfiguration.CellSizeZ / 2
        );

        for (int y= 0; y < rows; y++)
        {
            for (int x = 0; x < colums; x++)
            {
                var cell = CreateCell(x, y,
                    BoardConfiguration.CellSpacingX,
                    BoardConfiguration.CellSpacingY,
                    BoardConfiguration.CellSizeX,
                    0.1f,
                    BoardConfiguration.CellSizeZ,
                    offset,
                    BoardParent);

                cell.name = $"Board-Cell_{x}_{y}";

                _boardPositions[y][x] = new ReferencePosition(cell.transform.position, false);
            }
        }

        var benchRows = BoardConfiguration.BenchRows;
        var benchColumns = BoardConfiguration.BenchColumns;

        _benchPositions = new ReferencePosition[benchRows][];

        for (int y = 0; y < benchRows; y++)
        {
            _benchPositions[y] = new ReferencePosition[benchColumns];
            for (int x = 0; x < benchColumns; x++)
            {
                var cell = CreateCell(x, y,
                    BoardConfiguration.CellSpacingX,
                    BoardConfiguration.CellSpacingY,
                    BoardConfiguration.CellSizeX,
                    0.1f,
                    BoardConfiguration.CellSizeZ,
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

        var width = (BoardConfiguration.CellSizeX * columns + BoardConfiguration.CellSpacingX * (columns - 1));
        var height = (BoardConfiguration.CellSizeZ * rows + BoardConfiguration.CellSpacingY * (rows - 1));

        var x = Mathf.FloorToInt(columns * ((position.x - parent.position.x) / width));
        var y = Mathf.FloorToInt(rows * ((position.z - parent.position.z) / height));

        //Debug.Log($"{rows} {columns}  {width} {height}  {x} {y}   {position}   {parent.position}    {columns * ((position.x - parent.position.x) / width)} {rows * ((position.z - parent.position.z) / height)}");

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
        Debug.Log($"{characterController.CharacterRuntime.InBench}  {newPosition}");

        var newBoardPos = GetBoardPosition(newPosition);

        if (newBoardPos != (-1, -1))
        {
            Debug.Log($"New Board Pos   {newBoardPos}");
            if (!_boardPositions[newBoardPos.Item2][newBoardPos.Item1].Occuped)
            {
                characterController.transform.position = _boardPositions[newBoardPos.Item2][newBoardPos.Item1].Position;
                _boardPositions[newBoardPos.Item2][newBoardPos.Item1].Occuped = true;

                ClearOldPosition(characterController, characterController.OldPosition);

                characterController.CharacterRuntime.InBench = false;
            }
            else
            {
                characterController.transform.position = characterController.OldPosition;
            }
        }
        else
        {
            var newBenchPos = GetBenchPosition(newPosition);

            Debug.Log($"New Bench Pos   {newBenchPos}");

            if (newBenchPos != (-1, -1))
            {
                if (!_benchPositions[newBenchPos.Item2][newBenchPos.Item1].Occuped)
                {
                    characterController.transform.position = _benchPositions[newBenchPos.Item2][newBenchPos.Item1].Position;
                    _benchPositions[newBenchPos.Item2][newBenchPos.Item1].Occuped = true;

                    ClearOldPosition(characterController, characterController.OldPosition);

                    characterController.CharacterRuntime.InBench = true;
                }
                else
                {
                    characterController.transform.position = characterController.OldPosition;
                }
            }
            else
            {
                characterController.transform.position = characterController.OldPosition;
            }
        }
    }

    private void ClearOldPosition(CharacterMovementController characterController, Vector3 oldPosition)
    {
        Debug.Log($"==>   {oldPosition}");

        if (characterController.CharacterRuntime.InBench)
        {
            var oldBenchPos = GetBenchPosition(oldPosition);

            Debug.Log(oldBenchPos);

            _benchPositions[oldBenchPos.Item2][oldBenchPos.Item1].Occuped = false;


            Debug.Log($"       {BenchParent.position}");
        }
        else
        {
            var oldBoardPos = GetBoardPosition(oldPosition);

            Debug.Log(oldBoardPos);

            _boardPositions[oldBoardPos.Item2][oldBoardPos.Item1].Occuped = false;


            Debug.Log($"       {BoardParent.position}");
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
