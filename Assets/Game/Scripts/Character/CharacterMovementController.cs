using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterMovementController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BoardController BoardController;
    public CharacterSO CharacterSO;
    public MeshRenderer MeshRenderer;

    private bool _isDragging = false;
    private Vector3 _oldPosition;
    private CharacterRuntime _characterRuntime;

    private void Update()
    {
        if (_isDragging)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 10f, LayerMask.NameToLayer("Floor")))
            {
                var position = transform.position;

                position.x = hitInfo.point.x;
                position.z = hitInfo.point.z;

                transform.position = position;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;

        _oldPosition = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;

        BoardController.MoveCharacter(this, _oldPosition, transform.position);
    }

    public void SetCharacter(BoardController boardController, CharacterSO characterSO, bool inBench)
    {
        BoardController = boardController;

        _characterRuntime = new CharacterRuntime(CharacterSO, inBench);

        //MeshRenderer.material.color = CharacterSO.Color;
        Debug.Log($"=> {MeshRenderer}");
    }

    public CharacterRuntime CharacterRuntime => _characterRuntime;
}
