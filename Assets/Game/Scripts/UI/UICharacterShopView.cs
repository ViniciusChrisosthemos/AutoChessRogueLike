using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICharacterShopView : MonoBehaviour
{
    [SerializeField] private BoardController _boardController;
    [SerializeField] private CharacterShopController _characterShopController;

    [SerializeField] private UIListDisplay _characterListDisplay;

    [SerializeField] private Button _btnRefreshShop;

    private void Start()
    {
        _btnRefreshShop.onClick.AddListener(RefreshShop);
        RefreshShop();
    }

    private void Update()
    {
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            RefreshShop();
        }
    }

    public void UpdateCharacters(List<CharacterSO> characters)
    {
        _characterListDisplay.SetItems(characters, HandleCharacterSelected);
    }

    public void RefreshShop()
    {
        var characters = _characterShopController.RefreshShop();

        UpdateCharacters(characters);
    }

    private void HandleCharacterSelected(UIItemController controller)
    {
        if (!_boardController.IsBenchFull())
        {
            var characterSO = controller.GetItem<CharacterSO>();

            _boardController.CreateCharacter(characterSO);

            var characterViewController = controller as UICharacterView;
            characterViewController.Hiden();
        }
    }
}
