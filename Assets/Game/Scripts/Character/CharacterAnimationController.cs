using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private string _attackTriggerName = "Attack";
    [SerializeField] private string _runningFlagName = "Running";

    private float _attackSpeed = 1f;

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Attack();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartRunning();
        }

        if (Keyboard.current.rKey.wasReleasedThisFrame)
        {
            StopRunning();
        }
    }

    public void Attack()
    {
        _animator.SetTrigger(_attackTriggerName);
        Debug.Log("Attack animation triggered.");
    }

    public void StartRunning()
    {
        _animator.SetBool(_runningFlagName, true);
    }

    public void StopRunning()
    {
        _animator.SetBool(_runningFlagName, false);
    }
}
