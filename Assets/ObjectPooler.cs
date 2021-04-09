using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public List<Pool> pools;
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab);
                obj.GetComponent<IPooledObject>()?.InitialSetUp();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnFromPool(string poolTag, Vector2 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogError("Pool Tag does not exist!"); return null;
        }

        var objectToSpawn = poolDictionary[poolTag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        objectToSpawn.GetComponent<IPooledObject>()?.OnObjectSpawn();
        
        poolDictionary[poolTag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
