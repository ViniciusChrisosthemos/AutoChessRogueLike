using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterMovementController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private MeshRenderer _meshRenderer;

    [Header("UI")]
    [SerializeField] private GameObject _1starView;
    [SerializeField] private GameObject _2starView;
    [SerializeField] private GameObject _3starView;

    private bool _isDragging = false;
    private Vector3 _oldPosition;
    private CharacterRuntime _characterRuntime;
    private BoardController _boardController;

    private bool _hasSendDeleteRequest = false;

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

            if (GameStateController.Instance.IsMouseInShopArea)
            {
                if (!_hasSendDeleteRequest)
                {
                    GameStateController.Instance.TriggerCharacterToDeleteRequest();

                    _hasSendDeleteRequest = true;
                }
            }
            else
            {
                if (_hasSendDeleteRequest)
                {
                    GameStateController.Instance.TriggerCharacterToDeleteCancel();

                    _hasSendDeleteRequest = false;
                }
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

        if (GameStateController.Instance.IsMouseInShopArea)
        {
            _boardController.DeleteCharacter(this);

            GameStateController.Instance.TriggerCharacterToDeleteCancel();
        }
        else
        {
            _boardController.MoveCharacter(this, transform.position);
        }
    }

    public void SetCharacter(BoardController boardController, CharacterSO characterSO, bool inBench, Vector3 initialPosition)
    {
        _boardController = boardController;
        _oldPosition = initialPosition;

        _characterRuntime = new CharacterRuntime(characterSO, inBench);

        _meshRenderer.materials[0].color = _characterRuntime.CharacterData.Color;

        _characterRuntime.OnLevelUp.AddListener(UpdateStars);

        UpdateStars();
    }

    public void UpdateStars()
    {
        _1starView.SetActive(_characterRuntime.Stars == 1);
        _2starView.SetActive(_characterRuntime.Stars == 2);
        _3starView.SetActive(_characterRuntime.Stars == 3);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        _oldPosition = position;
    }

    public CharacterRuntime CharacterRuntime => _characterRuntime;
    public Vector3 OldPosition => _oldPosition;
}
