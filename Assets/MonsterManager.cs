using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

	public static MonsterManager instance {get; private set;}
	void Awake()
	{
		if(instance != null){
			Destroy(gameObject);
			Debug.LogError("Another instance of MonsterManager already exists!");
			return;
		}
		instance = this;
	}
	public void MonsterDestroyed(){
		
	}

	void Update()
	{
		
	}
}
