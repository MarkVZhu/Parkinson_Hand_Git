using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosTracking : MonoBehaviour
{
	public UDPReceive udpReceive;
	public GameObject[] PosePoints;
	private string data;

	// Update is called once per frame
	void Update()
	{
		//Data Process
		data = udpReceive.data;
		// data = data.Remove(0,1);
		data = data.Remove(data.Length-1,1);
		//Debug.Log(data);
		
		string[] points = data.Split(',');
		
		//0		   1*3      2*3
		//x1,y1,z1,x2,y2,z2,x3,y3,z3
		for(int i = 0; i <= 32; i++)
		{
			float x = float.Parse(points[i*3])/100;
			float y = float.Parse(points[i*3 + 1])/100;
			float z = float.Parse(points[i*3 + 2])/100;
			
			PosePoints[i].transform.localPosition = new Vector3(x,y,z);
		}
	}
	
	public bool isReceived()
	{
		return data != null ? true : false;
	}
}
