using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class LanguageControl : Singleton<LanguageControl>
{
	private string SavePath;
	[SerializeField]private string CurrentLanguage = "简体中文";
	private Transform LanList;
	public Dictionary<string, Language> LanguageLib = new Dictionary<string, Language>();
	public bool OverrideMode; 
	
	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this);
		
		SavePath = Application.persistentDataPath;
		LoadLanguage();
		Debug.LogFormat("Using:{0}, Author:{1}, Version:{2}", GetLocalizedString("Example_Text"), LanguageLib[CurrentLanguage].Author,LanguageLib[CurrentLanguage].Version);
		//Debug.Log(SavePath);
	}

	public void ChangeLanguage(string l)
	{
		if(LanguageLib.ContainsKey(l))	CurrentLanguage = l;
		else CurrentLanguage = "简体中文";
	}
	
	private void LoadLanguage()
	{
		#region Load internal language package
		foreach(var item in Resources.LoadAll("Language"))
		{
			TextAsset t = (TextAsset)item;
			string js = t.text;
			Language lan = JsonConvert.DeserializeObject<Language>(js);
			LanguageLib.Add(lan.LanguageType, lan);
			Debug.Log(LanguageLib[CurrentLanguage].LanguageType);
		}
		#endregion
		
		#region Load external language package
		if(!Directory.Exists(SavePath + "/Language"))
		{
			Directory.CreateDirectory(SavePath + "/Language");
		}
		string[] paths = Directory.GetFiles(SavePath + "/Language"); //Get all files in this directory
		if(paths.Length > 0)
		{
			foreach(var path in paths)
			{
				FileStream file = new FileStream((path), FileMode.Open);
				StreamReader reader = new StreamReader(file);
				string js = reader.ReadToEnd();
				Language lan = JsonConvert.DeserializeObject<Language>(js);
				if(LanguageLib.ContainsKey(lan.LanguageType))
				{
					if(OverrideMode)
					{
						LanguageLib[lan.LanguageType].Author = lan.Author;
						LanguageLib[lan.LanguageType].Version = lan.Version;
					}
					foreach(var pair in lan.Lib)
					{
						if(LanguageLib[lan.LanguageType].Lib.ContainsKey(pair.Key)) //Replace the text with the external language
						{
							if(OverrideMode) LanguageLib[lan.LanguageType].Lib[pair.Key] = pair.Value;
						}
						else //Add the new text from the external language
						{
							LanguageLib[lan.LanguageType].Lib.Add(pair.Key, pair.Value);
						}
					}
				}
				else
				{
					LanguageLib.Add(lan.LanguageType, lan);
				}
			}
		}
		#endregion
		
	}
	
	private void Start() 
	{
		//TODO:手动调整start顺序？
		// SavePath = Application.persistentDataPath;
		// LoadLanguage();
		// Debug.LogFormat("Using:{0}, Author:{1}, Version:{2}", GetLocalizedString("Example_Text"), LanguageLib[CurrentLanguage].Author,LanguageLib[CurrentLanguage].Version);
		// GenerateExample();
	}
	
	public string GetLocalizedString(string index)
	{
		if(LanguageLib[CurrentLanguage].Lib.ContainsKey(index))
			return LanguageLib[CurrentLanguage].Lib[index];
		Debug.Log("No such index");
		return index;
	}
	
	public string GetCurrentLanguage()
	{
		return CurrentLanguage;
	}
	
	private void GenerateExample()
	{
		Debug.Log(SavePath);
		Language l = new Language();
		FileStream file = File.Create(SavePath + "/Example.json");
		string json = JsonConvert.SerializeObject(l, Formatting.Indented);
		file.Close();
		StreamWriter writer = new StreamWriter((SavePath + "/Example.json"), false);
		writer.WriteLine(json);
		writer.Close();
	}
}

public class Language
	{
		public string LanguageType;
		public string Author;
		public string Version;
		public Dictionary<string, string> Lib = new Dictionary<string, string>();
		public Language(){}
	}