using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Character/Character")]
public class CharacterSO : ScriptableObject
{
    public enum CharacterArtType
    {
        ShopArt
    }

    public string CharacterName;
    public CostSO Cost;
    public List<KeywordSO> Keywords;

    // Art
    public Sprite ShopArt;

    public Sprite GetArt(CharacterArtType artType)
    {
        switch (artType)
        {
            case CharacterArtType.ShopArt: return ShopArt;
            default: return ShopArt;
        }
    }
}
