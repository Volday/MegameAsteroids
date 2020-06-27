﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    List<Action> actions = new List<Action>();

    public void AddAction(Action _action) {
        actions.Add(_action);
    }

    public void RemoveAction(Action _action)
    {
        actions.Remove(_action);
    }

    public void ClearActions()
    {
        actions.Clear();
    }

    public void TakeHit() {
        if (GetComponent<Invulnerability>() == null) {
            int actionsCount = actions.Count;
            for (int i = 0; i < actionsCount; i++)
            {
                actions[0]();
                actions.RemoveAt(0);
            }
        }
    }
}
