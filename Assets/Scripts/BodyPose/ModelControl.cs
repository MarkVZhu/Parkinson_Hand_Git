using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelControl : MonoBehaviour
{
	public Transform headTrans, bodyTrans;
	public Transform[] leftHands, leftLegs;
	public Transform[] rightHands, rightLegs;
	public Transform bodyCenter;
	public Vector3 kuaOffset;
	[Range(0, 1)]public float lerpSpeed;
	Vector3 bodyOriginCenter;
	Vector3 originKuaPos;
	private PosTracking posTracking;
	private GameObject[] bodyPoints;
	
	void Start()
	{
		posTracking = GetComponent<PosTracking>();
		bodyPoints = posTracking.PosePoints;
		bodyOriginCenter = bodyCenter.position;
	}
	
	void Update()
	{
		if (posTracking.isReceived())
		{
			//取7，8的中间点,计算与0点的方向; 头的旋转
			Vector3 headCenter = (bodyPoints[7].transform.position + bodyPoints[8].transform.position) / 2;
			headCenter.y = bodyPoints[0].transform.position.y;
			headTrans.forward = Vector3.Lerp(headTrans.forward,((bodyPoints[0].transform.position-headCenter).normalized), lerpSpeed);

			//身体的旋转
			Vector3 pos1 = bodyPoints[11].transform.position;
			Vector3 pos2 = bodyPoints[12].transform.position;
			pos1.y = 0;
			pos2.y = 0;
			Vector3 bodyDir = Quaternion.AngleAxis(90, Vector3.up) * (pos1 - pos2).normalized;
			bodyTrans.forward = Vector3.Lerp(bodyTrans.forward,bodyDir, lerpSpeed);
			
			//11,13,15 左胳膊旋转
			leftHands[0].up = Vector3.Lerp(leftHands[0].up,(bodyPoints[13].transform.position - bodyPoints[11].transform.position).normalized, lerpSpeed);
			leftHands[1].up = Vector3.Lerp(leftHands[1].up, (bodyPoints[15].transform.position - bodyPoints[13].transform.position).normalized, lerpSpeed);
			
			//12,14,16 右胳膊旋转
			rightHands[0].up = Vector3.Lerp(rightHands[0].up,(bodyPoints[14].transform.position - bodyPoints[12].transform.position).normalized, lerpSpeed);
			rightHands[1].up = Vector3.Lerp(rightHands[1].up, (bodyPoints[16].transform.position - bodyPoints[14].transform.position).normalized, lerpSpeed);
			
			//23,25,27,31 左腿旋转
			//24,26,28,32 右腿旋转
			leftLegs[0].up =Vector3.Lerp(rightLegs[0].up,(bodyPoints[26].transform.position - bodyPoints[24].transform.position).normalized, lerpSpeed);
			leftLegs[1].up =Vector3.Lerp(rightLegs[1].up,(bodyPoints[28].transform.position - bodyPoints[26].transform.position).normalized, lerpSpeed);
			//leftLegs[2].forward =Vector3.Lerp(leftLegs[2].forward,(bodyPoints[31].transform.position - bodyPoints[29].transform.position).normalized, lerpSpeed);

			rightLegs[0].up = Vector3.Lerp(rightLegs[0].up,(bodyPoints[26].transform.position - bodyPoints[24].transform.position).normalized, lerpSpeed);
			rightLegs[1].up = Vector3.Lerp(rightLegs[1].up,(bodyPoints[28].transform.position - bodyPoints[26].transform.position).normalized, lerpSpeed);
			rightLegs[2].forward = Vector3.Lerp(rightLegs[2].forward,(bodyPoints[32].transform.position - bodyPoints[30].transform.position).normalized, lerpSpeed);
		}
	}
	
	private void UpdateBone(Transform joint, int thisIndex, int parentIndex)
	{
		var dir1 = joint.up;
		var dir2 = bodyPoints[thisIndex].transform.position - bodyPoints[parentIndex].transform.position;
		Quaternion rot = Quaternion.FromToRotation(dir1, dir2);
		Quaternion rot1 = joint.parent.rotation;
		joint.parent.rotation = Quaternion.Lerp(rot1, rot, lerpSpeed);
	}
}
