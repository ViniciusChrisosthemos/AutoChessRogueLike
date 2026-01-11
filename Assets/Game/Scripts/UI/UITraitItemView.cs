using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITraitItemView : UIItemController
{
    [SerializeField] private Image _imgTraitArt;
    [SerializeField] private TextMeshProUGUI _txtCurrentNumber;
    [SerializeField] private TextMeshProUGUI _txtTraitName;
    [SerializeField] private TextMeshProUGUI _txtTraitSequence;

    [Header("Parameters")]
    [SerializeField] private Color _colorItemDisabled;

    protected override void HandleInit(object obj)
    {
        var traitData = obj as TraitRuntime;

        _txtTraitName.text = traitData.TraitData.TraitName;
        _imgTraitArt.sprite = traitData.TraitData.Icon;
        _txtCurrentNumber.text = traitData.CurrentCharacters.ToString();

        var sequenceItems = new List<string>();

        for (int i = 0; i < traitData.TraitData.Levels.Count; i++)
        {
            sequenceItems.Add(traitData.TraitData.Levels[i].CharacterAmountRequirement.ToString());

            if (i != traitData.TraitData.Levels.Count - 1)
            {
                sequenceItems.Add(" > ");
            }
        }

        _txtTraitSequence.text = string.Join("", sequenceItems);
    }
}
