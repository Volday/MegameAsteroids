using UnityEngine;

public class Player : MonoBehaviour
{
    private GameController gameController;

    public float maxSpeed;
    public float rotationSpeed;
    public float acceleration;
    public float attackSpeed;
    public float bulletLifeTime;
    public float bulletMoveSpeed;

    private Vector3 movingVector;

    private void Update()
    {
        transform.position += movingVector * Time.deltaTime;
    }

    public void SetGameController(GameController _gameController) {
        gameController = _gameController;
    }

    public void MoveForward() {
        movingVector += transform.forward * acceleration;
        movingVector = Vector3.ClampMagnitude(movingVector, maxSpeed);
    }

    public void Rotate(Vector3 _targetToRotation) { 
        
    }

    public void Rotate(float _angleOfRotation){
        transform.Rotate(new Vector3(0, _angleOfRotation * rotationSpeed * Time.deltaTime, 0));
    }

    public void Shoot() {
        GameObject newBullet = gameController.objectsPool.GetPooledObject(ObjectsPool.PoolType.greenBullet);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.rotation;
        newBullet.GetComponent<MovingForward>().SetMoveSpeed(bulletMoveSpeed);
        newBullet.SetActive(true);
    }
}
