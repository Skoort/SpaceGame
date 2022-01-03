using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame
{ 
    [RequireComponent(typeof(Rigidbody))]
    public class Spaceship : MonoBehaviour
    {
        [Header("*** Ship Movement Settings ***")]
        [SerializeField] private float _yawTorque = 500F;
        [SerializeField] private float _pitchTorque = 1000F;
        [SerializeField] private float _rollTorque = 1000F;
        [SerializeField] private float _thrust = 100F;
        [SerializeField] private float _upThrust = 50F;
        [SerializeField] private float _strafeThrust = 50F;
        [SerializeField, Range(0.001F, 0.999F)]
        private float _thrustGlideReduction = 0.999F;
        [SerializeField, Range(0.001F, 0.999F)]
        private float _upDownGlideReduction = 0.111F;
        [SerializeField, Range(0.001F, 0.999F)]
        private float _leftRightGlideReduction = 0.111F;

        [SerializeField] private bool _invertPitch = false;

        private Rigidbody _rb;

		private float _currentThrust;
        private float _currentStrafe;
        private float _currentUpDown;
        private float _currentRoll;
        private Vector2 _currentPitchYaw;

        private float _glide;
        private float _verticalGlide;
        private float _horizontalGlide;

		private void Awake()
		{
            _rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
            HandleMovement();
		}

		private void HandleMovement()
		{
            _currentThrust = Input.GetAxis("Vertical");
            _currentUpDown = Input.GetAxis("VerticalThrust");
            _currentStrafe = Input.GetAxis("Horizontal");
            _currentRoll = Input.GetAxis("Roll");
            _currentPitchYaw = new Vector2(0, Input.GetAxis("Mouse Y"));

            // Roll
            _rb.AddRelativeTorque(Vector3.back * _currentRoll * _rollTorque * Time.fixedDeltaTime);

			// Pitch
			_rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(_currentPitchYaw.y * (_invertPitch ? +1 : -1), -1F, +1F) * _pitchTorque * Time.fixedDeltaTime);

			// Yaw
			_rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(_currentPitchYaw.x, -1, +1) * _yawTorque * Time.fixedDeltaTime);

            // Thrust
            if (_currentThrust > 0.1F || _currentThrust < -0.1F)
            {
                var currentThrust = _thrust;
                _rb.AddRelativeForce(Vector3.forward * _currentThrust * _thrust * Time.fixedDeltaTime);
                _glide = _thrust;
            }
            else
            {
                _rb.AddRelativeForce(Vector3.forward * _glide * Time.fixedDeltaTime);
                _glide -= _thrustGlideReduction;
                if (_glide < 0)
                {
                    _glide = 0;
                }
            }

            // Up/down
            if (_currentUpDown > 0.1F || _currentUpDown < -0.1F)
            {
                _rb.AddRelativeForce(Vector3.up * _currentUpDown * _upThrust * Time.fixedDeltaTime);
                _verticalGlide = _upThrust;
            }
            else
            {
                _rb.AddRelativeForce(Vector3.up * _verticalGlide * Time.fixedDeltaTime);
                _verticalGlide -= _upDownGlideReduction;
                if (_verticalGlide < 0)
                {
                    _verticalGlide = 0;
                }
            }

            // Strafing
            if (_currentStrafe > 0.1F || _currentStrafe < -0.1F)
            {
                _rb.AddRelativeForce(Vector3.right * _currentStrafe * _strafeThrust * Time.fixedDeltaTime);
                _horizontalGlide = _strafeThrust;
            }
            else
            {
                _rb.AddRelativeForce(Vector3.right * _horizontalGlide * Time.fixedDeltaTime);
                _horizontalGlide -= _leftRightGlideReduction;
                if (_horizontalGlide < 0)
                {
                    _horizontalGlide = 0;
                }
            }
        }
	}
}
