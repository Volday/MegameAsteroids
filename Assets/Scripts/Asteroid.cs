using System;
using UnityEngine;

[RequireComponent(typeof(MovingForward))]
[RequireComponent(typeof(Damageable))]
public abstract class Asteroid : MonoBehaviour
{
    [HideInInspector] public GameController gameController;
    private Action die;

    public void Activate(Vector3 _position, GameController _gameController, float _moveSpeed)
    {
        transform.position = _position;
        gameController = _gameController;
        gameController.AddActiveAsteroid(gameObject);
        GetComponent<MovingForward>().SetMoveSpeed(_moveSpeed);
        die = Die;
        GetComponent<Damageable>().AddAction(die);
        GetComponent<Damageable>().AddAction(RemoveFromActiveActeroidList);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Damageable>() != null)
        {
            if (other.gameObject.GetComponent<Asteroid>() == null) {
                other.gameObject.GetComponent<Damageable>().TakeHit();
                if (other.gameObject.GetComponent<Player>() != null ||
                    other.gameObject.GetComponent<UFO>() != null) {
                    gameObject.GetComponent<Damageable>().RemoveAction(die);
                    GetComponent<Damageable>().TakeHit();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void RemoveFromActiveActeroidList() {
        gameController.RemoveActiveAsteroid(gameObject);
    }

    public abstract void Die();
}
