using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
	[SerializeField] Animator animator;
	[SerializeField] TextWriter BestTimeText;
	[SerializeField] TextWriter CurrentTimeText;
	[SerializeField] Color beatBestColor;
	void Start()
	{
		
	}


	void Update()
	{
		
	}
	public void Open(int totalSeconds){
		animator.SetTrigger("Open");
		SlowMotion.LerpSlowDown(0.02f, 0.5f, this);
		GameManager.instance.TogglePause(true);

		CurrentTimeText.Set(GetTimeStr(totalSeconds));
		int bestTime = 0;
		if(PlayerPrefs.HasKey("bestTime")){
			bestTime = PlayerPrefs.GetInt("bestTime");
		}
		if(totalSeconds >= bestTime){
			CurrentTimeText.Set(beatBestColor);
			bestTime = totalSeconds;
		}

		PlayerPrefs.SetInt("bestTime", totalSeconds);
		BestTimeText.Set(GetTimeStr(bestTime));


	}
	string GetTimeStr(int totalSeconds){
		int seconds = (int)Mathf.Floor(totalSeconds % 60);
		string secondsString = seconds.ToString();
		if(seconds<10){
			secondsString = "0" + secondsString;
		}
		
		int minutes = (int)Mathf.Floor(totalSeconds / 60);
		string minutesString = minutes.ToString();
		if(minutes<10){
			minutesString = "0" + minutesString;
		}

		return minutesString + ":" + secondsString;
	}
}
