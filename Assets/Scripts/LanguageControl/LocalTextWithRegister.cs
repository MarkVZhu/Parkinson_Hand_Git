using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalTextWithRegister : MonoBehaviour
{
	public string index;
	
	private void Start() 
	{
		LanguageChange.Instance.OnLanguageChange += changeText;
		this.GetComponent<Text>().text = LanguageControl.Instance.GetLocalizedString(index);
	}
	
	public void changeText()
	{
		this.GetComponent<Text>().text = LanguageControl.Instance.GetLocalizedString(index);
	}
}
