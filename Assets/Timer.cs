using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] TextWriter Minutes;
    [SerializeField] TextWriter Seconds;


    // public void StartTimer(){
    // 	timePassed = 0;
    // 	StartCoroutine(Timer());
    // }
    void Start(){
    	timePassed = 0;
    	StartCoroutine(TimerClock());
    }
    int timePassed;
    IEnumerator TimerClock(){
        while(!GameManager.instance.gameOver){
            SetTime(timePassed);
            yield return new WaitForSeconds(1);
            timePassed++;
        }
    }
    public int GetTimePassed(){
    	return timePassed;
    }
    void SetTime(int totalSeconds){
        int seconds = (int)Mathf.Floor(totalSeconds % 60);
        string secondsString = seconds.ToString();
        if(seconds<10){
            secondsString = "0" + secondsString;
        }
        Seconds.Set(secondsString);
        int minutes = (int)Mathf.Floor(totalSeconds / 60);
        string minutesString = minutes.ToString();
        if(minutes<10){
            minutesString = "0" + minutesString;
        }
        Minutes.Set(minutesString);
    }
}
