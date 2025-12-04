using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler Reference { get; private set; }

    void Awake()
    {
        Reference = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDict;

    void Start()
    {
        PoolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {

                GameObject obj = Instantiate(pool.prefab, new Vector3(0, 16, 0), Quaternion.identity,this.gameObject.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);

            }

            PoolDict.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot, GameObject parent)
    {
        if (!PoolDict.ContainsKey(tag))
        {
            Debug.Log("Pool of tag " + tag + "doesn't exist, check your inputs bozo");
            return null;
        }

        GameObject spawnedObj = PoolDict[tag].Dequeue();

        spawnedObj.transform.parent = parent.transform;
        spawnedObj.transform.position = pos;
        spawnedObj.transform.rotation = rot;
        spawnedObj.SetActive(true);

        PoolDict[tag].Enqueue(spawnedObj);

        return spawnedObj;
    }

    public void resetPooledObject(string tag, GameObject pooledObject)
    {
        if (!PoolDict.ContainsKey(tag))
        {
            Debug.Log("Pool of tag " + tag + "doesn't exist, check your inputs bozo");
            return;
        }
        if (!PoolDict[tag].Contains(pooledObject)) 
        {
            Debug.Log($"Object {pooledObject} does not exist in the {tag} pool");
            return;
        }

        pooledObject.transform.parent = this.gameObject.transform;
        pooledObject.transform.position = new Vector3(0, 16, 0);
        pooledObject.transform.rotation = Quaternion.identity;
        pooledObject.SetActive(false);


    }

}