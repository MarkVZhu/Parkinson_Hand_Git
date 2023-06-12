using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordRoundData : Singleton<RecordRoundData>
{
	public int number;
	private float time;
	public float frequency;
	public float avFrequency;
	private float extent;
	public float range;
	public float avRange;
	
	private void Start() 
	{
		DataSaveManager.instance.WriteData += ReflectInflate;	
	}
	
	public void ReflectInflate()
	{
		Save currentSave = DataSaveManager.instance.GetSaveTemp();
		
		number = currentSave.number;
		range = Mathf.Abs(currentSave.extent - extent);
		extent = currentSave.extent;
		if(number >= 2) avRange = (avRange * (number - 2) + range) / (number - 1);
		
		if(currentSave.number > 1 && (currentSave.number - 1) % 2 == 0)
			{
				frequency = 1/(currentSave.time - time);
				time = currentSave.time;
				avFrequency = number / (2 * time);
			}
			
		Debug.Log(string.Format("num : {4} ; freq : {0} ; av freq : {1} ; range : {2} ; av range : {3} ", frequency, avFrequency, range, avRange, number));
	}
	
	public void UpdateFinalAvFrequency()
	{
		avFrequency = avFrequency = number / (2 * FunctionNum.Instance.time);
	}
}
