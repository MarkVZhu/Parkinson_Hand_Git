using System.Collections.Generic;
using UnityEngine;

public class AvatarJoint : JointBase
{
	private int[] fixedRotationY = {6,7, 10, 11, 14, 15};
	private int[] fixedRotationYL = { 18, 19 };

	public class AvatarTree
	{
		public Transform transf;
		public AvatarTree[] childs;
		public AvatarTree parent;
		public int idx;  // pose_joint's index

		public AvatarTree(Transform tf, int count, int idx, AvatarTree parent = null)
		{
			this.transf = tf;
			this.parent = parent;
			this.idx = idx;
			if (count > 0)
			{
				childs = new AvatarTree[count];
			}
		}

		public Vector3 GetDir()
		{
			if (parent != null)
			{
				return transf.position - parent.transf.position;
			}
			return Vector3.up;
		}
	}

	private AvatarTree wrist, thumb_cmc, thumb_mcp, thumb_ip, thumb_tip, index_finger_mcp, index_finger_pip, index_finger_dip, index_finger_tip, middle_finger_mcp, middle_finger_pip;
	private AvatarTree middle_finger_dip, middle_finger_tip, ring_finger_mcp, ring_finger_pip, ring_finger_dip, ring_finger_tip, pinky_mcp, pinky_pip, pinky_dip, pinky_tip;
	
	private int[] L = {3, 4, 7, 8, 11, 12, 15, 16, 19, 20};
	private List<int> limitYRotationList;

	protected override float speed { get { return 10f; } }

	void Start()
	{
		limitYRotationList = new List<int>(L);
		InitData();
		BuildTree();
	}

	void BuildTree()
	{
		wrist = new AvatarTree(Wrist.transform, 5, 0);
		thumb_cmc = wrist.childs[0] = new AvatarTree(Thumb_CMC.transform, 1, 1, wrist);
		index_finger_mcp = wrist.childs[1] = new AvatarTree(Index_Finger_MCP.transform, 1, 5, wrist);
		middle_finger_mcp = wrist.childs[2] = new AvatarTree(Middle_Finger_MCP.transform, 1, 9, wrist);
		ring_finger_mcp = wrist.childs[3] = new AvatarTree(Ring_Finger_MCP.transform, 1, 13, wrist);
		pinky_mcp = wrist.childs[4] = new AvatarTree(Pinky_MCP.transform, 1, 17, wrist);

		thumb_mcp = thumb_cmc.childs[0] = new AvatarTree(Thumb_MCP.transform, 1, 2, thumb_cmc);
		thumb_ip = thumb_mcp.childs[0] = new AvatarTree(Thumb_IP.transform, 1, 3, thumb_mcp);
		thumb_tip = thumb_ip.childs[0] = new AvatarTree(Thumb_TIP.transform, 0, 4, thumb_ip);

		index_finger_pip = index_finger_mcp.childs[0] = new AvatarTree(Index_Finger_PIP.transform, 1, 6, index_finger_mcp);
		index_finger_dip = index_finger_pip.childs[0] = new AvatarTree(Index_Finger_DIP.transform, 1, 7, index_finger_pip);
		index_finger_tip = index_finger_dip.childs[0] = new AvatarTree(Index_Finger_TIP.transform, 0, 8, index_finger_dip);

		middle_finger_pip = middle_finger_mcp.childs[0] = new AvatarTree(Middle_Finger_PIP.transform, 1, 10, middle_finger_mcp);
		middle_finger_dip = middle_finger_pip.childs[0] = new AvatarTree(Middle_Finger_DIP.transform, 1, 11, middle_finger_pip);
		middle_finger_tip = middle_finger_dip.childs[0] = new AvatarTree(Middle_Finger_TIP.transform, 0, 12, middle_finger_dip);

		pinky_pip = pinky_mcp.childs[0] = new AvatarTree(Pinky_PIP.transform, 1, 18, pinky_mcp);
		pinky_dip = pinky_pip.childs[0] = new AvatarTree(Pinky_DIP.transform, 1, 19, pinky_pip);
		pinky_tip = pinky_dip.childs[0] = new AvatarTree(Pinky_TIP.transform, 0, 20, pinky_dip);

		ring_finger_pip = ring_finger_mcp.childs[0] = new AvatarTree(Ring_Finger_PIP.transform, 1, 14, ring_finger_mcp);
		ring_finger_dip = ring_finger_pip.childs[0] = new AvatarTree(Ring_Finger_DIP.transform, 1, 15, ring_finger_pip);
		ring_finger_tip = ring_finger_dip.childs[0] = new AvatarTree(Ring_Finger_TIP.transform, 0, 16, ring_finger_dip);
	}


