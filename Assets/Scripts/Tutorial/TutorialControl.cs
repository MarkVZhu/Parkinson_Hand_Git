using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
	GameObject Panel;
	
	private void Awake() 
	{
		Panel = transform.GetChild(1).gameObject;
	}
	
	public void startPrompt()
	{
		transform.GetChild(0).gameObject.SetActive(false);
		Panel.SetActive(true);
		Invoke("ActivePanel",3f);
	}
	
	private void ActivePanel()
	{
		Panel.transform.GetChild(0).gameObject.SetActive(false);
		Panel.transform.GetChild(1).gameObject.SetActive(true);
	}
}
