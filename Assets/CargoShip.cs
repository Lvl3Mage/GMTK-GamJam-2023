using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Ship : MonoBehaviour{
	[SerializeField] float Health;
	[SerializeField] GameObject DestroyedShipPrefab;
	[SerializeField] protected float health {get{return Health;} private set{Health = value;}}
	public void Attack(float damage){
		if(health <= 0){return;}
		health -= damage;
		OnAttack(damage);
		if(health <= 0){
			
			OnDestroy();
			Instantiate(DestroyedShipPrefab, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
	public virtual void OnAttack(float damage){}
	public virtual void OnDestroy(){}

	public abstract void SetTarget(Transform newTarget);
}
public class CargoShip : Ship
{
	Rigidbody2D rigidbody;
	[SerializeField] float targetSpeed;
	[SerializeField] float slowdownDistance;
 	[SerializeField] [Range(0,1)] float forwardAcceleration, sideDrag;
	Transform target;
	[SerializeField] float rotationSmoothness;
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}
	public override void SetTarget(Transform newTarget){
		target = newTarget;
	}


	void Update()
	{
		Vector2 targetDelta = target.position - transform.position;

		float forwardComponent = Vector2.Dot(rigidbody.velocity, transform.up);
		float tangentComponent = Vector2.Dot(rigidbody.velocity, transform.right);
		forwardComponent = Mathf.Lerp(forwardComponent, targetSpeed* Mathf.Min(targetDelta.magnitude/slowdownDistance,1), 1 - Mathf.Pow(1-forwardAcceleration,Time.deltaTime));
		tangentComponent = Mathf.Lerp(tangentComponent, 0, 1 - Mathf.Pow(1-sideDrag,Time.deltaTime));
		rigidbody.velocity = forwardComponent*transform.up + tangentComponent*transform.right;

		float targetAngle = Mathf.Atan2(targetDelta.y,targetDelta.x)*Mathf.Rad2Deg - 90f;
		float curAngle = transform.eulerAngles.z;
		curAngle = Mathf.LerpAngle(curAngle, targetAngle, (1 - Mathf.Pow(rotationSmoothness,Time.deltaTime)));
		transform.rotation = Quaternion.Euler(0,0,curAngle);
	}

	/*
		float targetAngle = Mathf.Atan2(rigidbody.velocity.y,rigidbody.velocity.x)*Mathf.Rad2Deg - 90f;
		float curAngle = transform.eulerAngles.z;
		curAngle = Mathf.LerpAngle(curAngle, targetAngle, rigidbody.velocity.magnitude* (1 - Mathf.Pow(rotationSmoothness,Time.deltaTime)));
		transform.rotation = Quaternion.Euler(0,0,curAngle);
	*/
}
