using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class UICharacterShopView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CharacterShopController _characterShopController;
    [SerializeField] private UIListDisplay _characterListDisplay;
    [SerializeField] private UIListDisplay _characterProbabilitiesListDisplay;
    [SerializeField] private Button _btnRefreshShop;
    [SerializeField] private Button _btnUpgrade;
    [SerializeField] private GameObject _toDeleteView;
    [SerializeField] private TextMeshProUGUI _txtGold;
    [SerializeField] private Slider _sliderExperienceBar;
    [SerializeField] private TextMeshProUGUI _txtExperienceLabel;
    [SerializeField] private TextMeshProUGUI _txtCharactersInBoard;

    private void Start()
    {
        _btnUpgrade.onClick.AddListener(BuyExp);
        _btnRefreshShop.onClick.AddListener(RefreshShop);

        RefreshShop();

        GameStateController.Instance.OnDeleteCharacterRequest.AddListener(HandleDeleteCharacterRequest);
        GameStateController.Instance.OnCharactersInBoardChanged.AddListener(HandleCharactersInBoardChanged);

        _characterProbabilitiesListDisplay.SetItems(_characterShopController.GetProbabilities(), null);
    }

    private void HandleCharactersInBoardChanged(int currentCharactersInBoard, int maxCharactersInBoard)
    {
        _txtCharactersInBoard.text = $"{currentCharactersInBoard} / {maxCharactersInBoard}";
    }

    private void Update()
    {
        if (Keyboard.current.dKey.wasPressedThisFrame && GameStateController.Instance.CanBuyShopRefresh())
        {
            RefreshShop();
        }


        if (Keyboard.current.eKey.wasPressedThisFrame && GameStateController.Instance.CanBuyExperience())
        {
            BuyExp();
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
        UpdateShopUI();
    }

    public void BuyExp()
    {
        _characterShopController.BuyExperience();

        UpdateShopUI();
    }

    private void UpdateShopUI()
    {

        var gameStateController = GameStateController.Instance;

        _txtGold.text = gameStateController.GameState.Gold.ToString();

        _btnUpgrade.interactable = _characterShopController.CanBuyExperienceShop();
        _btnRefreshShop.interactable = _characterShopController.CanRefreshShop();

        _sliderExperienceBar.value = gameStateController.GetLevelProgress();
        _txtExperienceLabel.text = gameStateController.IsMaxLevel() ? "MAX" : $"{gameStateController.GetCurrentExperience()} / {gameStateController.GetExperienceToNextLevel()}";

        UpdateCharacterAvailability();
    }

    private void HandleCharacterSelected(UIItemController controller)
    {
        var characterSO = controller.GetItem<CharacterSO>();

        if (_characterShopController.CanBuyCharacter(characterSO))
        {
            _characterShopController.BuyCharacter(characterSO);

            var characterViewController = controller as UICharacterView;
            characterViewController.Hidden();

            UpdateShopUI();
        }
    }

    private void UpdateCharacterAvailability()
    {
        var gameState = GameStateController.Instance.GameState;

        foreach (var item in _characterListDisplay.GetItems())
        {
            var itemController = item as UICharacterView;

            itemController.SetAvailable(gameState.Gold >= itemController.GetCharacterSO().Cost.Cost);
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
