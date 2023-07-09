using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] Animator animator;
	void Start()
	{
		
	}

	bool open = false;
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			TogglePause();
		}
	}
	public void TogglePause(){
		if(!open){
			animator.SetTrigger("Open");
			SlowMotion.LerpSlowDown(0.02f, 0.5f, this);
			GameManager.instance.TogglePause(true);
		}
		else{

			animator.SetTrigger("Close");
			SlowMotion.LerpSpeedUp(0.5f, this);
			GameManager.instance.TogglePause(false);
		}
		open = !open;
	}
}
