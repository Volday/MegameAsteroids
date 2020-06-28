using System;
using UnityEngine;

[RequireComponent(typeof(MovingForward))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(KillForPoints))]
public abstract class Asteroid : MonoBehaviour
{
    [HideInInspector] public GameController gameController;
    public Audio2dController.AudioClipType audioClipType;
    private Action die;

    public void Activate(Vector3 _position, GameController _gameController, float _moveSpeed)
    {
        transform.position = _position;
        gameController = _gameController;
        gameController.AddActiveAsteroid(gameObject);
        GetComponent<MovingForward>().SetMoveSpeed(_moveSpeed);
        GetComponent<KillForPoints>().Activate(gameController);
        die = Die;
        GetComponent<Damageable>().AddAction(die);
        GetComponent<Damageable>().AddAction(RemoveFromActiveActeroidList);
        GetComponent<Damageable>().AddAction(PlayExplosionSound);
        gameObject.SetActive(true);
    }

    //
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Damageable>() != null)//если астероид столкнулся с целью которой можно нанести урон
        {
            if (other.gameObject.GetComponent<Asteroid>() == null) {
                other.gameObject.GetComponent<Damageable>().TakeHit();
                if (other.gameObject.GetComponent<Player>() != null || 
                    other.gameObject.GetComponent<UFO>() != null) { // и это игрок или нло, то он удаляет эффект распадания на маленькие астероиды
                    gameObject.GetComponent<Damageable>().RemoveAction(die);
                    GetComponent<Damageable>().TakeHit();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void PlayExplosionSound()
    {
        gameController.audio2dController.PlayAudio(audioClipType);
    }

    public void RemoveFromActiveActeroidList() {
        gameController.RemoveActiveAsteroid(gameObject);
    }

    public abstract void Die();
}
