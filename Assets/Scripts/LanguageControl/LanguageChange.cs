using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageChange : Singleton<LanguageChange>
{
	public Action OnLanguageChange; 
 	
	private void Start() 
	{
		//TODO:add more cases if there are new languages
		switch(LanguageControl.Instance.GetCurrentLanguage())
		{
			case "简体中文":
			GetComponent<Dropdown>().value = 0;
			break;
			case "English":
			GetComponent<Dropdown>().value = 1;
			break;
		}
	}
	
	public void LanguageSwitch()
	{
		string targetLanguage = this.transform.GetChild(0).GetComponent<Text>().text;	
		LanguageControl.Instance.ChangeLanguage(targetLanguage);
		OnLanguageChange?.Invoke();
	}
}
