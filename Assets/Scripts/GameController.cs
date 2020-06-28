using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectsPool))]
[RequireComponent(typeof(Audio2dController))]
public class GameController : MonoBehaviour
{
    public int seed;
    public bool useRandomSeed;

    public int maxPlayerHealthPoints;
    private int currentPlayerHealthPoints;

    public Player player;
    public UFO ufo;
    public Menu menu;
    [HideInInspector] public ObjectsPool objectsPool;
    [HideInInspector] public Audio2dController audio2dController;
    public Camera mainCamera;

    public int startAsteroidsCount;
    public int increaseAsteroidsCount;
    public int numberOfAsteroidChild;
    public float waveSpawnTime;
    private IEnumerator newWave;

    public float minAsteroidSpeed;
    public float maxAsteroidSpeed;
    public float angleOfAsteroidsSpread;

    private bool gameStarted = false;
    private int currentAsteroidsCountAtWave;
    private List<GameObject> activeAsteroids = new List<GameObject>();

    public float pctUFOSpawnHeightIndent;
    public float minUFORespawnTime;
    public float maxUFORespawnTime;
    private IEnumerator respawnUFO;

    private void Awake()
    {
        if (!useRandomSeed) {
            Random.InitState(seed);
        }

        audio2dController = GetComponent<Audio2dController>();

        player.SetGameController(this);
        ufo.SetGameController(this);
        menu.SetGameController(this);

        objectsPool = GetComponent<ObjectsPool>();
        if (mainCamera == null) {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        player.gameObject.SetActive(false);
    }

    public bool GetGameStatus() {
        return gameStarted;
    }

    public void NewGame() {
        //Сбрасываются все накопленные предсмертные эффекты
        Damageable[] damageables = FindObjectsOfType<Damageable>();
        for (int i = 0; i < damageables.Length; i++)
        {
            damageables[i].ClearActions();
        }

        player.gameObject.SetActive(true);
        player.ResetPlayer();
        player.GetComponent<Damageable>().AddAction(DiePlayer);

        currentPlayerHealthPoints = maxPlayerHealthPoints;
        menu.UpdateHealthIcons(currentPlayerHealthPoints);
        menu.ResetScore();

        currentAsteroidsCountAtWave = startAsteroidsCount;
        activeAsteroids.Clear();
        objectsPool.ResetPools();
        InitializationPools();

        if (newWave != null) {
            StopCoroutine(newWave);
        }
        newWave = NewWave();
        StartCoroutine(newWave);
        DieUFO();

        gameStarted = true;
    }

    IEnumerator NewWave() {
        //После задержки waveSpawnTime берёт из пула и расставляет большие астероиды
        yield return new WaitForSeconds(waveSpawnTime);
        for (int i = 0; i < currentAsteroidsCountAtWave; i++) {
            GameObject newAsteroid = objectsPool.GetPooledObject(ObjectsPool.PoolType.bigAsteroid);

            float screenRatio = (float)Screen.width / Screen.height;
            Vector3 direction = new Vector3(Random.Range(-mainCamera.orthographicSize, mainCamera.orthographicSize)
                * screenRatio, 0, Random.Range(-mainCamera.orthographicSize, mainCamera.orthographicSize));

            newAsteroid.transform.LookAt(direction);

            newAsteroid.GetComponent<Asteroid>().Activate(GetAsteroidSpawnPosition(), this, Random.Range(minAsteroidSpeed, maxAsteroidSpeed));
        }

        //Расширение пула астероидов
        currentAsteroidsCountAtWave += increaseAsteroidsCount;
        objectsPool.PoolsExtension();
    }

    public void AddActiveAsteroid(GameObject _gameObject) {
        activeAsteroids.Add(_gameObject);
    }

    public void RemoveActiveAsteroid(GameObject _gameObject)
    {
        activeAsteroids.Remove(_gameObject);
        if (activeAsteroids.Count == 0) {//Если не остаётся активных астероидов запускается новая волна
            if (newWave != null)
            {
                StopCoroutine(newWave);
            }
            newWave = NewWave();
            StartCoroutine(newWave);
        }
    }

    private Vector3 GetAsteroidSpawnPosition() {
        bool orientation = Random.Range(0f, Screen.width + Screen.height) < Screen.width;
        float screenRatio = (float)Screen.width / Screen.height;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = mainCamera.orthographicSize * screenRatio;
        Vector3 spawnPosition;
        if (orientation)
        {
            //по горизонтали
            if (Random.Range(0, 2) == 0)
            {
                //сверху
                spawnPosition = new Vector3(Random.Range(-halfWidth, halfWidth), 0, halfHeight);
            }
            else
            {
                //снизу
                spawnPosition = new Vector3(Random.Range(-halfWidth, halfWidth), 0, -halfHeight);
            }
        }
        else {
            //по вертикали
            if (Random.Range(0, 2) == 0)
            {
                //спарва
                spawnPosition = new Vector3(halfWidth, 0, Random.Range(-halfHeight, halfHeight));
            }
            else
            {
                //слева
                spawnPosition = new Vector3(-halfWidth, 0, Random.Range(-halfHeight, halfHeight));
            }
        }
        return spawnPosition;
    }

    private void InitializationPools()
    {
        //Устанавливается размер расширения пула, для новой волны
        objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.bigAsteroid]]
            .SetExtensionOverWave(increaseAsteroidsCount);
        objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.mediumAsteroid]]
            .SetExtensionOverWave(increaseAsteroidsCount * numberOfAsteroidChild);
        objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.smallAsteroid]]
            .SetExtensionOverWave(increaseAsteroidsCount * numberOfAsteroidChild * numberOfAsteroidChild);

        //Устанавливается стартовые размеры пулов
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
        int maxUFOBulletCount = (int)(ufo.bulletLifeTime * (1 / ufo.maxAttackDelay)) + 1;
        if (objectsPool.listsOfPooledPrefabs.ContainsKey(ObjectsPool.PoolType.redBullet))
        {
            objectsPool.pools[objectsPool.listsOfPooledPrefabs[ObjectsPool.PoolType.redBullet]]
                .Extension(maxUFOBulletCount);
        }
    }

    public void DiePlayer() {
        currentPlayerHealthPoints--;
        menu.UpdateHealthIcons(currentPlayerHealthPoints);

        if (currentPlayerHealthPoints > 0)
        {
            player.ResetPlayer();
            player.GetComponent<Damageable>().AddAction(DiePlayer);
        }
        else {
            gameStarted = false;
            menu.PauseGame();
        }
    }

    public void DieUFO() {
        ufo.gameObject.SetActive(false);

        float screenRatio = (float)Screen.width / Screen.height;
        Vector3 position;
        if (Random.Range(0, 2) == 0)//Выбор новой позиции
        {
            //справа
            float heightFreeToSpawn = (1 - pctUFOSpawnHeightIndent / 100) * mainCamera.orthographicSize;
            position = new Vector3(mainCamera.orthographicSize * screenRatio, 0, 
                Random.Range(-heightFreeToSpawn, heightFreeToSpawn));
            ufo.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            //слево
            float heightFreeToSpawn = (1 - pctUFOSpawnHeightIndent / 100) * mainCamera.orthographicSize;
            position = new Vector3(-mainCamera.orthographicSize * screenRatio, 0,
                Random.Range(-heightFreeToSpawn, heightFreeToSpawn));
            ufo.transform.rotation = Quaternion.Euler(0, 90,0);
        }
        ufo.transform.position = position;

        float timeToRespawn = Random.Range(minUFORespawnTime, maxUFORespawnTime);
        if (respawnUFO != null) {
            StopCoroutine(respawnUFO);
        }
        respawnUFO = RespawnUFO(timeToRespawn);
        StartCoroutine(respawnUFO);
    }

    IEnumerator RespawnUFO(float _timeToRespawn)
    {
        yield return new WaitForSeconds(_timeToRespawn);
        ufo.Activate();
    }
}
