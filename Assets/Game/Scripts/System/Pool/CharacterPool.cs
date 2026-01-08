using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterPool
{
    private Dictionary<CostSO, List<ItemHolder<CharacterSO>>> _pool;
    private CharacterPoolParametersSO _poolParameters;

    public void InitPool(CharacterPoolParametersSO parameters, CharacterPoolAmountsSO characterPoolAmounts)
    {
        _pool = new Dictionary<CostSO, List<ItemHolder<CharacterSO>>>();

        foreach (var itemHolder in characterPoolAmounts.Characters)
        {
            var cost = itemHolder.Item.Cost;

            if (!_pool.ContainsKey(cost))
            {
                _pool[cost] = new List<ItemHolder<CharacterSO>>();
            }

            _pool[cost].Add(new ItemHolder<CharacterSO>(itemHolder.Item, itemHolder.Amount));
        }

        _poolParameters = parameters;
    }
    /*
    public void ReturnCharacters(List<CharacterSO> characters)
    {
        foreach (var character in characters)
        {
            var cost = character.Cost;
            
            if (_pool.ContainsKey(cost))
            {
                var characterHolder = _pool[cost].FirstOrDefault(c => c.Item == character);
                if (characterHolder != null)
                {
                    characterHolder.Amount++;
                }
            }
        }
    } 
    */
    public List<CharacterSO> GetSample(int sampleSize)
    {
        var samples = new List<CharacterSO>();
        var availableCosts = _pool.Keys.Where(key => _pool[key].Sum(item => item.Amount) > 0).ToList();

        for (int sampleIdx = 0; sampleIdx < sampleSize; sampleIdx++)
        {
            var randomCost = _poolParameters.GetRandomCost();

            var characters = _pool[randomCost];
            var availableCharacters = characters.Where(c => c.Amount > 0).ToList();

            var randomIndex = Random.Range(0, availableCharacters.Count);
            var sample = availableCharacters[randomIndex].Item;

            samples.Add(sample);
        }

        return samples;
    }
}
