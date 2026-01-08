using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public BoardConfigurationSO BoardConfiguration;
    public Transform BoardParent;
    public Transform BenchParent;

    private List<CharacterRuntime> _boardCharacters;
    private bool[][] _boardPositions;
    private bool[][] _benchPositions;

    public GameObject CellPrefab;

    private void Start()
    {
        var rows = BoardConfiguration.BoardRows;
        var columns = BoardConfiguration.BoardColumns;

        InitBoard(rows, columns);
    }

    private void InitBoard(int rows, int colums)
    {
        _boardPositions = new bool[rows][];

        for (int i = 0; i < rows; i++)
        {
            _boardPositions[i] = new bool[colums];
            for (int j = 0; j < colums; j++)
            {
                _boardPositions[i][j] = false;
            }
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
            }
        }

        var benchRows = BoardConfiguration.BenchRows;
        var benchColumns = BoardConfiguration.BenchColumns;

        _benchPositions = new bool[benchRows][];

        for (int y = 0; y < benchRows; y++)
        {
            _benchPositions[y] = new bool[benchColumns];
            for (int x = 0; x < benchColumns; x++)
            {
                _benchPositions[y][x] = false;

                var cell = CreateCell(x, y,
                    BoardConfiguration.CellSpacingX,
                    BoardConfiguration.CellSpacingY,
                    BoardConfiguration.CellSizeX,
                    0.1f,
                    BoardConfiguration.CellSizeZ,
                    offset,
                    BenchParent);

                cell.name = $"Bench-Cell_{x}_{y}";
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
    
    public (int, int) GetPosition(Transform obj, bool[][] grid, Transform parent)
    {
        var rows = grid.Length;
        var columns = grid[0].Length;

        var width = (BoardConfiguration.CellSizeX * columns + BoardConfiguration.CellSpacingX * (columns - 1));
        var height = (BoardConfiguration.CellSizeZ * rows + BoardConfiguration.CellSpacingY * (rows - 1));

        var x = Mathf.FloorToInt(columns * (obj.position.x - parent.position.x));
        var y = Mathf.FloorToInt(rows * (obj.position.z - parent.position.z));

        if (x < 0 || y < 0 || x > width || y > height)
        {
            return (-1, -1);
        }

        return (x, y);
    }

    public (int, int) GetBenchPosition(Transform obj)
    {
        return GetPosition(obj, _benchPositions, BenchParent);
    }

    public (int, int) GetBoardPosition(Transform obj)
    {
        return GetPosition(obj, _boardPositions, BoardParent);
    }
}