	protected override void LerpUpdate(float lerp)
	{
		Wrist.transform.position = pose_joint[0].transform.position - new Vector3(5,0,0);
		//Wrist.transform.rotation = pose_joint[0].transform.rotation;
		//UpdateBone(thumb_cmc, lerp);
		UpdateBone(index_finger_mcp, lerp);
		//UpdateBone(pinky_mcp, lerp);
		//UpdateBone(thumb_mcp, lerp);
		UpdateBone(thumb_ip, lerp);
		UpdateBone(thumb_tip, lerp);
		UpdateBone(index_finger_pip, lerp);
		UpdateBone(index_finger_dip, lerp);
		UpdateBone(index_finger_tip, lerp);
		UpdateBone(middle_finger_mcp, lerp);
		UpdateBone(middle_finger_pip, lerp);
		UpdateBone(middle_finger_dip, lerp);
		UpdateBone(middle_finger_tip, lerp);
		UpdateBone(pinky_pip, lerp);
		UpdateBone(pinky_dip, lerp);
		UpdateBone(pinky_tip, lerp);
		UpdateBone(ring_finger_mcp, lerp);
		UpdateBone(ring_finger_pip, lerp);
		UpdateBone(ring_finger_dip, lerp);
		UpdateBone(ring_finger_tip, lerp);
	}


	private void UpdateTree(AvatarTree tree, float lerp)
	{
		if (tree.parent != null)
		{
			UpdateBone(tree, lerp);
		}
		if (tree.childs != null)
		{
			for (int i = 0; i < tree.childs.Length; i++)
				UpdateTree(tree.childs[i], lerp);
		}
	}

	private void UpdateBone(AvatarTree tree, float lerp)
	{
		var dir1 = tree.GetDir();
		var dir2 = pose_joint[tree.idx].transform.position - pose_joint[tree.parent.idx].transform.position;
		 //dir2.y = -dir2.y;
		 Quaternion rot = Quaternion.FromToRotation(dir1, dir2);
		 Quaternion rot1 = tree.parent.transf.rotation;

		#region Recover rotation
		var lerpToAngle = rot * rot1;
		
		// if(limitYRotationList.Contains(tree.idx))
		// {
		// 	lerpToAngle.y = tree.parent.transf.rotation.y;
		// }
		
		foreach (int n in fixedRotationY)
		{
		   if (tree.parent.idx == n)
		   {
		       lerpToAngle.eulerAngles = new Vector3(lerpToAngle.eulerAngles.x, 0, lerpToAngle.eulerAngles.z);
		       Debug.Log(Middle_Finger_MCP.transform.rotation.eulerAngles.y);
		   }
		}

		foreach (int n in fixedRotationYL)
		{
		   if (tree.parent.idx == n)
		   {
		       lerpToAngle.eulerAngles = new Vector3(lerpToAngle.eulerAngles.x, Pinky_MCP.transform.rotation.eulerAngles.y, lerpToAngle.eulerAngles.z);
		       Debug.Log(Middle_Finger_MCP.transform.rotation.eulerAngles.y);
		   }
		}
		#endregion

		tree.parent.transf.rotation = Quaternion.Lerp(rot1, lerpToAngle, lerp);
	}
}