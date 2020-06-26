using UnityEngine;

public class DividableAsteroid : Asteroid
{
    public GameObject child;
    public ObjectsPool.PoolType childType;

    public override void Die() {
        float angleBetweenAsteroids = 0;
        if (gameController.numberOfAsteroidChild > 1)
        {
            angleBetweenAsteroids = gameController.angleOfAsteroidsSpread * 2 / (gameController.numberOfAsteroidChild - 1);
        }

        float childSpeed = Random.Range(gameController.minAsteroidSpeed, gameController.maxAsteroidSpeed);
        for (int i = 0; i < gameController.numberOfAsteroidChild; i++) {
            GameObject newAsteroid = gameController.objectsPool.GetPooledObject(childType);

            newAsteroid.transform.rotation = transform.rotation;
            if (gameController.numberOfAsteroidChild > 1) {
                newAsteroid.transform.Rotate(new Vector3(0, gameController.angleOfAsteroidsSpread - i * angleBetweenAsteroids, 0));
            }
            newAsteroid.GetComponent<Asteroid>().Activate(transform.position, gameController, 
                Random.Range(gameController.minAsteroidSpeed, gameController.maxAsteroidSpeed));
        }
        gameObject.SetActive(false);
    }
}
