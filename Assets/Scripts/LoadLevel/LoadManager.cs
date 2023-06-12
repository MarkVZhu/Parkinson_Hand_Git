using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
	public GameObject motionCanvas;
	public GameObject modeCanvas;
	public GameObject setCanvas;
	public GameObject setCanvasE;
	public GameObject stopCanvas;
	public GameObject instructionCanvas;
	public GameObject rankCanvas;
	public GameObject soundIcon;
	public GameObject languageCanvas;
	int motionNumber = 0; // 1 : Flip; 2 : Fist; 3 : Finger Tapping
	
	
	public void FlipOpt()
	{
		motionNumber = 1;
		ForwardCanvas();
	}
	
	public void FistOpt()
	{
		motionNumber = 2;
		ForwardCanvas();
	}
	
	public void TapOpt()
	{
		motionNumber = 3;
		modeCanvas.transform.GetChild(1).gameObject.SetActive(false);
		ForwardCanvas();
	}
	
	public void LoadTo(bool isFuncMode)
	{
		AudioManager.Instance.PlayAudio("Click");
		switch(motionNumber)
		{
			case 1: 
				//TODO:加上翻掌关卡
				print("To be added");
				break;
			case 2:
				GameObject.Find("PythonManager").GetComponent<OpenExe>().Open();
				AudioManager.Instance.StopAudio("BGM0");
				AudioManager.Instance.PlayAudio("balloonLevel");
				if(isFuncMode) SceneManager.LoadScene("FunctionModeScene");
				else SceneManager.LoadScene("EntertainmentModeScene");
				break;
			case 3:
				GameObject.Find("PythonManager").GetComponent<OpenExe>().Open();
				AudioManager.Instance.StopAudio("BGM0");
				AudioManager.Instance.PlayAudio("buddhismLevel");
				if(isFuncMode) SceneManager.LoadScene("FunctionModeScene_FT");
				//else SceneManager.LoadScene("EntertainmentModeScene_FT");
				break;
			default:
				Debug.Log("No Such Mode");
				break;
		}
	}
	
	private void ForwardCanvas()
	{
		AudioManager.Instance.PlayAudio("Click");
		motionCanvas.SetActive(false);
		modeCanvas.SetActive(true);
	}
	
	public void ReturnCanvas()
	{
		AudioManager.Instance.PlayAudio("Click");
		motionCanvas.SetActive(true);
		modeCanvas.SetActive(false);
		modeCanvas.transform.GetChild(1).gameObject.SetActive(true);
	}
	
	public void SettingCanvas()
	{
		AudioManager.Instance.PlayAudio("Click");
		setCanvas.SetActive(true);
		modeCanvas.SetActive(false);
		soundIcon.SetActive(false);
	}
	
	public void ReturnModeCanvas()
	{
		AudioManager.Instance.PlayAudio("Click");
		setCanvas.SetActive(false);
		modeCanvas.SetActive(true);
		soundIcon.SetActive(true);
	}
	
	public void SettingCanvasE()
	{
		AudioManager.Instance.PlayAudio("Click");
		setCanvasE.SetActive(true);
		modeCanvas.SetActive(false);
		soundIcon.SetActive(false);
	}
	
	public void ReturnModeCanvasE()
	{
		AudioManager.Instance.PlayAudio("Click");
		setCanvasE.SetActive(false);
		modeCanvas.SetActive(true);
		soundIcon.SetActive(true);
	}
	
	public void InstructionCanvas(bool isFuncMode)
	{
		AudioManager.Instance.PlayAudio("Click");
		
		
		if(isFuncMode)
		{
			switch(motionNumber)
			{
				//TODO: case 1 to be added
				case 2: instructionCanvas.transform.GetChild(1).GetComponent<LocalTextWithRegister>().index = "Instruction_FF"; break;
				case 3: instructionCanvas.transform.GetChild(1).GetComponent<LocalTextWithRegister>().index = "Instruction_TF"; break;
				default: Debug.LogWarning("no such level"); break;
			}
		}
		else
		{
			switch(motionNumber)
			{
				//TODO: case 1 & case 3 to be added
				case 2: instructionCanvas.transform.GetChild(1).GetComponent<LocalTextWithRegister>().index = "Instruction_FE"; break;
				default: Debug.LogWarning("no such level"); break;
			}
		}
		instructionCanvas.transform.GetChild(1).GetComponent<LocalTextWithRegister>().changeText();
		
		instructionCanvas.SetActive(true);
		modeCanvas.SetActive(false);
		soundIcon.SetActive(false);
	}
	
	public void ReturnInstruction()
	{
		AudioManager.Instance.PlayAudio("Click");
		instructionCanvas.SetActive(false);
		modeCanvas.SetActive(true);
		soundIcon.SetActive(true);
	}
	
	public void LoadMainMenu()
	{
		AudioManager.Instance.PlayAudio("Click");
		AudioManager.Instance.StopAudio("buddhismLevel");
		AudioManager.Instance.StopAudio("balloonLevel");
		AudioManager.Instance.PlayAudio("BGM0");
		
		if(SceneManager.GetActiveScene().buildIndex == 2)
		{
			UDPReceive.Instance.client.Close();
			UDPReceive.Instance.startReceiving = false;
			Time.timeScale = 1;
		}	
		SceneManager.LoadScene(0);
	}
	
	public void LoadThisScene()
	{
		AudioManager.Instance.PlayAudio("Click");
		if(SceneManager.GetActiveScene().buildIndex == 2)
		{
			UDPReceive.Instance.client.Close();
			UDPReceive.Instance.startReceiving = false;
			Time.timeScale = 1;
		}	

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	public void QuitGame()
	{
		PlayerMessageManager.Instance.SavePlayerData();
		Application.Quit();
	}
	
	public void StopGame()
	{
		AudioManager.Instance.PlayAudio("Click");
		stopCanvas.SetActive(true);
		//UDPReceive.Instance.startReceiving = false; //FIXME:暂停回来不能接受
		Time.timeScale = 0;
	}
	
	public void ContinueGame()
	{
		AudioManager.Instance.PlayAudio("Click");
		Time.timeScale = 1;
		UDPReceive.Instance.startReceiving = true;
		GameObject.Find("StopCanvas").SetActive(false);
	}
	
	public void OpenRankCanvas()
	{
		AudioManager.Instance.PlayAudio("Click");
		switch(motionNumber)
		{
			//TODO: case 1 to be added
			case 2: 
				PlayerMessageManager.Instance.currentRankList = PlayerMessageManager.Instance.playerMessageListF;
				modeCanvas.SetActive(false);
				languageCanvas.SetActive(false);
				rankCanvas.SetActive(true);
				break; 

			case 3: 
				PlayerMessageManager.Instance.currentRankList = PlayerMessageManager.Instance.playerMessageListT;
				modeCanvas.SetActive(false);
				languageCanvas.SetActive(false);
				rankCanvas.SetActive(true);	
				break; 
			default: Debug.LogWarning("no such level"); break;
		}		
	}
	
	public void CloseRankCanvas()
	{
		AudioManager.Instance.PlayAudio("Click");
		modeCanvas.SetActive(true);
		languageCanvas.SetActive(true);
		rankCanvas.SetActive(false);
	}
	
	public void PlayClickAudio()
	{
		AudioManager.Instance.PlayAudio("Click");
	}
}
