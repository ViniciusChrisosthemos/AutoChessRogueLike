using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBaseTrait : ScriptableObject
{
    [SerializeField] private string _description;

    [SerializeField] private List<AbstractBaseTraitLevel> _levels;
}
