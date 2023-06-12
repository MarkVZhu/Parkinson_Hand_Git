using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
	public bool canChange;
	
	void Start() 
	{
		canChange = true;
		DataSaveManager.instance.OnInflate += LightIntense;	
	}
	
	void LightIntense()
	{
		if(canChange)
			this.GetComponent<Light>().intensity += this.GetComponent<Light>().intensity > 1500 ? 250 : 125;
	}
}
