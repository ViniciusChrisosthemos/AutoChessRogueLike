using UnityEngine;

public abstract class AbstractBaseTraitLevel : ScriptableObject
{
    [SerializeField] private string _description;
    [SerializeField] private int _characterAmountRequirement;
}
