using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleManager : MonoBehaviour
{
	[Header("Start Setting")]
	public float startTime = 3f;
	public GameObject startCanvas;
	public GripValue gripValue;
	public TimeManager timeManager;
	private bool isInStartState;
	private float startCountDown;
	
	[Header("End Setting / sec")]
	public float oneRoundTime;
	private bool isEnd;
	public GameObject tutorialCanvas;
	public GameObject resultCanvas;
	public GameObject encourageText;
	public GameObject model;
	
	[Header("Count Setting")]
	public Text CountText;
	private int countNum; 
	private bool canCount;
	
	//[Space]
	//public DataSaveManager dataSaveManager;

	
	void Start() 
	{
		isInStartState = true;
		startCountDown = startTime;
		
		if(FunctionNum.Instance) oneRoundTime = FunctionNum.Instance.time;
		
		canCount = false;
		countNum = 0;
		DataSaveManager.instance.OnInflate += CountInflate;
		
		if(FunctionNum.Instance.Tutorial)
		{
			tutorialCanvas.SetActive(true);
		}
	}
	
	void Update() 
	{
		if(isInStartState) DetectStart();
		if(!isEnd) DetectEnd();
	}
	
	private void DetectStart()
	{
		if(gripValue.GetGripExtent() < 0.15f) //The extent threshold for game start
		{
			if(startCountDown > 0.01f)
			{
				startCountDown -= Time.deltaTime;
				startCanvas.transform.GetChild(0).GetComponent<Text>().text = ((int)(startCountDown + 0.99f)).ToString();
				//startTimeText.text = startCountDown.ToString("0");
				//Debug.Log(startCountDown.ToString() + "---" + ((int)(startCountDown + 0.99f)).ToString());
			}
			else
			{
				isInStartState = false;
				timeManager.enabled = true;
				startCanvas.transform.GetChild(0).GetComponent<Text>().text = "";
				
				startCanvas.transform.GetChild(1).GetComponent<LocalText>().index = "Start";
				startCanvas.transform.GetChild(1).GetComponent<Text>().text = LanguageControl.Instance.GetLocalizedString("Start");
				//startCanvas.transform.GetChild(2).GetComponent<Text>().text = "Start!";
				
				if(tutorialCanvas && FunctionNum.Instance.Tutorial)
				{
					tutorialCanvas.GetComponent<TutorialControl>().startPrompt();
				}
				
				canCount = true;
				DataSaveManager.instance.canRecord = true;
				Invoke("DisableStartCanvas",2f);
			}
		}
		else
		{
			startCountDown = startTime;
			startCanvas.transform.GetChild(0).GetComponent<Text>().text = ((int)(startCountDown + 0.99f)).ToString();
		}
	}
	
	private void DetectEnd()
	{
		if(TimeManager.instance.GetTimer().GetCurrentRecord() >= oneRoundTime)
		{
			TimeManager.instance.EndPointProcessOnUIText();
			TimeManager.instance.totalTimeText.text = Timer.ToTimeFormat(oneRoundTime);
			TimeManager.instance.GetTimer().PauseRecording();
			EnableStartCanvasWhenEnd();
			Debug.Log("游戏结束");
			//Debug.Log("Game End!");
			

			if(model != null)
			{
				model.SetActive(false);
				AudioManager.Instance.PlayAudio("Explosion");
			}
			
			isEnd = true;
			
			if(tutorialCanvas) tutorialCanvas.SetActive(false);
			
			//Close UDP Receiver
			if(!gripValue.ModifyMode)
			{
				UDPReceive.Instance.client.Close();
				UDPReceive.Instance.startReceiving = false;
			}
			
			//Save Data for Every Game Round
			//TODO:Enable next line in formal play
			DataSaveManager.instance.SaveByXML();
			
			Invoke("ShowResultCanvas",1f);
		}
	}
	
	private void DisableStartCanvas()
	{
		startCanvas.transform.GetChild(1).GetComponent<LocalText>().index = "Keep_Open";
		startCanvas.SetActive(false);
	}	
	
	private void EnableStartCanvasWhenEnd()
	{
		startCanvas.SetActive(true);
		startCanvas.transform.GetChild(1).GetComponent<LocalText>().index = "End";
		startCanvas.transform.GetChild(1).GetComponent<Text>().text = LanguageControl.Instance.GetLocalizedString("End");
		canCount = false;
		DataSaveManager.instance.canRecord = false;
	}
	
	void CountInflate()
	{
		if(canCount)
		{
			AudioManager.Instance.PlayAudio("Inflate" + (int)Random.Range(1,6));
			countNum++;
			CountText.text = countNum.ToString();
		}
	}
	
	public void ShowResultCanvas()
	{
		AudioManager.Instance.PlayAudio("result");
		if(resultCanvas) resultCanvas.SetActive(true);
		if(encourageText) encourageText.SetActive(false);
	}
}
