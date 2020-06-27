using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class KillForPoints : MonoBehaviour
{
    public int points;

    private GameController gameController;

    private void Awake()
    {
        GetComponent<Damageable>().AddAction(GivePoints);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void GivePoints() {
        gameController.menu.IncreaseScore(points);
    }
}
