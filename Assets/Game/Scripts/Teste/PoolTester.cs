using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoolTester : MonoBehaviour
{
    public CharacterPool characterPool;
    public CharacterPoolParametersSO poolParameters;
    public CharacterPoolAmountsSO poolAmountsSO;

    private List<CharacterSO> _characters = new List<CharacterSO>();

    private void Start()
    {
        characterPool = new CharacterPool();
        characterPool.InitPool(poolParameters, poolAmountsSO);
    }

    private void Update()
    {
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            RefreshShop();
        }
    }

    public void RefreshShop()
    {
        _characters = characterPool.GetSample(5);

        string message = "";

        foreach (var c in _characters)
        {
            message += c.CharacterName + "-" + c.Cost.Cost + " | ";
        }

        Debug.Log(message);
    }
}
