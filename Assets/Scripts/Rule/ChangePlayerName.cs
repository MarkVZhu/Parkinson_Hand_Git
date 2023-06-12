using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerName : MonoBehaviour
{
	public ERuleManager eRuleManager;
	
	private void Start() {
		TextChange();
	}
	
	public void TextChange()
	{
		//FIXME:仅在交换玩家时调用
		this.GetComponent<Text>().text = string.Format(LanguageControl.Instance.GetLocalizedString("Player"), eRuleManager.GetPlayerNum().ToString());
	}
}
