using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Sprite submergedSprite, emergedSprite;
	[SerializeField] float submergedZPosition = 1, emergedZPosition = -1;
	[SerializeField] float acceleration;
	[SerializeField] Rigidbody2D tentacleEnd;
	Transform target = null;
	public void SetTarget(Transform _target){
		target = _target;
	}
	void Update(){
		if(target != null){
			tentacleEnd.velocity += (Vector2)(target.transform.position - tentacleEnd.transform.position).normalized * acceleration* Time.deltaTime;
		}
	}
	public void ToggleState(bool emerged){
		if(emerged){
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,emergedZPosition);
			spriteRenderer.sprite = emergedSprite;
		}
		else{
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,submergedZPosition);
			spriteRenderer.sprite = submergedSprite;

		}
	}
}
