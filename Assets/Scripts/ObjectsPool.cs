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

    public Pool[] pools;

    private void Awake()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            listsOfPooledPrefabs.Add(pools[i].poolType, i);
        }
    }

    public void PoolsExtension()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].Extension();
        }
    }

    public GameObject GetPooledObject(PoolType _poolType)
    {
        return pools[listsOfPooledPrefabs[_poolType]].GetObject();
    }

    public void ResetPools()
    {
        for (int i = 0; i < pools.Length; i++)
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
        private int extensionOverWave = 0;
        private int countOfUsingObjects = 0;

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
