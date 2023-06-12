using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
	//Store single Audio's info 
	[System.Serializable]
	public class Sound
	{
		[Header("Audio Clip")]
		public AudioClip clip;
		[Header("Audio Group")]
		public AudioMixerGroup outputGroup;
		
		[Range(0,1)] public float volume;
		public bool playOnAwake;
		public bool loop;
	}
	
	public List<Sound> sounds;

	public Dictionary<string, AudioSource> audiosDic;
	
	protected override void Awake() 
	{
		base.Awake();
		audiosDic = new Dictionary<string, AudioSource>();		
		DontDestroyOnLoad(this);	
	}
	
	private void Start() 
	{
		//create game objects with AudioSource for each sound in the list & add it into the dictionary
		foreach(var sound in sounds)
		{
			GameObject obj = new GameObject(sound.clip.name);
			obj.transform.SetParent(transform);
			
			AudioSource source = obj.AddComponent<AudioSource>();
			source.clip = sound.clip;
			source.playOnAwake = sound.playOnAwake;
			source.loop = sound.loop;
			source.volume = sound.volume;
			source.outputAudioMixerGroup = sound.outputGroup;
			
			if(sound.playOnAwake)
				source.Play();
				
			audiosDic.Add(sound.clip.name, source);
		}	
	}
	
	public void PlayAudio(string name, bool isWait = false)
	{
		//Debug.Log("Test Play");
		if(!audiosDic.ContainsKey(name))
		{
			Debug.LogWarning($"{name}does not exist");
			return;
		}
		if(isWait)
		{
			if(!audiosDic[name].isPlaying)
				audiosDic[name].Play();
		}
		else 
			audiosDic[name].Play();
	}
	
	public void StopAudio(string name)
	{
		if(!audiosDic.ContainsKey(name))
		{
			Debug.LogWarning($"{name}does not exist");
			return;
		}
		audiosDic[name].Stop();
	}
}
