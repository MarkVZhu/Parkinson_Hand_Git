using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonAnimaControl : MonoBehaviour
{
	public Animation anim;
	public bool canInflate;
	[Range(0,1)] public float balloonValue;
	
	
	void Start() 
	{
		canInflate = true;
		DataSaveManager.instance.OnInflate += AnimInflate;	
	}
	
	// Update is called once per frame
	void Update()
	{
		//Debug.Log(anim["CloseHand"].normalizedTime);
		if(balloonValue > anim["InflateAnimation"].normalizedTime)
		{
			anim["InflateAnimation"].speed = 1;
		}
		else if(balloonValue < anim["InflateAnimation"].normalizedTime)
		{
			anim["InflateAnimation"].speed = -1;
		}
		else
		{
			anim["InflateAnimation"].speed = 0;
		}	
	}

	public float GetBalloonValue()
	{
		return balloonValue;
	}

	public void SetBalloonValue(float value)
	{
		balloonValue = value > 1 ? 1 : value;
	}
	
	void AnimInflate()
	{
		if(canInflate)
			SetBalloonValue(balloonValue + 0.01f);
	}
		
}