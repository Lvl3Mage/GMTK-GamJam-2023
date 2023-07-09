using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance {get; private set;}
	[SerializeField] float maxCargo;
	float currentCargo = 0;
	void Awake()
	{
		if(instance != null){
			Destroy(gameObject);
			Debug.LogError("Another instance of GameManager already exists!");
			return;
		}
		instance = this;
	}
	public void ChangeCargo(float change){

		if(currentCargo >= maxCargo){return;}

		currentCargo += change;
		currentCargo = Mathf.Clamp(currentCargo, 0, maxCargo);
		if(currentCargo >= maxCargo){
			// gameOver
		}
	}

	void Update()
	{
		
	}
}
