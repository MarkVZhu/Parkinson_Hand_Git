using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class DataSaveManager : MonoBehaviour
{
	public static DataSaveManager instance;
	public Action OnInflate;
	public Action WriteData;
	
	public HandAnimaControl handAnima;
	[SerializeField] private int callSaveNumber;
	public BalloonAnimaControl balloonAnima;

	public float downThreshold; // defult: 0.2
	private float upThreshold;
	private bool enterDownArea;
	private bool enterUpArea;
	private Save saveTemp;

	private bool canRecordDown;
	private bool canRecordUp;
	public bool canRecord;

	XmlDocument xmlDocument;
	XmlElement root;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		callSaveNumber = 1;
		upThreshold = 1 - downThreshold;

		xmlDocument = new XmlDocument();
		root = xmlDocument.CreateElement("Save"); // Var name: root  ; Tag Name: Save
		root.SetAttribute("FileName", "Player_Data");
		xmlDocument.AppendChild(root); // Add the root to the XML document 

		saveTemp = CreateSaveGameObject(callSaveNumber);
		canRecordDown = true;
	}

	public Save CreateSaveGameObject(int num)
	{
		Save save = new Save();
		save.number = num;
		save.time = TimeManager.instance.GetTimer().GetCurrentRecord();
		save.extent = handAnima.GetHandValue();

		return save;
	}

	public void WriteXML(Save save)
	{
		WriteData?.Invoke();
		//Save save = CreateSaveGameObject(callSaveNumber);

		#region Create XML Elements
		XmlElement itemElement = xmlDocument.CreateElement("Item");
		root.AppendChild(itemElement);

		XmlElement numberElement = xmlDocument.CreateElement("Number");
		numberElement.InnerText = save.number.ToString();
		itemElement.AppendChild(numberElement);

		XmlElement timeElement = xmlDocument.CreateElement("Time");
		timeElement.InnerText = Timer.ToTimeFormat(save.time);
		itemElement.AppendChild(timeElement);

		XmlElement extentElement = xmlDocument.CreateElement("Extent");
		extentElement.InnerText = (save.extent * 100).ToString() + "%";
		itemElement.AppendChild(extentElement);
		#endregion

		callSaveNumber++;
		saveTemp = CreateSaveGameObject(callSaveNumber);
	}

	public void SaveByXML()
	{
		string directoryPath = Application.dataPath + "/PlayerData/";
		string filePath = directoryPath;
		if(SceneManager.GetActiveScene().buildIndex == 1)
		{
			filePath += "Fist_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xml";
		}
		else if(SceneManager.GetActiveScene().buildIndex == 3)
		{
			filePath += "Tap_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xml";
		}

		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}

		if (File.Exists(filePath))
		{
			Debug.LogWarning("File already exists. Overwriting...");
		}

		xmlDocument.Save(filePath);
		Debug.Log("XML FILE SAVED");
	}

	private void Update()
	{
		judgeArea();
		if (enterDownArea)
		{
			recordExtremeSaveData(saveTemp, true);
		}
		if (enterUpArea)
		{
			recordExtremeSaveData(saveTemp, false);
		}
	}

	private void recordExtremeSaveData(Save save, bool isInDown)
	{
		if (isInDown)
		{
			if(handAnima.GetHandValue() < save.extent)
			{
				save.extent = handAnima.GetHandValue();
				save.time = TimeManager.instance.GetTimer().GetCurrentRecord();
			}
		}
		else
		{
			if (handAnima.GetHandValue() > save.extent)
			{
				save.extent = handAnima.GetHandValue();
				save.time = TimeManager.instance.GetTimer().GetCurrentRecord();
			}
		}
	}
	
	//Changing GetHandValue into GetHandAnimeNormalize to avoid handValue's variation from upThreshold into downThreshold in a flash
	private void judgeArea() 
	{
		if (handAnima.GetHandAnimeNormalize() < downThreshold && enterDownArea == false) // Starting entering down area
		{
			enterDownArea = true;
			canRecordUp = true; //FIXED:Sometimes enterDownArea -> true  but canRecordUp not-> true
		}
		else if (handAnima.GetHandAnimeNormalize() > downThreshold && enterDownArea == true) // leaving down area
		{
			enterDownArea = false;
			if (canRecordDown && canRecord)
			{
				WriteXML(saveTemp);
				canRecordDown = false;
			} 
		}

		if (handAnima.GetHandAnimeNormalize() > upThreshold && enterUpArea == false) // Starting entering up area 
		{
			enterUpArea = true;
			canRecordDown = true;
			//Debug.Log(canRecordUp);
			if(canRecordUp && canRecord)
			{
				//Debug.Log("Invoke");
				OnInflate?.Invoke();
				// if(balloonAnima != null)
				// {
				// 	balloonAnima.SetBalloonValue(balloonAnima.GetBalloonValue() + 0.02f);
				// }
			}
		}
		else if (handAnima.GetHandAnimeNormalize() < upThreshold && enterUpArea == true) // leaving up area 
		{
			enterUpArea = false;
			if (canRecordUp && canRecord)
			{
				WriteXML(saveTemp);
				canRecordUp = false;
				//Debug.Log("canRecordUp change false");
			}
		}
	}
	
	public Save GetSaveTemp()
	{
		return saveTemp;
	}
}
