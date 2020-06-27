using System;
using System.Collections.Generic;
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

    public Dictionary<ControllerType, Control> listsOfPooledPrefabs = new Dictionary<ControllerType, Control>();
    public Control[] controls;
    private Control currentControl;

    private void Awake()
    {
        for (int i = 0; i < controls.Length; i++)
        {
            listsOfPooledPrefabs.Add(controls[i].controllerType, controls[i]);
        }
        SetController(defaultController);
    }

    public ControllerType GetCurrentControllerType() {
        return currentControllerType;
    }

    public void SetController(ControllerType _controllerType)
    {
        currentControllerType = _controllerType;
        currentControl = listsOfPooledPrefabs[currentControllerType];
        currentControl.controller.player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Time.timeScale != 0) {
            currentControl.controller.ControlsUpdate();
        }
    }

    [Serializable]
    public class Control {
        public ControllerType controllerType;
        public Controller controller;
    }
}
