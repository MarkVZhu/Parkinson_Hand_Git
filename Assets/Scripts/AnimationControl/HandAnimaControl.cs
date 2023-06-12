using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimaControl : MonoBehaviour
{
	public Animation handAnim;
	public string handAnimName;
	public Animation addAnim;
	public string addAnimName;
	public float animaSpeed;
	[SerializeField][Range(0,1)] float handValue;

	// Update is called once per frame
	void Update()
	{
		if(handValue > handAnim[handAnimName].normalizedTime + 0.02f)
		{
			handAnim[handAnimName].speed = animaSpeed;
			if(addAnim) addAnim[addAnimName].speed = animaSpeed;
		}
		else if(handValue < handAnim[handAnimName].normalizedTime - 0.02f)
		{
			handAnim[handAnimName].speed = -animaSpeed;
			if(addAnim) addAnim[addAnimName].speed = -animaSpeed;
		}
		else
		{
			handAnim[handAnimName].speed = 0;
			if(addAnim) addAnim[addAnimName].speed = 0;
		}	
	}

	public float GetHandAnimeNormalize()
	{
		return handAnim[handAnimName].normalizedTime;
	}
	
	public float GetHandValue()
	{
		return handValue;
	}

	public void SetHandValue(float value)
	{
		handValue = value;
	}
	
}
