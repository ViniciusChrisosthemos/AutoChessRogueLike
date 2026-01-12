using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/System/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    public int StartingGold = 100;

    [Header("Shop")]
    public int ShopRefreshCost = 2;
    public int UpgradeCost = 4;
}
