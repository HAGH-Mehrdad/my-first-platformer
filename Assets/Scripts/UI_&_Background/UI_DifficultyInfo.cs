using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DifficultyInfo : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI txtDifficultyInfo;


    [TextArea]
    [SerializeField] private string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        txtDifficultyInfo.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        txtDifficultyInfo.text = string.Empty;
    }
}
