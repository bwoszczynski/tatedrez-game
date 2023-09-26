using System;
using System.Collections.Generic;
using Gameplay.View;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class ElementsPool<T0>
    {
        public GameObject prefab;
        public List<T0> pool = new();
        
        public T0 GetElementFromPool()
        {
            if (pool.Count > 0)
            {
                T0 element = pool[0];
                pool.RemoveAt(0);

                return element;
            }
            else
            {
                GameObject gameObject = GameObject.Instantiate(prefab);
                return gameObject.GetComponent<T0>();
            }
        }

        public void ReturnToPool(T0 element)
        {
            pool.Add(element);
        }
    }
}