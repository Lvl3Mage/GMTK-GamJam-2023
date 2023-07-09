using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionShortcut : MonoBehaviour
{
	public void SlowDown(float speed){
		SlowMotion.SlowDown(speed);
	}
	public void SpeedUp(){
		SlowMotion.SpeedUp();
		
	}
	public void LerpSlowDown(float speed, float duration){
		SlowMotion.LerpSlowDown(speed, duration, this);
	}
	public void LerpSpeedUp(float duration){
		SlowMotion.LerpSpeedUp(duration, this);
	}
}
