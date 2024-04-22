using UnityEngine;

public class Waypoints : MonoBehaviour {

	public static Transform[] pathPoints1;
	public static Transform[] pathPoints2;

	public GameObject PathPoints1;
	public GameObject PathPoints2;
	public GameObject PathPoints3;

	// Calculating the length of the path
	public static float totalLength1, totalLength2 = 0;
	public int calculatePath1, calculatePath2 = 0;

	void Awake ()
	{
		if (PathPoints1 != null)
		{
			pathPoints1 = new Transform[PathPoints1.transform.childCount];
			for (int i = 0; i < PathPoints1.transform.childCount; i++)
			{
				pathPoints1[i] = PathPoints1.transform.GetChild(i);
			}
		}
		if(PathPoints2 != null)
		{	
			pathPoints2 = new Transform[PathPoints2.transform.childCount];
			for (int j = 0; j < PathPoints2.transform.childCount; j++)
			{
				pathPoints2[j] = PathPoints2.transform.GetChild(j);
			}
		}
	}
	
	void Start ()
	{
		PathLengthOne();
		PathLengthTwo();
	}

	public void PathLengthOne()
	{
		for (int i = 0; i < PathPoints1.transform.childCount; i++)
		{
			if (i == PathPoints1.transform.childCount - 1)
			{
				return;
			}
			else
			{
					int j = i + 1;
					float calculatePath1 = Vector3.Distance(pathPoints1[i].position, pathPoints1[j].position);
					totalLength1 = calculatePath1 + totalLength1;
			}
			//Debug.Log("This is the path length #1 " + totalLength1 + pathPoints1[i].name);
		}
	}
	public void PathLengthTwo()
	{
		for (int i = 0; i < PathPoints2.transform.childCount; i++)
		{
			if (i == PathPoints2.transform.childCount - 1)
			{
				return;
			}
			else
			{
					int j = i + 1;
					float calculatePath2 = Vector3.Distance(pathPoints2[i].position, pathPoints2[j].position);
					totalLength2 = calculatePath2 + totalLength2;
			}
			//Debug.Log("This is the path length #2 " + totalLength2 + pathPoints2[i].name);
		}
	}
}
