using System;
using UnityEngine;
using UnityEngine.Events;

public class GameStateController : Singleton<GameStateController>
{
    public UnityEvent<bool> OnDeleteCharacterRequest;

    public bool IsMouseInShopArea { get; set; }

    public void TriggerCharacterToDeleteRequest()
    {
        OnDeleteCharacterRequest?.Invoke(true);
    }

    public void TriggerCharacterToDeleteCancel()
    {
        OnDeleteCharacterRequest?.Invoke(false);
    }
}
