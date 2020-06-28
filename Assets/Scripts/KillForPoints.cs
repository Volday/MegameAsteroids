using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class KillForPoints : MonoBehaviour
{
    public int points;

    private GameController gameController;

    public void Activate(GameController _gameController)
    {
        GetComponent<Damageable>().AddAction(GivePoints);
        gameController = _gameController;
    }

    public void GivePoints() {
        gameController.menu.IncreaseScore(points);
    }
}
