using UnityEngine;
using UnityEngine.Events;

public class CharacterRuntime
{
    private const int COPIES_TO_LEVEL_UP = 3;

    public CharacterSO CharacterData { get; private set; }
    public bool InBench { get; set; }
    public int Stars { get; set; } = 1;

    public UnityEvent OnLevelUp = new UnityEvent();

    public CharacterRuntime(CharacterSO characterData, bool inBench)
    {
        CharacterData = characterData;
        InBench = inBench;
    }

    public void LevelUp()
    {
        Stars++;

        OnLevelUp?.Invoke();
    }

    public int GetCopiesToLevelUp()
    {
        return COPIES_TO_LEVEL_UP;
    }
}
