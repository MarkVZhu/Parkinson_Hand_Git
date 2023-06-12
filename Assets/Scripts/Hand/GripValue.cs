using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripValue : MonoBehaviour
{
	[Header("Points for judging grip extent")]
	public Transform middleTip;
	public Transform wrist;
	public Transform indexMcp;
	
	[Header("Points for judging ready state")]
	public Transform thumbTip;
	public Transform indexTip;
	public Transform pinkyTip;
	public Transform middlePip;
	private float readyValue;
	private float verticalValue1;
	private float verticalValue2;
	
	[Space]
	public bool ModifyMode;
	public bool EntertainmentMode;
	public bool isTapLevel;

	private float offset;

	[SerializeField] [Range(0,1f)]float gripExtent;
	float compareLength;

	// Start is called before the first frame update
	void Awake()
	{
		if (ModifyMode)
		{
			GetComponent<HandTracking>().enabled = false;
			GetComponent<UDPReceive>().enabled = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!ModifyMode)
		{
			if(isTapLevel) CalculateTapExtent();
			else CalculateGripExtent();
		}
		
		GetComponent<HandAnimaControl>().SetHandValue(gripExtent);
	}
	
	public float GetGripExtent()
	{
		return gripExtent;
	}
	
	public void CalculateGripExtent()
	{
		gripExtent = Vector3.Distance(middleTip.position, (wrist.position + middleTip.position) / 2);
		compareLength = Vector3.Distance(wrist.position, indexMcp.position);

		gripExtent = gripExtent / compareLength;
		//Debug.Log(gripExtent);

		//TODO:调试
		offset = 0.37f;
		gripExtent -= offset;
		gripExtent = gripExtent < 0 ? 0 : gripExtent;
		gripExtent = gripExtent > 0.566f ? 0.566f : gripExtent;

		gripExtent = gripExtent / 0.566f;
		gripExtent = 1 - gripExtent;
		//Debug.Log(gripExtent);
	}
	
	public void CalculateTapExtent()
	{
		gripExtent = Vector3.Distance(indexTip.position, thumbTip.position);
		compareLength = Vector3.Distance(wrist.position, indexMcp.position);
		
		gripExtent = gripExtent / compareLength;
		
		offset = 0.164f;
		gripExtent -= offset;
		gripExtent = gripExtent < 0 ? 0 : gripExtent;
		gripExtent = gripExtent > 0.9f ? 0.9f : gripExtent;
		
		gripExtent = gripExtent / 0.9f;
		gripExtent = 1 - gripExtent;
		//Debug.Log(gripExtent);
	}
	
	//Judge whether the player is ready for a new round in entertainment mode
	public bool JudgeIsReady()
	{
		if(thumbTip && indexTip && pinkyTip)
		{
			readyValue = Vector3.Distance(indexTip.position, wrist.position);
			readyValue += Vector3.Distance(middleTip.position, wrist.position);
			readyValue += Vector3.Distance(pinkyTip.position, wrist.position);
			readyValue /= Vector3.Distance(thumbTip.position, wrist.position);
			//Debug.Log(readyValue);
			
			verticalValue1 = Mathf.Abs(Vector3.Dot((thumbTip.position - pinkyTip.position), Vector3.right));
			verticalValue2 = Mathf.Abs(Vector3.Dot((pinkyTip.position - wrist.position), Vector3.up));
			
			//Debug.Log("verticalValue1: " + verticalValue1 + "; verticalValue2: " + verticalValue2);
		}
		
		return (readyValue < 2.0f && verticalValue1 < 0.2f && verticalValue2 < 0.2f) ? true : false;
	}
	
	public bool JudgeThumbClose()
	{
		return Vector3.Distance(thumbTip.position, middlePip.position) < 0.6f ? true : false;		
	}	
}
