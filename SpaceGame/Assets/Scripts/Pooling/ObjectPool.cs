using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Pooling
{
	public class ObjectPool : MonoBehaviour
    {
        private Dictionary<string, Queue<GameObject>> _availableObjects;

        public static ObjectPool Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                _availableObjects = new Dictionary<string, Queue<GameObject>>();
            }
            else
            {
                Debug.LogError($"Attempted to make another ObjectPool instance {this.name}!");
                Destroy(this);
            }
        }

        public GameObject RequestObject(string resourceName, GameObject objectPrefab, Transform desiredTransform = null)
        {
            GameObject myObject;
            if (_availableObjects.TryGetValue(resourceName, out var queue))
            {
                myObject = queue.Dequeue();
                if (queue.Count <= 0)
                {
                    _availableObjects.Remove(objectPrefab.name);
                }
            }
            else
            {
                myObject = Instantiate(objectPrefab, this.transform);
            }

            if (desiredTransform != null)
            {
                myObject.transform.position = desiredTransform.position;
                myObject.transform.rotation = desiredTransform.rotation;
            }

            if (!myObject.activeInHierarchy)
            {
                myObject.SetActive(true);
            }

            return myObject;
        }

        public void ReleaseObject(IPoolable myObject)
        {
            Debug.Log($"Releasing resource {myObject.ResourceName} {myObject.InstanceObject?.name}.");

            if (!_availableObjects.TryGetValue(myObject.ResourceName, out var queue))
            {
                queue = new Queue<GameObject>();
                _availableObjects.Add(myObject.ResourceName, queue);
            }
            myObject.InstanceObject.SetActive(false);
            queue.Enqueue(myObject.InstanceObject);
        }
    }
}
