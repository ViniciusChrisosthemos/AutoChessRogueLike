using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterShopController : MonoBehaviour
{
    public CharacterPool characterPool;
    public CharacterPoolParametersSO poolParameters;
    public CharacterPoolAmountsSO poolAmountsSO;

    private List<CharacterSO> _characters = new List<CharacterSO>();

    private void Awake()
    {
        characterPool = new CharacterPool();
        characterPool.InitPool(poolParameters, poolAmountsSO);
    }

    public List<CharacterSO> RefreshShop()
    {
        _characters = characterPool.GetSample(5);

        return _characters;
    }
}
