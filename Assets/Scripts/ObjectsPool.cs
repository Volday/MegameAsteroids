using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectsPool : MonoBehaviour
{
    public enum PoolType
    {
        greenBullet,
        redBullet,
        bigAsteroid,
        mediumAsteroid,
        smallAsteroid
    }

    public Dictionary<PoolType, int> listsOfPooledPrefabs = new Dictionary<PoolType, int>();

    public List<Pool> pools = new List<Pool>();

    public void Initialization(int _startAsteroidsCount, int _increaseAsteroidsCount, int _numberOfAsteroidChild, 
        int _maxPlayerBulletCount, int _maxUFOBulletCount)
    {
        for (int i = 0; i < pools.Count; i++) {
            listsOfPooledPrefabs.Add(pools[i].poolType, i);
        }

        pools[listsOfPooledPrefabs[PoolType.bigAsteroid]].SetExtensionOverWave(_increaseAsteroidsCount);
        pools[listsOfPooledPrefabs[PoolType.mediumAsteroid]].SetExtensionOverWave(_increaseAsteroidsCount * _numberOfAsteroidChild);
        pools[listsOfPooledPrefabs[PoolType.smallAsteroid]].SetExtensionOverWave(_increaseAsteroidsCount * _numberOfAsteroidChild * _numberOfAsteroidChild);

        if (listsOfPooledPrefabs.ContainsKey(PoolType.bigAsteroid))
        {
            pools[listsOfPooledPrefabs[PoolType.bigAsteroid]].Extension(_startAsteroidsCount);
        }

        if (listsOfPooledPrefabs.ContainsKey(PoolType.greenBullet))
        {
            pools[listsOfPooledPrefabs[PoolType.greenBullet]].Extension(_maxPlayerBulletCount);
        }

        if (listsOfPooledPrefabs.ContainsKey(PoolType.redBullet))
        {
            pools[listsOfPooledPrefabs[PoolType.redBullet]].Extension(_maxUFOBulletCount);
        }
    }

    public void NewWave()
    {

    }

    public GameObject GetPooledObject(PoolType _poolType)
    {
        return pools[listsOfPooledPrefabs[_poolType]].GetObject();
    }

    public void ResetPools()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].ResetPool();
        }
    }

    [Serializable]
    public class Pool {
        public PoolType poolType;
        public GameObject prefab;
        private List<GameObject> pooledPrefabs = new List<GameObject>();
        private Queue<GameObject> pooledPrefabsQueue = new Queue<GameObject>();
        private int extensionOverWave;
        private int countOfUsingObjects;

        public void Extension(int _extensionCount) {
            for (int i = 0; i < _extensionCount; i++) {
                if (countOfUsingObjects + _extensionCount > pooledPrefabs.Count)
                {
                    GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    newPrefab.SetActive(false);
                    pooledPrefabs.Add(newPrefab);
                    pooledPrefabsQueue.Enqueue(newPrefab);
                }
                countOfUsingObjects += _extensionCount;
            }
        }

        public void Extension() {
            if (extensionOverWave > 0) {
                Extension(extensionOverWave);
            }
        }

        public GameObject GetObject() {
            GameObject retrievedObject = pooledPrefabsQueue.Dequeue();
            pooledPrefabsQueue.Enqueue(retrievedObject);
            return retrievedObject;
        }

        public void SetExtensionOverWave(int _extensionOverWave) {
            extensionOverWave = _extensionOverWave;
        }

        public void ResetPool() {
            countOfUsingObjects = 0;
            for (int i = 0; i < pooledPrefabs.Count; i++)
            {
                pooledPrefabs[i].SetActive(false);
            }
        }
    }
}
