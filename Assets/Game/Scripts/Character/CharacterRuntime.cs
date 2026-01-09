using UnityEngine;

public class CharacterRuntime
{
    public CharacterSO CharacterData { get; private set; }
    public bool InBench { get; set; }

    public CharacterRuntime(CharacterSO characterData, bool inBench)
    {
        CharacterData = characterData;
        InBench = inBench;
    }
}
