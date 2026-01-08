using UnityEngine;

[CreateAssetMenu(fileName = "BoardConfigurationSO", menuName = "ScriptableObjects/Board/BoardConfigurationSO")]
public class BoardConfigurationSO : ScriptableObject
{
    [Header("Board Settings")]
    public int BoardRows = 7;
    public int BoardColumns = 7;

    [Header("Bench Settings")]
    public int BenchRows = 1;
    public int BenchColumns = 7;

    [Header("Cell Settings")]
    public float CellSizeX = 5;
    public float CellSizeZ = 5;
    public float CellSpacingX = 0.5f;
    public float CellSpacingY = 0.5f;
}
