using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.Weapons.Projectiles

{ 
    public class RaycastProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 0;

        [SerializeField] private UnityEvent _onHit = null;
        [SerializeField] private UnityEvent _onMiss = null;

		private void OnEnable()
		{
			
		}

		private void OnDisable()
		{
			
		}

		private void FixedUpdate()
		{
			
		}
	}
}
