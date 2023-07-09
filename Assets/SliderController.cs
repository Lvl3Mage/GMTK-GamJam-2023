using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	[SerializeField] Slider slider;
	void Start()
	{
		
	}
	public void SetRange(float min, float max){
		slider.minValue = min;
		slider.maxValue = max;
	}
	public void SetValue(float value){
		slider.value = value;
	}


	void Update()
	{
		
	}
}
