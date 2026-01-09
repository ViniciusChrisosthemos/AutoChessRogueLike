using UnityEngine;

[CreateAssetMenu(fileName = "Keyword_", menuName = "ScriptableObjects/Character/Keyword")]
public class KeywordSO : ScriptableObject
{
    public string Name;
    public Sprite Art;

    public AbstractBaseTrait Trait;
}
