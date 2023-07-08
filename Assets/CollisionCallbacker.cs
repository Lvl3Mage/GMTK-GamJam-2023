using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCallbacker : MonoBehaviour
{
	public delegate void CollisionCallback(Collision2D collision, CollisionCallbacker self);
	public event CollisionCallback OnStay;
	public event CollisionCallback OnEnter;
	public event CollisionCallback OnExit;

	void OnCollisionStay2D(Collision2D collision){
		if(OnStay == null){return;}
		OnStay.Invoke(collision, this);
	}
	void OnCollisionEnter2D(Collision2D collision){
		if(OnEnter == null){return;}
		OnEnter.Invoke(collision, this);
	}
	void OnCollisionExit2D(Collision2D collision){
		if(OnExit == null){return;}
		OnExit.Invoke(collision, this);
	}
}
