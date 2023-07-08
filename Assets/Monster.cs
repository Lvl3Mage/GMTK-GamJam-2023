using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
	[SerializeField] float health;
	[SerializeField] bool debug;
	[SerializeField] float lockOnRange;
	[SerializeField] float acceleration, maxSpeed, maxSpeedDistance;
	[SerializeField] Tentacle[] tentacles;
	Rigidbody2D rigidbody;
	bool emerged = false;
	void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	
	void Update()
	{
		Collider2D closestShip = GetClosest(GetShipsInRange(lockOnRange));
		
		if(closestShip == null){
			if(emerged){
				ToggleEmerged(false);
			}
			//hide under the water
			return;
		}
		// appear out of the water
		if(!emerged){
			ToggleEmerged(true);
		}

		Vector2 delta = closestShip.gameObject.transform.position - transform.position;

		// if(delta.magnitude < attackRange || !attackOnCooldown){
		// 	//execute attack
		// 	Ship target = closestShip.gameObject.GetComponentInParent<Ship>();
		// 	StartCoroutine(Attack(target));
		// }
		//move towards ship
		rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, Mathf.Min(delta.magnitude/maxSpeedDistance, 1) *maxSpeed * delta.normalized, 1-Mathf.Pow(1-acceleration, Time.deltaTime));

	}
	void ToggleEmerged(bool toggleVal){
		emerged = toggleVal;
		foreach(Tentacle tentacle in tentacles){
			tentacle.ToggleState(toggleVal);
		}
	}
	
	Collider2D[] GetShipsInRange(float range){
		return Physics2D.OverlapCircleAll(transform.position, range, 1 << 9 /* Current layer index */);
	}
	Collider2D GetClosest(Collider2D[] cols){
		float minDistSqr = Mathf.Infinity;
		Collider2D closest = null;
		foreach(Collider2D col in cols){
			float distSqr = (col.transform.position - transform.position).sqrMagnitude;
			if(minDistSqr > distSqr){
				minDistSqr = distSqr;
				closest = col;
			}
		}
		return closest;
	}
	void OnDrawGizmos(){
		if(!debug){return;}
		Gizmos.color = new Color(0.5f,0.5f,1f,0.5f);
		Gizmos.DrawSphere(transform.position, lockOnRange);
	}
}
