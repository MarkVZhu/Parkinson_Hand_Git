using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalTimeLanguage : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		if(FunctionNum.Instance)
		{
			this.GetComponent<Text>().text = string.Format(LanguageControl.Instance.GetLocalizedString("TotalTime"), FunctionNum.Instance.time.ToString());
		}
	}
}
