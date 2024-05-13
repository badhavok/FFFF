using UnityEngine;
using System.Collections;

public class Waypoints : MonoBehaviour {

	public static Transform[] pathPoints1;
	public static Transform[] pathPoints2;
	public static Transform[] pathPoints3;
	private static Transform[] highlightedPoints, calculatePoints;
	private float one = 2f;

	public GameObject PathPoints1;
	public GameObject PathPoints2;
	public GameObject PathPoints3;
	private static GameObject highlightedPath, calculatePath;

	// Calculating the length of the path
	public float totalLength, totalLength1, totalLength2, totalLength3 = 0;
	private bool countedOne, countedTwo, countedThree = false;

	public GameObject arrowObject;
	private GameObject arrow;

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
		StartCoroutine(PathLength(1));
	}
	void Update()
	{
		if(one > 1)
		{
			one -= Time.deltaTime;
		}
		else if(one < 1 && one > 0.5f)
		{
			StartCoroutine(HighlightPath(1));
			one -= Time.deltaTime;
		}
		else
		{

		}
	}
	public IEnumerator PathLength(int pathL)
	{
		if(pathL == 1)
		{
			calculatePath = PathPoints1;
			calculatePoints = pathPoints1;

			yield return StartCoroutine(CalculatingPath(calculatePath, calculatePoints));
		}
		if(pathL == 2)
		{
			calculatePath = PathPoints2;
			calculatePoints = pathPoints2;
			
			yield return StartCoroutine(CalculatingPath(calculatePath, calculatePoints));
		}
		if(pathL == 3)
		{
			calculatePath = PathPoints3;
			calculatePoints = pathPoints3;
			
			yield return StartCoroutine(CalculatingPath(calculatePath, calculatePoints));
		}
	}
	public IEnumerator CalculatingPath(GameObject calcThisPath, Transform[] calcThesePoints)
	{
		for (int i = 0; i < calcThisPath.transform.childCount; i++)
		{
			if (i == calcThisPath.transform.childCount - 1)
			{
				if(!countedOne)
				{
					countedOne = true;
					totalLength1 = totalLength;
					
					totalLength = 0;
					
					yield return StartCoroutine(PathLength(2));
				}
				if(PathPoints2 != null && !countedTwo)
				{
					countedTwo = true;
					totalLength2 = totalLength;
					
					totalLength = 0;
					
					if(PathPoints3 != null)
					{
						yield return StartCoroutine(PathLength(3));
					}
					else
					{
						yield return totalLength2;
					}
				}
				if(PathPoints3 != null && !countedThree)
				{
					countedThree = true;
					yield return totalLength3;
				}
			}
			else
			{
					int j = i + 1;
					float calculatedPath = Vector3.Distance(calcThesePoints[i].position, calcThesePoints[j].position);
					totalLength = calculatedPath + totalLength;
			}
			Debug.Log("This is the path length " + totalLength + pathPoints1[i].name);
		}
	}
	public IEnumerator HighlightPath(int pathH)
	{
		Debug.Log("Highlighting the path");
		if(pathH == 1)
		{
			highlightedPath = PathPoints1;
			highlightedPoints = pathPoints1;

			yield return StartCoroutine(HighlightThisPath(highlightedPath, highlightedPoints));
		}
		if(pathH == 2)
		{
			highlightedPath = PathPoints2;
			highlightedPoints = pathPoints2;
			
			yield return StartCoroutine(HighlightThisPath(highlightedPath, highlightedPoints));
		}
		if(pathH == 3)
		{
			highlightedPath = PathPoints3;
			highlightedPoints = pathPoints3;
			
			yield return StartCoroutine(HighlightThisPath(highlightedPath, highlightedPoints));
		}
	}
	public IEnumerator HighlightThisPath(GameObject hlThisPath, Transform[] hlThesePoints)
	{
		for (int i = 0; i < hlThisPath.transform.childCount; i++)
			{
				if (i != hlThisPath.transform.childCount - 1)
				{
					int j = i + 1;
					GameObject arrow = Instantiate(arrowObject, hlThesePoints[i].position, Quaternion.identity);

					arrow.transform.LookAt(hlThesePoints[j]);
					Destroy(arrow, 0.75f);
					// Debug.Log("I should be highlighting pathpoint > " + hlThesePoints[i] + " & pathpoint >" + hlThesePoints[j]);
				}

				yield return new WaitForSeconds(1f);
			}
	}
}
