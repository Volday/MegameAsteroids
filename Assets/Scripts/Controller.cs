using UnityEngine;

public abstract class Controller: ScriptableObject
{
    public Player player;
    public abstract void ControlsUpdate();
}
