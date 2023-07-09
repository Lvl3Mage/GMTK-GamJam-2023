using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
	[SerializeField] PrefabConnection originalPrefab;
	[SerializeField] float health;
	[SerializeField] bool debug;
	[SerializeField] float lockOnRange;
	[SerializeField] float acceleration, maxSpeed, maxSpeedDistance;
	[SerializeField] Animator animator;
	[SerializeField] GameObject deathEffect;
	protected Rigidbody2D rigidbody;
	public bool emerged {get; private set;}
	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	
	void Update()
	{
		Collider2D closestShip = GetClosest(GetShipsInRange(lockOnRange));
		
		if(closestShip == null){
			if(emerged){
				emerged = false;
				ToggleEmerged(emerged);
			}
			return;
		}

		if(!emerged){
			emerged = true;
			ToggleEmerged(emerged);
		}
		OnTargetDetected(closestShip.transform);


		Vector2 delta = closestShip.gameObject.transform.position - transform.position;
		rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, Mathf.Min(delta.magnitude/maxSpeedDistance, 1) *maxSpeed * delta.normalized, 1-Mathf.Pow(1-acceleration, Time.deltaTime));

	}
	protected virtual void ToggleEmerged(bool toggleVal){}
	protected virtual void OnTargetDetected(Transform target){}
	public void Attack(float damage){
		if(health <= 0){
			return;	
		}
		health -= damage;
		if(health <= 0){
			Instantiate(deathEffect, transform.position, transform.rotation);
			animator.SetTrigger("Death");
			//Die
			return;
		}
		//Hurt effect?

	}
	void Die(){
		MonsterManager.instance.MonsterDestroyed(originalPrefab.GetPrefab());
		Destroy(gameObject);
	}
	Collider2D[] GetShipsInRange(float range){
		return Physics2D.OverlapCircleAll(transform.position, range, 1 << 9 /* Ship layer index */);
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
