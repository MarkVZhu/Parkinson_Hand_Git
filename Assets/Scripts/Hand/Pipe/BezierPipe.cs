using UnityEngine;
using System.Collections.Generic;

//Refer from CSDN Article: 可弯曲软管的动态生成 Author: 夏日的玫瑰
public struct BezierLineSegment
{
	public Vector3 fromPoint;
	public Vector3 toPoint;
	public Vector3 controlPoint;
	public Quaternion fromDir;
	public Quaternion toDir;

	public void CalculateDir()
	{
		fromDir = Quaternion.FromToRotation(Vector3.forward, controlPoint - fromPoint);
		toDir = Quaternion.FromToRotation(Vector3.forward, toPoint - controlPoint);
	}

	public bool IsStraight()
	{
		return fromDir == toDir;
	}
}

[ExecuteInEditMode]
public class BezierPipe : MonoBehaviour
{

	public float cornerScale = 1f;
	[Range(1,100)]
	public int cornerStep = 10;
	[Range(2,100)]
	public int circleStep = 10;

	public float r = 0.1f;

	public Transform point1;
	public Vector3 point1Dir = Vector3.up;
	public Transform point2;
	public Vector3 point2Dir = Vector3.up;

	public bool update = false;

	public Mesh mesh;
	List<Vector3> verts = new List<Vector3>();
	List<int> triangles = new List<int>();
	private MeshCollider mc;

	// Use this for initialization
	void Start ()
	{
		mesh = new Mesh();
		mesh.name = "Pipe";
		MeshFilter mf = GetComponent<MeshFilter>();
		if (mf != null)
		{
			mf.sharedMesh = mesh;
		}
		mc = GetComponent<MeshCollider>();
		if (mc != null)
		{
			mc.sharedMesh = mesh;
		}
		BuildMesh();
	}

	// Update is called once per frame
	void Update()
	{
		if (update)
		{
			BuildMesh();
		}
	}

	void GetCirclePoint(Vector3 pos, Quaternion dir,bool draw)
	{
		for (int a = 0; a <= circleStep; a++)
		{
			float p = 2 * Mathf.PI * a / circleStep;
			Vector3 cp = new Vector3(r * Mathf.Cos(p), r * Mathf.Sin(p), 0);
			cp = dir* cp + pos;
			//if(draw)
			//    Gizmos.DrawSphere(cp, 0.0005f);
			cp = transform.worldToLocalMatrix.MultiplyPoint(cp);
			//cp += transform.position;
			verts.Add(cp);
		}
	}

	void SetTriangles()
	{
		triangles.Clear();
		for (int i = 0; i < verts.Count - circleStep - 2; i ++)
		{
			triangles.Add(i);
			triangles.Add(i+1);
			triangles.Add(i+circleStep + 1);
			triangles.Add(i+circleStep + 1);
			triangles.Add(i+1);
			triangles.Add(i+circleStep + 2);
		}
		mesh.triangles = triangles.ToArray();
	}

	public void BuildMesh()
	{
		if (point1 != null && point2 != null)
		{
			verts.Clear();
			float scale = cornerScale;
			float length = (point1.position - point2.position).magnitude/4;
			if (scale > length)
			{
				scale = length;
			}
			BezierLineSegment[] segments = new BezierLineSegment[3];
			segments[0].fromPoint = point1.position;
			point1Dir.Normalize();
			segments[0].controlPoint = point1.position + point1.rotation * point1Dir * scale;

			segments[2].toPoint = point2.position;
			point2Dir.Normalize();
			segments[2].controlPoint = point2.position + point2.rotation * point2Dir * scale;

			segments[1].controlPoint = (segments[0].controlPoint + segments[2].controlPoint) / 2;

			segments[0].toPoint = segments[1].fromPoint = segments[0].controlPoint + (segments[1].controlPoint - segments[0].controlPoint).normalized * scale;
			segments[1].toPoint = segments[2].fromPoint = segments[2].controlPoint + (segments[1].controlPoint - segments[2].controlPoint).normalized * scale;

			transform.position = segments[1].controlPoint;
			segments[1].CalculateDir();
			transform.rotation = segments[1].fromDir;
//            Debug.Log (transform.eulerAngles);

			foreach (var segment in segments)
			{
				segment.CalculateDir();
				if (segment.IsStraight())
				{
					GetCirclePoint(segment.fromPoint, segment.fromDir,true);
					GetCirclePoint(segment.controlPoint, segment.fromDir,false);
					GetCirclePoint(segment.toPoint, segment.toDir, true);
					//Gizmos.DrawLine(segment.fromPoint, segment.controlPoint);
					//Gizmos.DrawLine(segment.controlPoint, segment.toPoint);
				}
				else
				{
					GetCirclePoint(segment.fromPoint, segment.fromDir, true);
					Vector3 p1 = segment.fromPoint;
					for (int s = 1; s < cornerStep; s++)
					{
						float t = (float) s/cornerStep;
						Vector3 p2 = GetPoint(segment.fromPoint, segment.controlPoint, segment.toPoint, t);
						//Quaternion dir = Quaternion.FromToRotation(Vector3.forward, p2 - p1);
						Quaternion dir = Quaternion.Lerp(segment.fromDir, segment.toDir, t);
						GetCirclePoint(p2,dir,false);
						//Gizmos.DrawLine(p1, p2);
						p1 = p2;
					}
					GetCirclePoint(segment.toPoint, segment.toDir, true);
					//Gizmos.DrawLine(p1,segment.toPoint);
				}
			}
			mesh.Clear();
			mesh.vertices = verts.ToArray();
			SetTriangles();
			mesh.RecalculateNormals();

			if (mc != null)
			{
				mc.sharedMesh = mesh;
			}
		}
	}
	
	private Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
	}
}