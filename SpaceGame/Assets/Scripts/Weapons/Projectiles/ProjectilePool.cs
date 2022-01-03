using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Weapons.Projectiles
{
	public class ProjectilePool : MonoBehaviour
    {
        // Consider forcing tying these two together using a custom editor.
        [SerializeField] private List<GameObject> _projectilePrefabs = default;
        [SerializeField] private List<int> _idealAmountInPool = default;

        private Dictionary<GameObject, Queue<Transform>> _availableProjectiles;

        public static ProjectilePool Instance { get; set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                _availableProjectiles = new Dictionary<GameObject, Queue<Transform>>();
            }
            else
            {
                Debug.LogError($"Attempted to make another ProjectilePool instance {this.name}!");
                Destroy(this);
            }
        }

        public Transform RequestProjectile(GameObject projectilePrefab)
        {
            if (_availableProjectiles.TryGetValue(projectilePrefab, out var queue))
            {
                return queue.Dequeue();
            }
            else
            {
                return Instantiate<GameObject>(projectilePrefab).transform;
            }
        }

        public void ReleaseProjectile(GameObject projectile)
        {
            if (!_availableProjectiles.TryGetValue(projectile, out var queue))
            {
                queue = new Queue<Transform>();
                _availableProjectiles.Add(projectile, queue);
            }
            queue.Enqueue(projectile.transform);
        }
    }
}
