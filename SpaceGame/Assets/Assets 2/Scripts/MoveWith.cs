using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWith : MonoBehaviour
{
	[SerializeField] Transform _objectToMoveWith = default;
	private Vector3 _offsetToKeep;
	
	private void Awake()
	{
		_offsetToKeep = transform.position - _objectToMoveWith.position;
	}
	
    private void LateUpdate()
    {
        transform.position = _objectToMoveWith.position + _offsetToKeep;
    }
}
