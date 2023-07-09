using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	[SerializeField] float respawnTime = 20;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] int extraMonsters = 2;
	[SerializeField] GameObject[] monsterPrefabs;
	[SerializeField] int extraMonsterSpawnTime;
	public static MonsterManager instance {get; private set;}
	void Awake()
	{
		if(instance != null){
			Destroy(gameObject);
			Debug.LogError("Another instance of MonsterManager already exists!");
			return;
		}
		instance = this;
		StartCoroutine(SpawnExtraMonster());
	}
	public void MonsterDestroyed(GameObject originalPrefab){
		StartCoroutine(RespawnMonster(originalPrefab));
	}
	IEnumerator RespawnMonster(GameObject prefab){
		yield return new WaitForSeconds(20);
		Instantiate(prefab,spawnPoints[Random.Range(0,spawnPoints.Length)].position,Quaternion.identity);
	}
	IEnumerator SpawnExtraMonster(){
		yield return new WaitForSeconds(extraMonsterSpawnTime);
		Instantiate(monsterPrefabs[Random.Range(0,monsterPrefabs.Length)],spawnPoints[Random.Range(0,spawnPoints.Length)].position,Quaternion.identity);
		extraMonsters --;
		if(extraMonsters > 0){
			StartCoroutine(SpawnExtraMonster());
		}
	}

	void Update()
	{
		
	}
}
