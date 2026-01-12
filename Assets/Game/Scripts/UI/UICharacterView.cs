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

    private CharacterSO _characterSO;

    protected override void HandleInit(object obj)
    {
        _characterSO = obj as CharacterSO;

        _imgCharacterBackground.color = _characterSO.Color;
        _imgCharacter.sprite = _characterSO.GetArt(CharacterSO.CharacterArtType.ShopArt);

        _keywordsListDisplay.SetItems(_characterSO.Keywords, null);

        _txtName.text = _characterSO.CharacterName;
        _txtCost.text = _characterSO.Cost.Cost.ToString();

        _btnButton.onClick.AddListener(() => SelectItem());

        _view.SetActive(true);
    }

    public void Hidden()
    {
        _view.SetActive(false);
    }

    public CharacterSO GetCharacterSO()
    {
        return _characterSO;
    }

    public void SetAvailable(bool isAvailable)
    {
        _imgCharacter.color = isAvailable ? Color.white : Color.gray;
    }
}
