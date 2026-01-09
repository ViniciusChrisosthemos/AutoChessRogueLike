using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public BoardConfigurationSO BoardConfiguration;
    public Transform BoardParent;
    public Transform BenchParent;

    private List<CharacterRuntime> _boardCharacters;
    private ReferencePosition[][] _boardPositions;
    private ReferencePosition[][] _benchPositions;

    public GameObject CellPrefab;

    public Transform CharacterParent;
    public CharacterMovementController CharacterControllerPrefab;

    private void Start()
    {
        var rows = BoardConfiguration.BoardRows;
        var columns = BoardConfiguration.BoardColumns;

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

        Debug.Log($"{rows} {columns} {width} {height} {x} {y}  {position}");

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

    public void MoveCharacter(CharacterMovementController characterController, Vector3 oldPosition, Vector3 newPosition)
    {
        var newBoardPos = GetBoardPosition(newPosition);

        Debug.Log($"newBoardPos {newBoardPos}");

        if (newBoardPos != (-1, -1))
        {
            if (!_boardPositions[newBoardPos.Item2][newBoardPos.Item1].Occuped)
            {
                characterController.transform.position = _boardPositions[newBoardPos.Item2][newBoardPos.Item1].Position;
                _boardPositions[newBoardPos.Item2][newBoardPos.Item1].Occuped = true;

                ClearOldPosition(characterController, oldPosition);

                characterController.CharacterRuntime.InBench = false;
            }
            else
            {
                characterController.transform.position = oldPosition;
            }
        }
        else
        {
            var newBenchPos = GetBenchPosition(newPosition);

            Debug.Log($"newBenchPos {newBoardPos}");

            if (newBenchPos != (-1, -1))
            {
                if (!_benchPositions[newBenchPos.Item2][newBenchPos.Item1].Occuped)
                {
                    characterController.transform.position = _benchPositions[newBenchPos.Item2][newBenchPos.Item1].Position;
                    _benchPositions[newBenchPos.Item2][newBenchPos.Item1].Occuped = true;

                    ClearOldPosition(characterController, oldPosition);

                    characterController.CharacterRuntime.InBench = true;
                }
                else
                {
                    characterController.transform.position = oldPosition;
                }
            }
            else
            {
                characterController.transform.position = oldPosition;
            }
        }
    }

    private void ClearOldPosition(CharacterMovementController characterController, Vector3 oldPosition)
    {
        if (characterController.CharacterRuntime.InBench)
        {
            var oldBenchPos = GetBenchPosition(oldPosition);

            _benchPositions[oldBenchPos.Item2][oldBenchPos.Item1].Occuped = false;
        }
        else
        {
            var oldBoardPos = GetBoardPosition(oldPosition);

            _boardPositions[oldBoardPos.Item2][oldBoardPos.Item1].Occuped = false;
        }
    }

    public void CreateCharacter(CharacterSO characterSO)
    {
        var characterController = Instantiate(CharacterControllerPrefab, CharacterParent);

        characterController.SetCharacter(this, characterSO, true);

        var benchPos = GetAvailableBenchPosition();

        characterController.transform.position = _benchPositions[benchPos.Item2][benchPos.Item1].Position;
        _benchPositions[benchPos.Item2][benchPos.Item1].Occuped = true;
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
