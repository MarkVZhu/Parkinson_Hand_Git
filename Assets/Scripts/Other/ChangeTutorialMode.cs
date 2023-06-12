using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTutorialMode : MonoBehaviour
{
	public GameObject openLabel;
	public GameObject closeLabel;
	private bool canChange;
	
	private void Start() 
	{
		canChange = false;
		GetComponent<Toggle>().isOn = FunctionNum.Instance.Tutorial;
		if(FunctionNum.Instance.Tutorial)
		{
			openLabel.SetActive(true);
			closeLabel.SetActive(false);
		}
		else
		{
			openLabel.SetActive(false);
			closeLabel.SetActive(true);
		}
		canChange = true;
	}
	
	public void ChangeLabel()
	{
		if(canChange)
		{
			if(FunctionNum.Instance.Tutorial)
			{
				openLabel.SetActive(false);
				closeLabel.SetActive(true);
				FunctionNum.Instance.Tutorial = false;
			}
			else
			{
				openLabel.SetActive(true);
				closeLabel.SetActive(false);
				FunctionNum.Instance.Tutorial = true;
			}
		}
	}
}
