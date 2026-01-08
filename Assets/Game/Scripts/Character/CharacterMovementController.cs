using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovementController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BoardController BoardController;

    private bool _isDragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }
}
