using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class OpenExe : MonoBehaviour
{
	private string exePath; // exe path
	private Process process; 

	private void Start() 
	{
		if(SceneManager.GetActiveScene().buildIndex == 0)
			Close();
	}

	public void Open()
	{
		exePath = Application.streamingAssetsPath + "/HandCap.exe";
		UnityEngine.Debug.Log(exePath);
		process = Process.Start(exePath);
	}

	public void Close()
	{
		// Search Process according to the name
		Process[] processes = Process.GetProcessesByName("HandCap");
		if (processes.Length == 0)
		{
			// Process not found
			UnityEngine.Debug.Log("No process found with the name HandCap");
			return;
		}

		// close the first found process
		process = processes[0];
		process.Kill();
	}





}
