using UnityEngine;

public abstract class Controller : ScriptableObject
{
    [HideInInspector] public Player player;
    [HideInInspector] public bool shootButtonDown = false;
    public abstract void ControlsUpdate();
}
