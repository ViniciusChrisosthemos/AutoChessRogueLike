using System;
using UnityEngine;

[Serializable]
public class ProbabilityHolder<T>
{
    public T Item;
    [Range(0, 1)] public float Probability;
}
