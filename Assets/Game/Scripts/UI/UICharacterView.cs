using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UICharacterView : UIItemController
{
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private UIListDisplay _keywordsListDisplay;
    [SerializeField] private TextMeshProUGUI _txtCost;
    [SerializeField] private TextMeshProUGUI _txtName;

    public void SetCharacter()
    {
    }

    protected override void HandleInit(object obj)
    {
        CharacterSO characterSO = obj as CharacterSO;

        _imgCharacter.sprite = characterSO.GetArt(CharacterSO.CharacterArtType.ShopArt);

        _keywordsListDisplay.SetItems(characterSO.Keywords, null);

        _txtName.text = characterSO.CharacterName;
        _txtCost.text = characterSO.Cost.Cost.ToString();
    }
}
