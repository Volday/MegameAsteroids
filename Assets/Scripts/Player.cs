using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Player : MonoBehaviour
{
    private GameController gameController;

    public float invulnerabilityTime;
    public GameObject scin;

    public float maxSpeed;
    public float rotationSpeed;
    public float acceleration;
    public float attackSpeed;
    public float bulletLifeTime;

    private bool shootingAvailable = true;

    public Transform muzzle;

    public Vector3 startPosition;
    public Vector3 startDirection;

    private Vector3 movingVector;

    private void Start()
    {
        StartCoroutine(Shooting());
    }

    private void Update()
    {
        transform.position += movingVector * Time.deltaTime;
    }

    public void ResetPlayer()
    {
        movingVector = Vector3.zero;
        transform.position = startPosition;
        transform.LookAt(startPosition + startDirection);
        gameObject.AddComponent<Invulnerability>().Activate(invulnerabilityTime, scin);
    }

    public void SetGameController(GameController _gameController) {
        gameController = _gameController;
    }

    public GameController GetGameController()
    {
        return gameController;
    }

    public void MoveForward() {
        movingVector += transform.forward * acceleration;
        movingVector = Vector3.ClampMagnitude(movingVector, maxSpeed);
    }

    public void Rotate(Vector3 _targetToRotation) {
        Vector3 direction = _targetToRotation - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    public void Rotate(float _angleOfRotation){
        transform.Rotate(new Vector3(0, _angleOfRotation * rotationSpeed * Time.deltaTime, 0));
    }

    public void Shoot() {
        if (shootingAvailable) {
            GameObject newBullet = gameController.objectsPool.GetPooledObject(ObjectsPool.PoolType.greenBullet);
            newBullet.transform.position = muzzle.position;
            newBullet.transform.rotation = transform.rotation;
            newBullet.GetComponent<Bullet>().Activate(bulletLifeTime);
            
            shootingAvailable = false;
        }
    }

    IEnumerator Shooting() {
        while(true){
            if (!shootingAvailable) {
                yield return new WaitForSeconds(attackSpeed);
                shootingAvailable = true;
            }
            yield return null;
        }
    }
}
