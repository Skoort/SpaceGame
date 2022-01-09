using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
	[SerializeField] private Vector3 _axis = default;
	[SerializeField] private float _speed = 30;
	
    private void Update()
    {
		var axisWorldSpace = transform.TransformDirection(_axis);
		
		transform.RotateAround(transform.position, axisWorldSpace.normalized, _speed * Time.deltaTime);
    }
}
