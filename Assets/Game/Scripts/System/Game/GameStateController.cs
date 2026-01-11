using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateController : Singleton<GameStateController>
{

    public UnityEvent<bool> OnDeleteCharacterRequest;
    public UnityEvent<TraitsController> OnTraitsUpdated;

    private TraitsController _traitControllers;

    private void Awake()
    {
        _traitControllers = new TraitsController();
    }


    public void UpdateTrais(List<CharacterSO> characters)
    {
        _traitControllers.UpdateTraits(characters);

        OnTraitsUpdated?.Invoke(_traitControllers);
    }

    public void TriggerCharacterToDeleteRequest()
    {
        OnDeleteCharacterRequest?.Invoke(true);
    }

    public void TriggerCharacterToDeleteCancel()
    {
        OnDeleteCharacterRequest?.Invoke(false);
    }


    public bool IsMouseInShopArea { get; set; }
}
