using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentAffectedObject : MonoBehaviour
{
	Rigidbody2D rigidbody;
	CurrentController controller;
	[SerializeField] float inertia;
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		controller = CurrentController.instance;
	}


	void Update()
	{
		rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, controller.GetClosestPoint(transform.position).velocity, 1 - Mathf.Pow(inertia, Time.deltaTime));
	}
}
