using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
	public AudioMixer mixer;
	
	public void SetBGMVolume(float value)
	{
		if(value == -40) value = -80;
		mixer.SetFloat("BGM", value);
	}
	
	public void SetSFXVolume(float value)
	{
		if(value == -40) value = -80;
		mixer.SetFloat("UISFX", value);
	}
}
