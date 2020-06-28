using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MovingForward))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(KillForPoints))]
public class UFO : MonoBehaviour
{
    private GameController gameController;

    public float moveSpeed;
    public float minAttackDelay;
    public float maxAttackDelay;
    public float bulletLifeTime;

    public float projectileSpread;

    public float spawnBulletDistance;

    private void Awake() {
        GetComponent<MovingForward>().SetMoveSpeed(moveSpeed);
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        GetComponent<KillForPoints>().Activate(gameController);
        GetComponent<Damageable>().AddAction(gameController.DieUFO);
        StartCoroutine(Shooting());
    }

    public void SetGameController(GameController _gameController)
    {
        gameController = _gameController;
    }

    private void Shoot() {
        GameObject newBullet = gameController.objectsPool.GetPooledObject(ObjectsPool.PoolType.redBullet);
        newBullet.transform.position = transform.position;
        newBullet.transform.LookAt(gameController.player.transform);
        newBullet.transform.Rotate(new Vector3(0, Random.Range(-projectileSpread, projectileSpread), 0));
        newBullet.transform.position += newBullet.transform.forward * spawnBulletDistance;
        newBullet.GetComponent<Bullet>().Activate(bulletLifeTime);

        gameController.audio2dController.PlayAudio(Audio2dController.AudioClipType.shootSound);
    }

    IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minAttackDelay, maxAttackDelay));
            Shoot();
        }
    }
}
