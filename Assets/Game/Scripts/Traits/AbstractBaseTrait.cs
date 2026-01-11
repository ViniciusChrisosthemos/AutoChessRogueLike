using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBaseTrait : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private List<AbstractBaseTraitLevel> _levels;
    
    public string TraitName => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
    public List<AbstractBaseTraitLevel> Levels => _levels;
}
