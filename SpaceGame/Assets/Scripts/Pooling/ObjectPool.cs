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

        public GameObject RequestObject(string resourceName, GameObject objectPrefab, Vector3? desiredPosition = null, Quaternion? desiredRotation = null, Transform desiredParent = null)
        {
            GameObject myObject;
            if (_availableObjects.TryGetValue(resourceName, out var queue) && queue.Count > 0)  // For some reason sometimes this queue can be empty.
            {
                myObject = queue.Dequeue();
                if (queue.Count <= 0)
                {
                    _availableObjects.Remove(objectPrefab.name);
                }

                myObject.SetActive(true);
            }
            else
            {
                myObject = Instantiate(objectPrefab, desiredParent ?? this.transform);
            }

            if (desiredPosition != null)
            {
                myObject.transform.position = desiredPosition.Value;
            }
            if (desiredRotation != null)
            { 
                myObject.transform.rotation = desiredRotation.Value;
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
            myObject.InspectorOnReclaimedByObjectPool.Invoke();
            myObject.ResetTimer();
            queue.Enqueue(myObject.InstanceObject);
        }
    }
}
