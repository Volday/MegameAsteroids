using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectsPool))]
public class GameController : MonoBehaviour
{
    public int seed;
    public bool useRandomSeed;

    public Player player;
    public UFO ufo;
    public Menu menu;
    [HideInInspector] public ObjectsPool objectsPool;
    private Camera mainCamera;

    public int startAsteroidsCount;
    public int increaseAsteroidsCount;
    public int numberOfAsteroidChild;

    public float minAsteroidSpeed;
    public float maxAsteroidSpeed;
    public float angleOfAsteroidsSpread;

    private bool gameStarted = false;
    private int currentAsteroidsCountAtWave;
    private List<GameObject> activeAsteroids = new List<GameObject>();

    private void Awake()
    {
        if (!useRandomSeed) {
            Random.InitState(seed);
        }

        player.SetGameController(this);
        menu.SetGameController(this);

        objectsPool = GetComponent<ObjectsPool>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        player.gameObject.SetActive(false);
    }

    public bool GetGameStatus() {
        return gameStarted;
    }

    public void NewGame() {
        player.gameObject.SetActive(true);
        player.ResetPlayer();
        currentAsteroidsCountAtWave = startAsteroidsCount;
        objectsPool.ResetPools();
        InitializationPools();
        NewWave();
        gameStarted = true;
    }

    private void NewWave() {
        for (int i = 0; i < currentAsteroidsCountAtWave; i++) {
            GameObject newAsteroid = objectsPool.GetPooledObject(ObjectsPool.PoolType.bigAsteroid);

            float screenRatio = (float)Screen.width / Screen.height;
            newAsteroid.transform.LookAt(new Vector3(Random.Range(-mainCamera.orthographicSize, mainCamera.orthographicSize) 
                * screenRatio, 0, Random.Range(-mainCamera.orthographicSize, mainCamera.orthographicSize)));

            newAsteroid.GetComponent<Asteroid>().Activate(GetAsteroidSpawnPosition(), this, Random.Range(minAsteroidSpeed, maxAsteroidSpeed));
        }

        //расширение пула астероидов
        currentAsteroidsCountAtWave += increaseAsteroidsCount;
        objectsPool.PoolsExtension();
    }

    public void AddActiveAsteroid(GameObject _gameObject) {
        activeAsteroids.Add(_gameObject);
    }

    public void RemoveActiveAsteroid(GameObject _gameObject)
    {
        Debug.Log("asd");
        activeAsteroids.Remove(_gameObject);
        if (activeAsteroids.Count == 0) {
            NewWave();
        }
    }

    private void Update()
    {
        //Debug.Log(activeAsteroids.Count);
    }

    private Vector3 GetAsteroidSpawnPosition() {
        bool orientation = Random.Range(0f, Screen.width + Screen.height) < Screen.width;
        float screenRatio = (float)Screen.width / Screen.height;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = mainCamera.orthographicSize * screenRatio;
        if (orientation)
        {
            //по горизонтали
            if (Random.Range(0, 2) == 0)
            {
                //сверху
                return new Vector3(Random.Range(-halfWidth, halfWidth), 0, halfHeight);
            }
            else
            {
                //снизу
                return new Vector3(Random.Range(-halfWidth, halfWidth), 0, -halfHeight);
            }
        }
        else {
            //по вертикали
            if (Random.Range(0, 2) == 0)
            {
                //спарва
                return new Vector3(halfWidth, 0, Random.Range(-halfHeight, halfHeight));
            }
            else
            {
                //слева
                return new Vector3(-halfWidth, 0, Random.Range(-halfHeight, halfHeight));
            }
        }
    }

    private void InitializationPools()
    {
        objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.bigAsteroid]]
            .SetExtensionOverWave(increaseAsteroidsCount);
        objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.mediumAsteroid]]
            .SetExtensionOverWave(increaseAsteroidsCount * numberOfAsteroidChild);
        objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.smallAsteroid]]
            .SetExtensionOverWave(increaseAsteroidsCount * numberOfAsteroidChild * numberOfAsteroidChild);

        if (objectsPool.listsOfPooledPrefabs.ContainsKey(ObjectsPool.PoolType.bigAsteroid))
        {
            objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.bigAsteroid]]
                .Extension(startAsteroidsCount);
        }
        if (objectsPool.listsOfPooledPrefabs.ContainsKey(ObjectsPool.PoolType.mediumAsteroid))
        {
            objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.mediumAsteroid]]
                .Extension(startAsteroidsCount * numberOfAsteroidChild);
        }
        if (objectsPool.listsOfPooledPrefabs.ContainsKey(ObjectsPool.PoolType.smallAsteroid))
        {
            objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.smallAsteroid]]
                .Extension(startAsteroidsCount * numberOfAsteroidChild * numberOfAsteroidChild);
        }
        int maxPlayerBulletCount = (int)(player.bulletLifeTime * (1 / player.attackSpeed)) + 1;
        if (objectsPool.listsOfPooledPrefabs.ContainsKey(ObjectsPool.PoolType.greenBullet))
        {
            objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.greenBullet]]
                .Extension(maxPlayerBulletCount);
        }
        int maxUFOBulletCount = (int)(ufo.bulletLifeTime * (1 / ufo.attackSpeed)) + 1;
        if (objectsPool.listsOfPooledPrefabs.ContainsKey(ObjectsPool.PoolType.redBullet))
        {
            objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.redBullet]]
                .Extension(maxUFOBulletCount);
        }
    }
}
