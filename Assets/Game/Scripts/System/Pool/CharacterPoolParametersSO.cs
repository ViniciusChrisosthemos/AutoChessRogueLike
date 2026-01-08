using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "CharacterPoolParameters_", menuName = "ScriptableObjects/System/Pool/Pool Probabilities")]
public class CharacterPoolParametersSO : ScriptableObject
{
    public List<ProbabilityHolder<CostSO>> CharacterAmounts;

    public CostSO GetRandomCost()
    {
        var randomVal= Random.Range(0f, 1f);

        for (var i = 0; i < CharacterAmounts.Count - 1; i++)
        {
            var probabilityHolder = CharacterAmounts[i];
            if (randomVal <= probabilityHolder.Probability)
            {
                return probabilityHolder.Item;
            }
        }

        return CharacterAmounts[CharacterAmounts.Count - 1].Item;
    }
}