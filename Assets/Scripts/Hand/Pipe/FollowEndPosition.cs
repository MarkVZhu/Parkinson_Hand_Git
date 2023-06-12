using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEndPosition : MonoBehaviour
{
	public Transform targetTrans;
	[Range(0,0.1f)]public float offset;

	// Update is called once per frame
	void Update()
	{
		this.transform.position = targetTrans.position - new Vector3(0,offset,0);
	}
}
