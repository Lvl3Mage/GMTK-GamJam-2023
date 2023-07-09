using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Monster
{
	[SerializeField] Transform fin;
	[SerializeField] float rotationSpeed;

	void Start()
	{
		
	}


	void FixedUpdate()
	{
		float targetAngle = Mathf.Atan2(rigidbody.velocity.y,rigidbody.velocity.x)*Mathf.Rad2Deg - 90f;
		float curAngle = rigidbody.rotation;
		rigidbody.angularVelocity = Mathf.DeltaAngle(curAngle, targetAngle)*rotationSpeed;
	}
	protected override void ToggleEmerged(bool toggleVal){
		if(toggleVal){
			fin.localPosition = new Vector3(fin.localPosition.x, fin.localPosition.y, -2f);
		}
		else{

			fin.localPosition = new Vector3(fin.localPosition.x, fin.localPosition.y, -0.1f);
		}
	}
}
