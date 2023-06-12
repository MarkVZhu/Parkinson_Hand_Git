using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeEnourageText : MonoBehaviour
{
	public float textKeepTime = 2f;
	private float textKeepTimeTimeTemp;
	private bool canChange = true;
	
	// Start is called before the first frame update
	void Start()
	{
		DataSaveManager.instance.WriteData += changeeText;
	}

	public void changeeText()
	{
		if(canChange)
		{
			canChange = false;
			textKeepTimeTimeTemp = textKeepTime;
			
			float grade = RecordRoundData.Instance.frequency * 100 / 1.5f;
			grade += RecordRoundData.Instance.range * 100;
			grade /= 2;
			grade = grade > 100 ? 100 : grade;
			
			Debug.Log("Grade: " + grade);
			
			Text eText = GetComponent<Text>();
			if(grade > 90)
			{
				eText.text = LanguageControl.Instance.GetLocalizedString("Perfect_" + Random.Range(1,3));
				GetComponent<Animation>()["EncourageAnimation"].speed = 1.2f;
				AudioManager.Instance.PlayAudio("best");
			}
			else if(grade > 70)
			{
				eText.text = LanguageControl.Instance.GetLocalizedString("Good_" + Random.Range(1,3));
				GetComponent<Animation>()["EncourageAnimation"].speed = 1f;
				AudioManager.Instance.PlayAudio("good");
			}
			else
			{
				eText.text = LanguageControl.Instance.GetLocalizedString("Ok_" + Random.Range(1,3));
				GetComponent<Animation>()["EncourageAnimation"].speed = 0.8f;
				AudioManager.Instance.PlayAudio("ok");
			}
			
		}
	}
	
	private void Update() 
	{
		textKeepTimeTimeTemp -= Time.deltaTime;
		if(textKeepTimeTimeTemp < 0.01f) canChange = true; 
	}
}
