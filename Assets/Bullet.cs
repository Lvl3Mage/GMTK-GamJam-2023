using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] Rigidbody2D rigidbody;
	[SerializeField] float bulletVelocity;
	[SerializeField] float damage;
	bool destroyed;
	void Start()
	{
		rigidbody.velocity = transform.up*bulletVelocity;
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(destroyed) return;
		Monster monster = collision.collider.GetComponentInParent<Monster>();
		if(monster != null){
			monster.Attack(damage);
		}
		destroyed = true;
		//Instantiate splash effect
		Destroy(gameObject);
	}
}
