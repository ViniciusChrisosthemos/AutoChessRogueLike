using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterPoolAmountsSO", menuName = "ScriptableObjects/System/Pool/Character Poll Amounts")]
public class CharacterPoolAmountsSO : ScriptableObject
{
    public List<ItemHolder<CharacterSO>> Characters;
}
