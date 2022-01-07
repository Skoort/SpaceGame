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

        [SerializeField] private bool _invertPitch = false;

        private Rigidbody _rb;

		private float _currentThrust;
        private float _currentStrafe;
        private float _currentUpDown;
        private float _currentRoll;
        private Vector2 _currentPitchYaw;


        private float _distanceToMouseCapturePlane = 100;
        private Plane _mouseCapturePlane;
        private Transform _trackedLocation;

		private void Awake()
		{
            _rb = GetComponent<Rigidbody>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
		}

		private void FixedUpdate()
		{
            HandleMovement();
		}

        private void CalculateHeading()
        { 
        
        }

		private void HandleMovement()
		{
            _currentThrust = Input.GetAxis("Vertical");
            _currentUpDown = Input.GetAxis("VerticalThrust");
            _currentStrafe = Input.GetAxis("Horizontal");
            _currentRoll = Input.GetAxis("Roll");
            _currentPitchYaw = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            var fixedDeltaTime = Time.fixedDeltaTime;

            // Roll
            _rb.AddRelativeTorque(Vector3.back * _currentRoll * _rollTorque * fixedDeltaTime);

			// Pitch
			_rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(_currentPitchYaw.y * (_invertPitch ? +1 : -1), -1F, +1F) * _pitchTorque * fixedDeltaTime);

			// Yaw
			_rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(_currentPitchYaw.x, -1, +1) * _yawTorque * fixedDeltaTime);

            // Thrust
            if (_currentThrust > 0.1F)
            {
                _rb.AddRelativeForce(Vector3.forward * _currentThrust * _thrust * fixedDeltaTime);
            } else
            if (_currentThrust < -0.1F)
            {
                _rb.AddRelativeForce(Vector3.forward * _currentThrust * _thrust * 0.5F * fixedDeltaTime);
            }

            // Up/down
            if (_currentUpDown > 0.1F || _currentUpDown < -0.1F)
            {
                _rb.AddRelativeForce(Vector3.up * _currentUpDown * _upThrust * fixedDeltaTime);
            }

            // Strafing
            if (_currentStrafe > 0.1F || _currentStrafe < -0.1F)
            {
                _rb.AddRelativeForce(Vector3.right * _currentStrafe * _strafeThrust * fixedDeltaTime);
            }
        }
	}
}
