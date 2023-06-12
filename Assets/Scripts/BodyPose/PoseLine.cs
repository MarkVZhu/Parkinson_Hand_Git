using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseLine : MonoBehaviour
{
	[SerializeField] private PosTracking posTracking;
	LineRenderer lineRenderer;
	public Transform origin; //Start point
	public Transform destination;//End point
	
	// Start is called before the first frame update
	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.startWidth = 0.1f;
		lineRenderer.endWidth = 0.1f;
	}

	void Start() 
	{
		string name = this.name;
		name = name.Remove(0,4);
		string[] linePoints = name.Split('-');
		origin = posTracking.PosePoints[int.Parse(linePoints[0])].transform;
		destination = posTracking.PosePoints[int.Parse(linePoints[1])].transform;
	}

	// Update is called once per frame
	void Update()
	{
		lineRenderer.SetPosition(0, origin.position);
		lineRenderer.SetPosition(1, destination.position);
	}
}
