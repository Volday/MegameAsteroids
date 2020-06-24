using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public enum ControllerType {
        keyboard,
        mouseAndKeyboard
    }

    private ControllerType currentControllerType;
    public ControllerType defaultController;
    private Controller currentController;
    public Controller[] controllers;

    private void Awake()
    {
        currentController = controllers[(int)defaultController];
        currentController.player = GetComponent<Player>();
    }

    void SetCurrentController(ControllerType _controllerType)
    {
        if (currentControllerType != _controllerType) {
            currentControllerType = _controllerType;
            currentController = controllers[(int)currentControllerType];
        }
    }

    private void Update()
    {
        currentController.ControlsUpdate();
    }
}
