using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
	[SerializeField] Transform[] targets;
	[SerializeField] Ship[] shipPrefabs;
	[Tooltip("Amount of ships initially sent per minute")]
	[SerializeField] float initialShipRate;
	[Tooltip("Growth of the initial ship rate per minute (measured in ships*minute^-2)")]
	[SerializeField] float rateGrowth;
	[Tooltip("Maximum ships per minute")]
	[SerializeField] float maxShipRate;

	[SerializeField][Range(0,1)] float maxInitialPhase;
	[SerializeField] float initialPhasePower = 1;
	float shipRate;
	float accumulatedShips = 0;
	void Awake()
	{
		accumulatedShips = Mathf.Pow(Random.Range(0f,maxInitialPhase),initialPhasePower);
	}


	void Update()
	{


		float sps = Mathf.Min(initialShipRate + Time.time*rateGrowth, maxShipRate)/60;//ships per second
		accumulatedShips += sps*Time.deltaTime;

		if(accumulatedShips > 1){
			SpawnShip();
			accumulatedShips --;
		}
	}
	void SpawnShip(){
		Ship ship = Instantiate(shipPrefabs[Random.Range(0,shipPrefabs.Length)], transform.position, transform.rotation);
		ship.SetTarget(targets[Random.Range(0,targets.Length)]);
	}
	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		foreach(Transform target in targets){
			Gizmos.DrawLine(transform.position, target.position);

		}
	}
}
