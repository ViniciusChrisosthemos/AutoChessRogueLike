using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UICharacterView : UIItemController
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnButton;
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private Image _imgCharacterBackground;
    [SerializeField] private UIListDisplay _keywordsListDisplay;
    [SerializeField] private TextMeshProUGUI _txtCost;
    [SerializeField] private TextMeshProUGUI _txtName;

    protected override void HandleInit(object obj)
    {
        CharacterSO characterSO = obj as CharacterSO;

        _imgCharacterBackground.color = characterSO.Color;
        _imgCharacter.sprite = characterSO.GetArt(CharacterSO.CharacterArtType.ShopArt);

        _keywordsListDisplay.SetItems(characterSO.Keywords, null);

        _txtName.text = characterSO.CharacterName;
        _txtCost.text = characterSO.Cost.Cost.ToString();

        _btnButton.onClick.AddListener(() => SelectItem());

        _view.SetActive(true);
    }

    public void Hiden()
    {
        _view.SetActive(false);
    }
}
