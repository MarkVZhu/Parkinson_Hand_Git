using System;
using System.IO;
using UnityEngine;

/*
 *  human3.6m关节点标注顺序
 *  https://www.stubbornhuang.com/529/
 */

public class JointBase : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    public string path;

    protected GameObject[] pose_joint;
    public HandTracking HandManager;
    [Space]

    public GameObject Wrist;  
    public GameObject Thumb_CMC;
    public GameObject Thumb_MCP; 
    public GameObject Thumb_IP; 
    public GameObject Thumb_TIP;  
    public GameObject Index_Finger_MCP;
    public GameObject Index_Finger_PIP; 
    public GameObject Index_Finger_DIP;
    public GameObject Index_Finger_TIP; 
    public GameObject Middle_Finger_MCP; 
    public GameObject Middle_Finger_PIP; 
    public GameObject Middle_Finger_DIP; 
    public GameObject Middle_Finger_TIP; 
    public GameObject Ring_Finger_MCP; 
    public GameObject Ring_Finger_PIP;
    public GameObject Ring_Finger_DIP;
    public GameObject Ring_Finger_TIP; 
    public GameObject Pinky_MCP;  
    public GameObject Pinky_PIP; 
    public GameObject Pinky_DIP;
    public GameObject Pinky_TIP; 

    protected virtual float speed { get { return 0f; } }

    protected float lerp = 0f;
    protected Vector3[] skeleton;
    private int idx = 0, max = 100;

    protected void InitData() //TODO: 修改
    {
        pose_joint = HandManager.handPoints;
    }


    public void Reinit()
    {
        idx = 0;
        lerp = 0;
        InitData();
    }


    protected virtual void Update() //TODO: 修改
    {
        lerp += speed * Time.deltaTime;
        LerpUpdate(lerp);
        if (lerp >= 1)
        {
            if (++idx >= max) idx = 0;
            pose_joint = HandManager.handPoints;
        }
    }

    protected virtual void LerpUpdate(float lerp)
    {

    }

}
