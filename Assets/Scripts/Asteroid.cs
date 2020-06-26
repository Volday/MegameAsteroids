using UnityEngine;

[RequireComponent(typeof(MovingForward))]
[RequireComponent(typeof(Damageable))]
public abstract class Asteroid : MonoBehaviour
{
    [HideInInspector] public GameController gameController;

    public void Activate(Vector3 _position, GameController _gameController, float _moveSpeed)
    {
        transform.position = _position;
        gameController = _gameController;
        gameController.AddActiveAsteroid(gameObject);
        GetComponent<MovingForward>().SetMoveSpeed(_moveSpeed);
        GetComponent<Damageable>().AddAction(RemoveFromActiveActeroidList);
        GetComponent<Damageable>().AddAction(Die);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Damageable>() != null)
        {
            if (other.gameObject.GetComponent<Asteroid>() == null) {
                other.gameObject.GetComponent<Damageable>().TakeHit();
            }
        }
    }

    public void RemoveFromActiveActeroidList() {
        gameController.RemoveActiveAsteroid(gameObject);
    }

    public abstract void Die();
}
