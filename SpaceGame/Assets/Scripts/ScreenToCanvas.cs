using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenToCanvas : MonoBehaviour
{
	public RectTransform CanvasRoot;
	public Canvas Canvas;
	public Camera _camera;
	public RectTransform _elementRoot;
	public Transform _trackObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera, TargetLead.transform.position);

		var screenPosition = _camera.WorldToScreenPoint(_trackObject.position);
		screenPosition.z = 0;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			Canvas.GetComponent<RectTransform>(),
			//Input.mousePosition,
			screenPosition,
			Canvas.renderMode == RenderMode.ScreenSpaceOverlay
				? null
				: _camera,
			out var newPos);

		_elementRoot.anchoredPosition = newPos;
	}
}
