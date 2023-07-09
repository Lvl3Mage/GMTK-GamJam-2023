using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
	[SerializeField] float cannonRange;
	[SerializeField] float fireRate;
	[SerializeField] GameObject muzzleFlashPrefab;
	[SerializeField] Rigidbody2D bulletPrefab;
	[SerializeField] Transform gunPoint;
	void Start()
	{
		
	}
	bool shotReady = true;

	void Update()
	{
		Monster target = GetClosestEmerged(GetMonstersInRange(cannonRange));
		if(target == null){
			// transform.rotation = Quaternion.Euler(0,0,0);
			return;
		}

		Vector2 targetDelta = target.transform.position - transform.position;
		transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(targetDelta.y,targetDelta.x)*Mathf.Rad2Deg - 90);

		if(shotReady){
			StartCoroutine(Shoot(target));
		}
	}
	IEnumerator Shoot(Monster target){
		shotReady = false;
		Instantiate(muzzleFlashPrefab, gunPoint.position, gunPoint.rotation);
		Rigidbody2D bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
		yield return new WaitForSeconds(1/fireRate);
		shotReady = true;
	}
	Monster[] GetMonstersInRange(float range){
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range, 1 << 10 /* Monster layer index */);
		List<Monster> monsters = new List<Monster>();
		for(int i = 0; i < cols.Length; i++){
			Monster curMonster = cols[i].GetComponentInParent<Monster>();
			bool found = false;
			foreach(Monster monster in monsters){
				if(curMonster == monster){
					found = true;
					break;
				}
			}
			if(!found){
				monsters.Add(curMonster);
			}
		}
		return monsters.ToArray();
	}
	Monster GetClosestEmerged(Monster[] monsters){
		float minDistSqr = Mathf.Infinity;
		Monster closest = null;
		foreach(Monster monster in monsters){
			if(monster.emerged){
				float distSqr = (monster.transform.position - transform.position).sqrMagnitude;
				if(minDistSqr > distSqr){
					minDistSqr = distSqr;
					closest = monster;
				}
			}
		}
		return closest;
	}
}
