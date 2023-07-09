using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance {get; private set;}
	[SerializeField] float maxCargo;
	SliderController cargoSlider;
	GameOverMenu gameOverMenu;
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
	void Start(){
		cargoSlider = GameObject.FindGameObjectWithTag("Cargo").GetComponent<SliderController>();
		cargoSlider.SetRange(0,maxCargo);
		cargoSlider.SetValue(maxCargo-currentCargo);

		gameOverMenu = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOverMenu>();

	}
	public void ChangeCargo(float change){
		if(gameOver){return;}

		currentCargo += change;
		currentCargo = Mathf.Clamp(currentCargo, 0, maxCargo);
		cargoSlider.SetValue(maxCargo-currentCargo);
		if(currentCargo >= maxCargo){
			gameOver = true;
			ShowGameOver();
		}
	}
	public bool gamePaused {get; private set;}
	public bool gameOver {get; private set;}
	public void TogglePause(bool value){
		gamePaused = value;
	}
	void ShowGameOver(){
		Timer timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
		gameOverMenu.Open(timer.GetTimePassed());
	}

	void Update()
	{
		
	}
}
