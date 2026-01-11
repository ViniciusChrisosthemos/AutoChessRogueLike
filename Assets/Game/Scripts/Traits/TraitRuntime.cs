using UnityEngine;

public class TraitRuntime
{
    private AbstractBaseTrait _traitData;
    private int _currentCharacters;

    public TraitRuntime(AbstractBaseTrait traitData, int currentCharacters)
    {
        _traitData = traitData;
        _currentCharacters = currentCharacters;
    }

    public AbstractBaseTrait TraitData => _traitData;
    public int CurrentCharacters => _currentCharacters;
}
