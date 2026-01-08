using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIKeywordView : UIItemController
{
    [SerializeField] private Image _imgKeywordArt;
    [SerializeField] private TextMeshProUGUI _txtKeyword;

    protected override void HandleInit(object obj)
    {
        KeywordSO keywordData = obj as KeywordSO;

        _txtKeyword.text = keywordData.Name;
        _imgKeywordArt.sprite = keywordData.Art;
    }
}
