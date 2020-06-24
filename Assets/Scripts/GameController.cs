using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectsPool))]
public class GameController : MonoBehaviour
{
    public Player player;
    public UFO ufo;
    [HideInInspector] public ObjectsPool objectsPool;

    public int startAsteroidsCount;
    public int increaseAsteroidsCount;
    public int numberOfAsteroidChild;

    private void Awake()
    {
        player.SetGameController(this);

        objectsPool = GetComponent<ObjectsPool>();
        int maxPlayerBulletCount = (int)(player.bulletLifeTime * (1 / player.attackSpeed)) + 1;
        int maxUFOBulletCount = (int)(ufo.bulletLifeTime * (1 / ufo.attackSpeed)) + 1;
        objectsPool.Initialization(startAsteroidsCount, increaseAsteroidsCount, numberOfAsteroidChild,
            maxPlayerBulletCount, maxUFOBulletCount);
    }
}
