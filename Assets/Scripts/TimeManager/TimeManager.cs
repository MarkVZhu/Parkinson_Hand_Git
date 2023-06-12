using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeManager : MonoBehaviour
{
	public static TimeManager instance;

	Timer generalTimer;
	public Text totalTimeText;
	public bool timerCanRun = true;

	private void Awake()
	{
		generalTimer = new Timer("GeneralTime");
		instance = this;
	}

	private void OnEnable()
	{
		generalTimer.StartRecording();
	}

	private void Update()
	{
		if (timerCanRun)
		{
			totalTimeText.text = Timer.ToTimeFormat(generalTimer.GetCurrentRecord());
		}
	}

	public Timer GetTimer()
	{
		return generalTimer;
	}

	public void EndPointProcessOnUIText()
	{
		timerCanRun = false;
	}
}
