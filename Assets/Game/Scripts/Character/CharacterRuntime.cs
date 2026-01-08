using UnityEngine;

public class CharacterRuntime
{
    public CharacterSO CharacterData { get; private set; }

    public CharacterRuntime(CharacterSO characterData)
    {
        CharacterData = characterData;
    }
}
