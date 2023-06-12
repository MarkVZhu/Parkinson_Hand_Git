using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMessageManager : Singleton<PlayerMessageManager>
{
	//public int capacity;
	//List大小代表可以存储的列表前N位数据
	public List<PlayerMessage> playerMessageListF;
	public List<PlayerMessage> playerMessageListT;
	private string playerDataPathF;
	private string playerDataPathT;
	public List<PlayerMessage> currentRankList;
	//public RowManager _rowManager;
	//public RectTransform Content;

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this);
		
		playerMessageListF = new List<PlayerMessage>();
		playerMessageListT = new List<PlayerMessage>();
		playerDataPathF = Application.persistentDataPath + "/PlayerDataF.json";
		playerDataPathT = Application.persistentDataPath + "/PlayerDataT.json";
		Debug.Log(playerDataPathF);
		//Debug.Log(Application.persistentDataPath);
	}

	void Start()
	{
		//SavePlayerData();
		//Load data to the Rank
		string data = File.ReadAllText(playerDataPathF);
		playerMessageListF = SerializeList.ListFromJson<PlayerMessage>(data);
		data = File.ReadAllText(playerDataPathT);
		playerMessageListT = SerializeList.ListFromJson<PlayerMessage>(data);
		
	}

	// public void AddMessage(int number, float time, float avFrequency, float avRange, int score)
	// {
	// 	playerMessageListF.Add(new PlayerMessage(number,time,avFrequency,avRange,score));
	// 	playerMessageListF.Sort((x,y)=> { return -x.score.CompareTo(y.score); });
	// }

	public void AddMessage(PlayerMessage newPlayerMessage, List<PlayerMessage> list)
	{
		list.Add(newPlayerMessage);
		list.Sort((x,y)=> { return -x.score.CompareTo(y.score); });
	}
	
	public void SavePlayerData()
	{
		SaveData(playerMessageListF,playerDataPathF);
		SaveData(playerMessageListT,playerDataPathT);
	}
	
	private void SaveData(List<PlayerMessage> playerMessages, string path)
	{
		if (!File.Exists(path))
		{
			File.Create(path);
		}
		string json = SerializeList.ListToJson(playerMessages);
		File.WriteAllText(path, json);
		Debug.Log("保存成功");
	}

	private void updateLength()
	{
		
	}
}

[System.Serializable]
public class PlayerMessage
{
	public int number;
	public float time;
	public float frequency;
	public float avFrequency;
	private float extent;
	public float range;
	public float avRange;
	public int score;

	/// <summary>
	/// 创建一个记录玩家数据信息的条目
	/// </summary>
	/// <param name="number"> 玩家完成动作的次数 </param>
	/// <param name="time"> 玩家完成动作的时间 </param>
	/// <param name="avFrequency"> 玩家完成动作的平均频率</param>
	/// <param name="avRange"> 玩家完成动作的平均幅度 </param>
	/// <param name="score"> 玩家完成动作的总得分 </param>
	public PlayerMessage(int number, float time, float avFrequency, float avRange, int score)
	{
		this.number = number;
		this.time = time;
		this.avFrequency = avFrequency;
		this.avRange = avRange;
		this.score = score;
	}
	
	public PlayerMessage()
	{
	}
}