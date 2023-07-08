using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDamager : MonoBehaviour
{
	[SerializeField] float damage, attackCooldown;
	bool attackOnCooldown = false;
	void OnCollisionEnter2D(Collision2D collision){
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
