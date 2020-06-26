using System;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    Queue<Action> actions = new Queue<Action>();

    public void AddAction(Action _action) {
        actions.Enqueue(_action);
    }

    public void TakeHit() {
        int actionsCount = actions.Count;
        for (int i = 0; i < actionsCount; i++)
        {
            actions.Dequeue()();
        }
    }
}
