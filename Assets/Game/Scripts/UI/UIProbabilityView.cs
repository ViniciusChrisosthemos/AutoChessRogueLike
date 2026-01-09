using TMPro;
using UnityEngine;

public class UIProbabilityView : UIItemController
{
    [SerializeField] private TextMeshProUGUI _txtProbability;

    protected override void HandleInit(object obj)
    {
        var probData = obj as ProbabilityHolder<CostSO>;

        _txtProbability.text = $"{probData.Item.Cost} - {(probData.Probability*100):F0}%";
    }
}
