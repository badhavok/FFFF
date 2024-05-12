using UnityEngine;
using System.Collections;

public class Waypoints : MonoBehaviour {

	public static Transform[] pathPoints1;
	public static Transform[] pathPoints2;
	public static Transform[] pathPoints3;
	private static Transform[] highlightedPoints, calculatePoints;

	public GameObject PathPoints1;
	public GameObject PathPoints2;
	public GameObject PathPoints3;
	private static GameObject highlightedPath, calculatePath;

	// Calculating the length of the path
	public static float totalLength1, totalLength2, totalLength3 = 0;
	public int calculatePath1, calculatePath2, calculatePath3 = 0;

	public LineRenderer lineRenderer;

	void Awake ()
	{
		//Generate list of points from the inspector to calculate the distances between them
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
		if(PathPoints3 != null)
		{	
			pathPoints3 = new Transform[PathPoints3.transform.childCount];
			for (int j = 0; j < PathPoints3.transform.childCount; j++)
			{
				pathPoints3[j] = PathPoints3.transform.GetChild(j);
			}
		}
	}
	
	void Start ()
	{
		PathLength(1);
		if(pathPoints2 != null)
		{
			PathLength(2);
		}
		if(pathPoints3 != null)
		{
			PathLength(3);
		}
	}
	public void PathLength(int path)
	{
		if(path == 1)
		{
			calculatePath = PathPoints1;
			calculatePoints = pathPoints1;
		}
		if(path == 2)
		{
			calculatePath = PathPoints2;
			calculatePoints = pathPoints2;
		}
		if(path == 3)
		{
			calculatePath = PathPoints3;
			calculatePoints = pathPoints3;
		}
		for (int i = 0; i < calculatePath.transform.childCount; i++)
		{
			if (i == calculatePath.transform.childCount - 1)
			{
				return;
			}
			else
			{
					int j = i + 1;
					float calculatePath1 = Vector3.Distance(calculatePoints[i].position, calculatePoints[j].position);
					totalLength1 = calculatePath1 + totalLength1;
			}
			//Debug.Log("This is the path length #1 " + totalLength1 + pathPoints1[i].name);
		}
	}
	public IEnumerator HighlightPath(int path)
	{
		if(path == 1)
		{
			highlightedPath = PathPoints1;
			highlightedPoints = pathPoints1;
		}
		if(path == 2)
		{
			highlightedPath = PathPoints2;
			highlightedPoints = pathPoints2;
		}
		if(path == 3)
		{
			highlightedPath = PathPoints3;
			highlightedPoints = pathPoints3;
		}
		for (int i = 0; i < highlightedPath.transform.childCount; i++)
		{
			if (i == highlightedPath.transform.childCount - 1)
			{
				lineRenderer.enabled = false;
				// return;
			}
			else
			{
				int j = i + 1;
				lineRenderer.SetPosition(0, highlightedPoints[i].position);
				lineRenderer.SetPosition(1, highlightedPoints[j].position);
				lineRenderer.enabled = true;
				Debug.Log("I should be highlighting pathpoint > " + highlightedPoints[i] + " & pathpoint >" + highlightedPoints[j] + " & PosCount is " + lineRenderer.positionCount);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
