using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalText : MonoBehaviour
{
	public string index;
	
	private void Start() 
	{
		this.GetComponent<Text>().text = LanguageControl.Instance.GetLocalizedString(index);
	}
}
