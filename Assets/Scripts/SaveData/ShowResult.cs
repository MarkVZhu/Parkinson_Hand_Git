using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowResult : MonoBehaviour
{
	public GameObject TutorialText;
	
	private void OnEnable() 
	{
		RecordRoundData.Instance.UpdateFinalAvFrequency();
		
		if(TutorialText && FunctionNum.Instance.Tutorial)
		{
			TutorialText.SetActive(true);
			FunctionNum.Instance.Tutorial = false;
		}
		
		transform.GetChild(0).GetComponent<Text>().text = string.Format(LanguageControl.Instance.GetLocalizedString("Count_Time"), 
		(int)((RecordRoundData.Instance.number + 1)/2));
		transform.GetChild(1).GetComponent<Text>().text = string.Format(LanguageControl.Instance.GetLocalizedString("TotalTime"), 
		FunctionNum.Instance.time.ToString());
		transform.GetChild(2).GetComponent<Text>().text = string.Format(LanguageControl.Instance.GetLocalizedString("Frequency"), 
		RecordRoundData.Instance.avFrequency);
		transform.GetChild(3).GetComponent<Text>().text = string.Format(LanguageControl.Instance.GetLocalizedString("Average_Amplitude"), 
		RecordRoundData.Instance.avRange);
		
		//TODO:综合评分待修改
		float grade = RecordRoundData.Instance.avFrequency * 100 / 1.5f;
		grade += RecordRoundData.Instance.avRange * 100;
		grade /= 2;
		grade = grade > 100 ? 100 : grade;
		
		transform.GetChild(4).GetComponent<Text>().text = string.Format(LanguageControl.Instance.GetLocalizedString("Grade"), grade);	
		
		Text resultText = transform.parent.GetChild(1).GetComponent<Text>();
		if(grade > 90)
		{
			resultText.text = LanguageControl.Instance.GetLocalizedString("Degree_Excellent");
		}
		else if(grade > 70)
		{
			resultText.text = LanguageControl.Instance.GetLocalizedString("Degree_Good");
		}
		else
		{
			resultText.text = LanguageControl.Instance.GetLocalizedString("Degree_Normal");
		}
		
		//Write data into rank list
		PlayerMessage newMessage = new PlayerMessage((int)((RecordRoundData.Instance.number + 1)/2),
													FunctionNum.Instance.time,
													RecordRoundData.Instance.avFrequency,
													RecordRoundData.Instance.avRange,
													(int)grade);
		
		//TODO:加关卡增加List											
		if(SceneManager.GetActiveScene().buildIndex == 1)
		PlayerMessageManager.Instance.AddMessage(newMessage,PlayerMessageManager.Instance.playerMessageListF);
		else if(SceneManager.GetActiveScene().buildIndex == 3)
		PlayerMessageManager.Instance.AddMessage(newMessage,PlayerMessageManager.Instance.playerMessageListT);
	}
}
