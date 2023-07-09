using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poppup : MonoBehaviour
{
	[SerializeField] TextWriter text;
	[SerializeField] Color posColor, negColor;
	public void SetPoints(int points){
		transform.rotation = Quaternion.Euler(0,0,Random.Range(-10f,10f));
		if(points>0){
			
			text.Set(-points);
			text.Set(posColor);
		}
		else if(points<0){

			string strVal = "+" + points*-1;
			text.Set(strVal);
			text.Set(negColor);
		}
		else{
			Destroy(gameObject);
		}
	}
	void Remove(){
		Destroy(gameObject);
	}
}
