using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TraitsController
{
    private List<TraitRuntime> _traits;

    public void UpdateTraits(List<CharacterSO> characters)
    {
        var charactersSet = new HashSet<CharacterSO>(characters);

        _traits = new List<TraitRuntime>();
        var keywordSet = new HashSet<KeywordSO>();

        foreach (var character in charactersSet)
        {
            foreach (var keyword in character.Keywords)
            {
                keywordSet.Add(keyword);
            }
        }

        foreach (var keyword in keywordSet)
        {
            var matchingCharacters = charactersSet.Where(c => c.Keywords.Contains(keyword)).ToList();
            
            var traitRuntime = new TraitRuntime(keyword.Trait, matchingCharacters.Count);

            _traits.Add(traitRuntime);
        }
    }

    public List<TraitRuntime> GetTraits()
    {
        return _traits;
    }
}
