using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] float moveSpeed;
	[SerializeField] Camera camera;
	[SerializeField] float zoomSpeed;
	[SerializeField] float maxSize;
	[SerializeField] float positionSmoothness;
	[SerializeField] float zoomSmoothness;
	void Start()
	{
		prevMousePosition = GetMousePos();
		targetCamPos = transform.position;
		targetCamZoom = camera.orthographicSize;
	}

	Vector2 prevMousePosition;
	Vector2 targetCamPos;
	float targetCamZoom;
	void FixedUpdate()
	{
		Vector2 Axis = Vector2.zero;
		Axis.x = Input.GetAxis("Horizontal");
		Axis.y = Input.GetAxis("Vertical");
		targetCamPos += Axis*Time.fixedDeltaTime*moveSpeed;

		float zoomDelta = Input.mouseScrollDelta.y;
		Vector2 oldMousePos = GetMousePos();

		targetCamZoom += zoomDelta*zoomSpeed;
		targetCamZoom = Mathf.Clamp(targetCamZoom,0.5f,15);

		camera.orthographicSize = Mathf.Exp(Mathf.Lerp(Mathf.Log(camera.orthographicSize,  2.71828f), Mathf.Log(targetCamZoom, 2.71828f), 1-Mathf.Pow(zoomSmoothness, Time.deltaTime)));

		Vector2 newMousePos = GetMousePos();

		transform.position += (Vector3)(oldMousePos - newMousePos);
		targetCamPos += (oldMousePos - newMousePos);

		Vector2 mousePos = newMousePos;
		if(Input.GetMouseButton(1)){
			targetCamPos += (prevMousePosition - mousePos);
		}


		//Lerp camera stats

		transform.position = Vector3.Lerp(transform.position, new Vector3(targetCamPos.x, targetCamPos.y, transform.position.z), 1-Mathf.Pow(positionSmoothness, Time.deltaTime));
		


		prevMousePosition = GetMousePos();
	}

	Vector2 GetMousePos(){
		Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
		return worldPosition;
	}
}
