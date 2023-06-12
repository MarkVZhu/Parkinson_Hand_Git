using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class ERuleManager : MonoBehaviour
{
	[Header("One Round Setting")]
	public int OneRoundInflate;
	private int OneRoundITemp;
	[Header("UI Component")]
	public Text OneRoundText;
	public Text PromptText;
	public PlayableDirector director;
	
	private bool canInvokeSwitch;
	private bool canInvokeOut;
	private bool canInvokeReady;
	private bool canInvokeStart;
	
	private int playerNum;
	
	[Header("Counts")]
	[SerializeField]private int totalTimes;
	[SerializeField]private int count;
	
	[Header("Others")]
	public GameObject stopCanvas;
	public GameObject endCanvas;
	public GameObject balloon;
	public Material normalRed;
	public Material edgeGlow;
	private float promptOffset;
	[SerializeField]private handState currentState;
	[Range(2,6)]public int totalPlayers = 4;
	[Tooltip("How many rounds you want when all players complete their movements?")]public int roundQuantity = 3;
	[SerializeField]private List<int> outPlayers;
	
	public GripValue grip;
	
	private enum handState {ReadyPhase, StartPhase, EndPhase, SwitchPhase, OutPhase, OverPhase};
	
	// Start is called before the first frame update
	void Start()
	{
		OneRoundInflate = 5;
		OneRoundITemp = OneRoundInflate;
		OneRoundText.text = OneRoundITemp.ToString();
		
		if(FunctionNum.Instance) totalPlayers = FunctionNum.Instance.playersNum;
		
		currentState = handState.ReadyPhase;
		playerNum = 1;
		
		promptOffset = Random.Range(-0.1f, 0.1f);
		
		totalTimes = RadomTotalTimes();
		count = 0;
		DataSaveManager.instance.OnInflate += AddOneInflate;
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			stopCanvas.SetActive(true);
			//UDPReceive.Instance.startReceiving = false; 
			
			Time.timeScale = 0;
		}
		
		switch(currentState)
		{
			case handState.ReadyPhase:
				canInvokeSwitch = true;
				canInvokeOut = true;
				canInvokeReady = true;
				canInvokeStart = true;
				
				if(canInvokeReady)
				{
					canInvokeReady = false;
					OneRoundText.text = "";
					PromptText.text = string.Format(LanguageControl.Instance.GetLocalizedString("Ready"), playerNum.ToString());		
				}
				
				if(grip.JudgeIsReady() && Time.timeScale == 1)
				{
					Invoke("SwitchToStart", 0.3f);
				}				
				break;
			case handState.StartPhase:
				if(canInvokeStart)
				{
					canInvokeStart = false;
					PromptText.text = LanguageControl.Instance.GetLocalizedString("Remain_Inflate");
				}
				OneRoundText.text = OneRoundITemp.ToString();
				break;
			case handState.EndPhase:
				if((grip.JudgeIsReady() || OneRoundITemp == 0) && Time.timeScale == 1) currentState = handState.SwitchPhase;
				OneRoundText.text = OneRoundITemp.ToString();
				break;
			case handState.SwitchPhase:
				if(canInvokeSwitch) //once
				{
					canInvokeSwitch = false;
					
					balloon.GetComponent<BalloonAnimaControl>().canInflate = false;
					PromptText.text = LanguageControl.Instance.GetLocalizedString("Next");
					playerNum += 1;
					
					if(outPlayers.Contains(playerNum)) playerNum += 1;				
					if(playerNum == (totalPlayers + 1)) playerNum -= totalPlayers; 
					
					OneRoundText.text = playerNum.ToString();
					director.Play();
					
					OneRoundITemp = OneRoundInflate;
					Invoke("SwitchCountDown", 3f);
				}	
				break;
			case handState.OutPhase:
				if(canInvokeOut)
				{
					canInvokeOut = false;
					PromptText.text = string.Format(LanguageControl.Instance.GetLocalizedString("Elimination"), playerNum.ToString());
					OneRoundText.text = "";
					count = 0;
					
					if(outPlayers.Count == (totalPlayers - 1))
					{
						for(int i = 1; i <= totalPlayers; i++)
						{
							playerNum = i;
							if(!outPlayers.Contains(i)) break;
						}
						Invoke("SwitchToOver", 2f);
					}
					else
					{
						Invoke("SwitchNewRound", 2f);
					}
					
					
				}
				break;			
			case handState.OverPhase:
				PromptText.text = string.Format(LanguageControl.Instance.GetLocalizedString("Game_Over"), playerNum.ToString());
				OneRoundText.text = "";
				if(endCanvas) endCanvas.SetActive(true);
				break;
		}

	}
	
	int RadomTotalTimes()
	{
		int baseNum = (totalPlayers - outPlayers.Count) * OneRoundInflate * roundQuantity;
		int reNum = (int)Random.Range(baseNum - 10, baseNum + 10);
		if(reNum < 10) reNum = (int)Random.Range(8, 20);
		return reNum; 
	}
	
	void AddOneInflate()
	{
		if((currentState == handState.StartPhase || currentState == handState.EndPhase) && grip.JudgeThumbClose())
		{
			AudioManager.Instance.PlayAudio("Inflate" + (int)Random.Range(1,6));
			Debug.Log("Inflate Add One");
			count++;
			OneRoundITemp--;
			Debug.Log(OneRoundITemp);
			currentState = handState.EndPhase;
			//OneRoundText.text = OneRoundITemp.ToString();
		}
		Debug.Log((float)count/totalTimes + "||||" + (0.7f + promptOffset) + ((float)count/totalTimes >= (0.7f + promptOffset)));
		if((float)count/totalTimes >= (0.7f))
		{
			Material[] targetMaterials = new Material[2];
			targetMaterials[0] = normalRed;
			targetMaterials[1] = edgeGlow;	
			balloon.transform.GetChild(0).GetComponent<MeshRenderer>().materials = targetMaterials;
		}
		if(count == totalTimes)
		{
			
			Debug.Log("Explode！！！Eliminate：Player" + playerNum);
			AudioManager.Instance.PlayAudio("Explosion");
			balloon.SetActive(false);
			balloon.GetComponent<BalloonAnimaControl>().balloonValue = 0;
			balloon.transform.GetChild(0).GetComponent<MeshRenderer>().materials[1] = null;
			
			outPlayers.Add(playerNum);
			currentState = handState.OutPhase;
			totalTimes = RadomTotalTimes();
		}
		
	}
	
	void SwitchToStart()
	{
		currentState = handState.StartPhase;
		balloon.GetComponent<BalloonAnimaControl>().canInflate = true;
	}
	
	void SwitchCountDown()
	{
		currentState = handState.ReadyPhase;
	}
	
	void SwitchNewRound()
	{
		balloon.SetActive(true);
		currentState = handState.SwitchPhase;
	}
	
	void SwitchToOver()
	{
		currentState = handState.OverPhase;
	}
	
	public int GetPlayerNum()
	{
		return playerNum;
	}
}
