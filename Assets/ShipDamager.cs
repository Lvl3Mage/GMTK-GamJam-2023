using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDamager : MonoBehaviour
{
	[SerializeField] float damage, attackCooldown;
	[SerializeField] CollisionCallbacker[] callbackers;
	bool attackOnCooldown = false;
	void Awake(){
		foreach(CollisionCallbacker callbacker in callbackers){
			callbacker.OnStay += OnCollisionStay;
		}
	}
	void OnCollisionStay(Collision2D collision, CollisionCallbacker callbacker){
		if(attackOnCooldown){return;}
		if(collision.collider.gameObject.layer != 9){return;}
		Ship ship = collision.collider.attachedRigidbody.gameObject.GetComponent<Ship>();
		StartCoroutine(Attack(ship));
	}
	IEnumerator Attack(Ship target){
		target.Attack(damage);
		attackOnCooldown = true;
		yield return new WaitForSeconds(attackCooldown);
		attackOnCooldown = false;
	}
}
