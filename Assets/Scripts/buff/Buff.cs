using UnityEngine;

public abstract class Buff : ScriptableObject
{
    public string buffName;
    public string description;
    public Sprite buffIcon;

    public abstract void Apply(PlayerController player);
}

