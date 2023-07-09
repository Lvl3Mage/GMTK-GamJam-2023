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
