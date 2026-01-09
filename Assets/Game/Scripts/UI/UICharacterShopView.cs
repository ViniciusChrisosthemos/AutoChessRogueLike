using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICharacterShopView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BoardController _boardController;
    [SerializeField] private CharacterShopController _characterShopController;
    [SerializeField] private UIListDisplay _characterListDisplay;
    [SerializeField] private UIListDisplay _characterProbabilitiesListDisplay;
    [SerializeField] private Button _btnRefreshShop;
    [SerializeField] private GameObject _toDeleteView;

    private void Start()
    {
        _btnRefreshShop.onClick.AddListener(RefreshShop);
        RefreshShop();

        GameStateController.Instance.OnDeleteCharacterRequest.AddListener(HandleDeleteCharacterRequest);

        _characterProbabilitiesListDisplay.SetItems(_characterShopController.GetProbabilities(), null);
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

    private void HandleDeleteCharacterRequest(bool toDelete)
    {
        _toDeleteView.SetActive(toDelete);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameStateController.Instance.IsMouseInShopArea = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameStateController.Instance.IsMouseInShopArea = true;
    }
}
