using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private Action<T> pullObject;
        private Action<T> pushObject;
        private Stack<T> pooledObjects = new();
        private GameObject prefab;
        private List<T> spawnedObjects = new();
        private GameObject pooledObjectParent;

        public int spawnedCount
        {
            get { return spawnedObjects.Count; }
        }

        public int pooledCount
        {
            get { return pooledObjects.Count; }
        }

        public ObjectPool(GameObject pooledObject, int numToSpawn = 0)
        {
            spawnedObjects.Clear();
            pooledObjectParent = new GameObject(pooledObject.name + "_Pool");
            this.prefab = pooledObject;
            Spawn(numToSpawn);
        }

        public ObjectPool(GameObject pooledObject, Action<T> pullObject, Action<T> pushObject, int numToSpawn = 0)
        {
            spawnedObjects.Clear();
            this.prefab = pooledObject;
            this.pullObject = pullObject;
            this.pushObject = pushObject;
            Spawn(numToSpawn);
        }

        public T Pull()
        {
            T t;
            if (pooledCount > 0)
                t = pooledObjects.Pop();
            else
            {
                t = GameObject.Instantiate(prefab).GetComponent<T>();
                spawnedObjects.Add(t);
            }

            t.gameObject.name = prefab.name;
            t.gameObject.SetActive(true);
            t.transform.parent = pooledObjectParent.transform;
            t.CacheAction(Push);
            pullObject?.Invoke(t);

            return t;
        }

        public void PushAll()
        {
            foreach (var poolable in spawnedObjects)
            {
                if (poolable.transform.gameObject.activeInHierarchy)
                    poolable.ReturnToPool();
            }
        }

        public T Pull(Vector3 position)
        {
            T t = Pull();
            t.transform.position = position;
            return t;
        }

        public T Pull(Vector3 position, Quaternion rotation)
        {
            T t = Pull();
            t.transform.position = position;
            t.transform.rotation = rotation;
            return t;
        }

        public GameObject PullGameObject()
        {
            return Pull().gameObject;
        }

        public GameObject PullGameObject(Vector3 position)
        {
            GameObject go = Pull().gameObject;
            go.transform.position = position;
            return go;
        }

        public GameObject PullGameObject(Vector3 position, Quaternion rotation)
        {
            GameObject go = Pull().gameObject;
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }

        public void Push(T t)
        {
            pooledObjects.Push(t);
            pushObject?.Invoke(t);
            t.transform.parent = pooledObjectParent.transform;
            t.gameObject.SetActive(false);
        }

        private void Spawn(int number)
        {
            T t;

            for (int i = 0; i < number; i++)
            {
                t = GameObject.Instantiate(prefab).GetComponent<T>();
                pooledObjects.Push(t);
                t.gameObject.SetActive(false);
            }
        }
    }

    public interface IPool<T>
    {
        T Pull();
        void Push(T t);
    }

    public interface IPoolable<T>
    {
        void CacheAction(Action<T> returnAction);
        void ReturnToPool();
    }
}