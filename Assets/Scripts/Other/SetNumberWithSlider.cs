using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI
;

public class SetNumberWithSlider : MonoBehaviour
{
	public Slider slider;
	public int divider;

	private void Start() 
	{
		if(divider > 1)
		{
			if(FunctionNum.Instance.time != 30)
			{
				slider.value = FunctionNum.Instance.time/divider;
			}	
		}
		else
		{
			if(FunctionNum.Instance.playersNum != 3)
			{
				slider.value = FunctionNum.Instance.playersNum/divider;
			}	
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		GetComponent<Text>().text = (slider.value * divider).ToString();
		if(divider > 1) FunctionNum.Instance.time = slider.value * divider;
		else FunctionNum.Instance.playersNum = (int)slider.value * divider;
	}
}
