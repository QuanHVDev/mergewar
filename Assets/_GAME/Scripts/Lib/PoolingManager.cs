using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class PoolManager : SingletonBehaviourDontDestroy<PoolManager>{
    [Serializable]
    public class SpawnedObjectClass{
        public GameObject key;
        public GameObject value;
        public SpawnedObjectClass(GameObject key, GameObject value) {
            this.key = key;
            this.value = value;
        }
    }
    
    [Serializable]
    public class SpawnedObjectsClass{
        [SerializeField] private List<SpawnedObjectClass> spawnedObjects;
        public SpawnedObjectsClass() {
            spawnedObjects = new List<SpawnedObjectClass>();
        }

        public List<SpawnedObjectClass> SpawnedObjects => spawnedObjects;

        public void Add(SpawnedObjectClass obj) {
            spawnedObjects.Add(obj);
        }

        public bool TryGetValue(GameObject obj, out GameObject key) {
            foreach (var spawned in spawnedObjects) {
                if (spawned.key == obj) {
                    key = spawned.value;
                    return true;
                }
            }

            key = new GameObject();
            return false;
        }

        public void Remove(GameObject key) {
            foreach (var spawned in spawnedObjects) {
                if (spawned.key == key) {
                    spawnedObjects.Remove(spawned);
                    return;
                }
            }
        }

        public List<GameObject> Keys() {
            List<GameObject> keys = new List<GameObject>();
            foreach (var spawned in spawnedObjects) {
                keys.Add(spawned.key);
            }

            return keys;
        }

        public bool ContainsKey(GameObject key) {
            foreach (var spawned in spawnedObjects) {
                if (spawned.key == key) return true;
            }

            return false;
        }
    }

    [Serializable]
    public class PooledObject{
        public GameObject key;
        public List<GameObject> value;

        public PooledObject(GameObject key, List<GameObject> value) {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public class PooledObjectsClass{
        [SerializeField] private List<PooledObject> pooledObjects;
        public PooledObjectsClass() {
            pooledObjects = new List<PooledObject>();
        }

        public bool ContainsKey(GameObject key) {
            foreach (var pool in pooledObjects) {
                if (pool.key == key) return true;
            }
            
            return false;
        }

        public void Add(GameObject key, GameObject value) {
            bool isFounded = false;
            foreach (var pool in pooledObjects) {
                if (pool.key == key) {
                    pool.value.Add(value);
                    isFounded = true;
                    break;
                }
            }

            if (!isFounded) {
                pooledObjects.Add(new PooledObject(key, new List<GameObject> { value }));
            }
        }

        public void Add(GameObject key, List<GameObject> values) {
            bool isFounded = false;
            foreach (var pool in pooledObjects) {
                if (pool.key == key) {
                    foreach (var value in values) {
                        pool.value.Add(value);
                    }
                    isFounded = true;
                    break;
                }
            }

            if (!isFounded) {
                pooledObjects.Add(new PooledObject(key, values));
            }
        }

        public void Remove() {
            
        }

        public bool TryGetValue(GameObject key, out List<GameObject> values) {
            foreach (var pool in pooledObjects) {
                if (pool.key == key) {
                    values = pool.value;
                    return true;
                }
            }

            values = new List<GameObject>();
            return false;
        }

        public List<GameObject> Keys() {
            List<GameObject> keys = new List<GameObject>();
            foreach (var pool in pooledObjects) {
                keys.Add(pool.key);
            }

            return keys;
        }

        public void SetValuesFormKey(GameObject key, List<GameObject> values) {
            foreach (var pool in pooledObjects) {
                if (pool.key == key) {
                    pool.value = values;
                    return;
                }
            }
        }
    }
    private PooledObjectsClass pooledObjects = new PooledObjectsClass();
    private SpawnedObjectsClass spawnedObjects = new SpawnedObjectsClass();

    /// <summary>
    /// Call this to register gameobject with Pool, so the game object will be add to pooled when recycle, else it will be destroy.
    /// <para>If your scrip inhenrit from interface "IPoolable", you no longer need to call this method.</para>
    /// </summary>
    public void RegisterPool(GameObject prefab, int initialPoolSize) {
        if ((Object)prefab == (Object)null)
            return;
        List<GameObject> gameObjectList = (List<GameObject>)null;
        if (SingletonBehaviour<PoolManager>.Instance.pooledObjects.ContainsKey(prefab)) {
            SingletonBehaviour<PoolManager>.Instance.pooledObjects.TryGetValue(prefab, out gameObjectList);
            if (gameObjectList == null) {
                gameObjectList = new List<GameObject>();
                SingletonBehaviour<PoolManager>.Instance.pooledObjects.SetValuesFormKey(prefab, gameObjectList);
            }
        }
        else {
            gameObjectList = new List<GameObject>();
            SingletonBehaviour<PoolManager>.Instance.pooledObjects.Add(prefab, gameObjectList);
        }

        if (initialPoolSize <= gameObjectList.Count)
            return;
        while (gameObjectList.Count < initialPoolSize) {
            GameObject gameObject = Object.Instantiate<GameObject>(prefab, this.transform);
            gameObject.SetActive(false);
            gameObjectList.Add(gameObject);
        }
    }

    public T Spawn<T>(
        T prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation)
        where T : Component, IPoolable {
        return this.Spawn<T>(prefab, parent, position, scale, rotation, true);
    }

    public T Spawn<T>(
        T prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        bool createPoolIfNeed)
        where T : Component {
        if (!((Object)prefab == (Object)null) && !((Object)prefab.gameObject == (Object)null))
            return this.Spawn(prefab.gameObject, parent, position, scale, rotation, createPoolIfNeed).GetComponent<T>();
        Logs.LogError("[PoolManager] Cannot spawn a null object. Return value=null");
        return default(T);
    }

    public GameObject Spawn(GameObject prefab, Transform parent) =>
        this.Spawn(prefab, parent, Vector3.zero, Vector3.one, Quaternion.identity, true);

    public GameObject Spawn(
        GameObject prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        bool createPoolIfNeed) {
        if ((Object)prefab == (Object)null) {
            Logs.LogError("[PoolManager] Cannot spawn a null prefab!");
            return (GameObject)null;
        }

        if (!createPoolIfNeed)
            createPoolIfNeed = prefab.GetComponent<IPoolable>() != null;
        GameObject gameObject = (GameObject)null;
        List<GameObject> gameObjectList;
        if (!SingletonBehaviour<PoolManager>.Instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
            return SingletonBehaviour<PoolManager>.Instance.GetObject(gameObject, prefab, parent, position, scale,
                rotation, createPoolIfNeed, createPoolIfNeed);
        if (gameObjectList.Count <= 0)
            return SingletonBehaviour<PoolManager>.Instance.GetObject(gameObject, prefab, parent, position, scale,
                rotation);
        while ((Object)gameObject == (Object)null && gameObjectList.Count > 0) {
            gameObject = gameObjectList[0];
            gameObjectList.RemoveAt(0);
        }

        int num = (Object)gameObject != (Object)null ? 1 : 0;
        return SingletonBehaviour<PoolManager>.Instance.GetObject(gameObject, prefab, parent, position, scale,
            rotation);
    }

    private GameObject GetObject(
        GameObject obj,
        GameObject prefab,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Quaternion rotation,
        bool addToSpawns = true,
        bool createPool = false) {
        if ((Object)obj == (Object)null)
            obj = Object.Instantiate<GameObject>(prefab, position, rotation, parent);
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.localScale = scale;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        if (createPool)
            this.RegisterPool(obj, 0);
        if (addToSpawns)
            SingletonBehaviour<PoolManager>.Instance.spawnedObjects.Add(new SpawnedObjectClass(obj, prefab));
        obj.GetComponent<IPoolable>()?.OnSpawnCallback();
        return obj;
    }

    public void Recycle<T>(T obj) where T : Component {
        if ((Object)obj == (Object)null)
            return;
        this.Recycle(obj.gameObject);
    }

    public void Recycle(GameObject obj) {
        if ((Object)obj == (Object)null)
            return;
        IPoolable component = obj.GetComponent<IPoolable>();
        component?.OnRecycleCallback();
        obj.transform.SetParent(SingletonBehaviour<PoolManager>.Instance.transform);
        obj.SetActive(false);
        GameObject key;
        if (SingletonBehaviour<PoolManager>.Instance.spawnedObjects.TryGetValue(obj, out key)) {
            SingletonBehaviour<PoolManager>.Instance.spawnedObjects.Remove(obj);
            if ((Object)key != (Object)null && SingletonBehaviour<PoolManager>.Instance.pooledObjects.ContainsKey(key))
                SingletonBehaviour<PoolManager>.Instance.pooledObjects.Add(key, obj);
            else if ((Object)key != (Object)null && component != null)
                SingletonBehaviour<PoolManager>.Instance.pooledObjects.Add(key, new List<GameObject>() {
                    obj
                });
            else
                Object.Destroy((Object)obj);
        }
        else
            Object.Destroy((Object)obj);
    }

    public void RecycleAll<T>(T prefab) where T : Component {
        if ((Object)prefab == (Object)null)
            return;
        this.RecycleAll(prefab.gameObject);
    }

    public void RecycleAll(GameObject prefab) {
        if ((Object)prefab == (Object)null)
            return;
        List<GameObject> gameObjectList = new List<GameObject>();
        foreach (var spawnedObject in SingletonBehaviour<PoolManager>.Instance.spawnedObjects.SpawnedObjects) {
            if ((Object)spawnedObject.key != (Object)null && (Object)spawnedObject.value == (Object)prefab)
                gameObjectList.Add(spawnedObject.key);
        }

        for (int index = 0; index < gameObjectList.Count; ++index)
            this.Recycle(gameObjectList[index]);
    }

    public void RecycleAll() {
        List<GameObject> gameObjectList = new List<GameObject>();
        gameObjectList.AddRange((IEnumerable<GameObject>)SingletonBehaviour<PoolManager>.Instance.spawnedObjects.Keys());
        for (int index = 0; index < gameObjectList.Count; ++index)
            this.Recycle(gameObjectList[index]);
    }

    /// <summary>
    /// To check if gameobject has been registered with pool, no need to register again.
    /// </summary>
    public bool IsRegistered(GameObject obj) {
        if ((Object)obj == (Object)null) {
            Logs.LogError("[PoolManager] Cannot check 'IsPoolable' with null game object.");
            return false;
        }

        return this.HasPooled(obj) || this.HasSpawned(obj) || obj.GetComponent<IPoolable>() != null;
    }

    /// <summary>
    /// To check if gameobject can be spawn from pool, no need to instantiate new gameobject.
    /// </summary>
    public bool HasPooled(GameObject obj) => SingletonBehaviour<PoolManager>.Instance.pooledObjects.ContainsKey(obj);

    /// <summary>
    /// To check if gameobject has been spawned (currently active in scene)
    /// </summary>
    public bool HasSpawned(GameObject obj) => SingletonBehaviour<PoolManager>.Instance.spawnedObjects.ContainsKey(obj);

    /// <summary>
    /// Destroy gameobject which is register with pool (include pooled and spawned)
    /// </summary>
    public void UnRegisterPool(GameObject prefab) {
        if ((Object)prefab == (Object)null || !this.IsRegistered(prefab))
            return;
        this.RecycleAll(prefab);
        this.DestroyPooled(prefab);
    }

    /// <summary>
    /// Destroy gameobject which is register with pool (include pooled and spawned)
    /// </summary>
    public void UnRegisterPool<T>(T prefab) where T : Component {
        if ((Object)prefab == (Object)null)
            return;
        this.UnRegisterPool(prefab.gameObject);
    }

    /// <summary>
    /// Destroy all gameobject which is currently in pooled (currently disable in scene)
    /// <para>No action with spawned (currently active in scene)</para>
    /// </summary>
    public void DestroyPooled(GameObject prefab) {
        List<GameObject> gameObjectList;
        if ((Object)prefab == (Object)null ||
            !SingletonBehaviour<PoolManager>.Instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
            return;
        for (int index = 0; index < gameObjectList.Count; ++index)
            Object.Destroy((Object)gameObjectList[index]);
        gameObjectList.Clear();
    }

    /// <summary>
    /// Destroy all gameobject which is currently in pooled (currently disable in scene)
    /// <para>No action with spawned (currently active in scene)</para>
    /// </summary>
    public void DestroyPooled<T>(T prefab) where T : Component {
        if ((Object)prefab == (Object)null)
            return;
        this.DestroyPooled(prefab.gameObject);
    }

    /// <summary>
    /// Destroy all gameobject which is managing by PoolManager, include all spawned and pooled.
    /// </summary>
    public void DestroyAll() {
        try {
            this.RecycleAll();
            List<GameObject> gameObjectList = new List<GameObject>();
            gameObjectList.AddRange(pooledObjects.Keys());
            for (int index = 0; index < gameObjectList.Count; ++index)
                this.DestroyPooled(gameObjectList[index]);
        }
        catch { }
    }
}