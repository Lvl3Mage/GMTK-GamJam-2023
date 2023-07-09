using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class SlowMotion
{
    public delegate void TimeChange(float growthFraction);
    public static event TimeChange OnTimeChange;
    static float realPhysicsTime = 0.02f;
    static float realTime = 1;
    static Coroutine Timelerp;
    public static void SlowDown(float speed){
        SetTimeScale(speed);
    }
    public static void SpeedUp(){
        SetTimeScale(realTime);
        
    }
    public static void LerpSlowDown(float speed, float duration, MonoBehaviour self){
        if(Timelerp != null){
            self.StopCoroutine(Timelerp);
        }
        Timelerp = self.StartCoroutine(lerpTime(speed, duration));
    }
    static IEnumerator lerpTime(float finaltime, float duration){
        float originaltime = Time.timeScale;
        for(float elapsed = 0; elapsed <= duration; elapsed += Time.deltaTime){
            float newTime = Mathf.Lerp(originaltime, finaltime, elapsed/duration);
            SetTimeScale(newTime);
            yield return null;
        }
        SetTimeScale(finaltime);
    }
    public static void LerpSpeedUp(float duration, MonoBehaviour self){
        if(Timelerp != null){
            self.StopCoroutine(Timelerp);
        }
        Timelerp = self.StartCoroutine(lerpTime(realTime, duration));
    }
    static void SetTimeScale(float newTimeScale){
        float delta = Time.timeScale/newTimeScale;

        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = newTimeScale * (realPhysicsTime/realTime);
        if(OnTimeChange != null){
            OnTimeChange(delta);
        }
        
    }
    public static void ClearSubscribers(){
        OnTimeChange = null;
    }
}
