using System.Collections.Generic;
using UnityEngine;

public class UITraitsView : MonoBehaviour
{
    [SerializeField] private UIListDisplay _traitListDisplay;

    private void Start()
    {
        _traitListDisplay.SetItems(new List<object>(), null);

        GameStateController.Instance.OnTraitsUpdated.AddListener(UpdateTraits);
    }

    public void UpdateTraits(TraitsController traitController)
    {
        _traitListDisplay.SetItems(traitController.GetTraits(), null);
    }
}
