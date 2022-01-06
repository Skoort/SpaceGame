using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Tests
{ 
    public class TestTracking : MonoBehaviour
    {
        [SerializeField] private bool _shouldLerpPosition = false;
        [SerializeField] private bool _shouldUseConstVelocity = false;

        [SerializeField] private Rigidbody _rb = default;

        [SerializeField] private Vector3 _fromVelocity = -Vector3.one;
        [SerializeField] private Vector3 _toVelocity = Vector3.one;

        [SerializeField] private Vector3 _fromPosition = -Vector3.one;
        [SerializeField] private Vector3 _toPosition = Vector3.one;

        private Vector3 _origin;

        private float _progress = 0;
        private bool _lerpDirection = true;

		private void Awake()
		{
            _origin = transform.position;
            if (_shouldUseConstVelocity)
            {
                _rb.velocity = _toVelocity;
            }
		}

		private void FixedUpdate()
        {
            if (_shouldUseConstVelocity)
            {
                return;
            }
            
            _progress += Time.fixedDeltaTime;
            
            if (_shouldLerpPosition)
            {
                var a = _lerpDirection ? _fromPosition : _toPosition;
                var b = _lerpDirection ? _toPosition : _fromPosition;

                transform.position = _origin + Vector3.Lerp(a, b, _progress);
            }
            else
            {
                var a = _lerpDirection ? _fromVelocity : _toVelocity;
                var b = _lerpDirection ? _toVelocity : _fromVelocity;

                _rb.velocity = Vector3.Lerp(a, b, _progress);
            }

            if (_progress >= 1)
            {
                _progress = 0;
                _lerpDirection = !_lerpDirection;
            }
        }
    }
}
