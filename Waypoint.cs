using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Waypoint {
     public Transform[] points;
}


//________________________________________________



// using UnityEngine;
// using System.Collections;

// public class Waypoint : MonoBehaviour {

// 	public Waypoints[] waypoint;
// 	public int waypointIndex = 0;
// 	public static Transform points;
// 	private Transform[] pointsToCalculate;

// 	// Calculating the length of the path
// 	public static float totalLength = 0;
// 	public int counter, calculatePath = 0;

// 	void Awake ()
// 	{
// 		// path = Wavespawner. 
// 		//Wave wave = waves[waveIndex];
// 		//waypointIndex must be the path that the enemy is requesting
// 		//PathLength();
		
// 	}


// 	IEnumerator CalculateWaypoints()
// 	{
// 		Waypoints waypoints = waypoint[waypointIndex];

// 		for (int i = 0; i < counter; i++)
// 		{
// 			path = new Transform[waypoint.childcount]; 	
// 		}
// 	}
	
// 	IEnumerator WayPoints()
// 	{
// 		pointsToCalculate = new Transform[transform.childCount];
// 		for (int i = 0; i < waypoint.Length; i++)
// 		{
// 			//points[i] = transform.GetChild(i);
// 		}
// 	}

// 	// IEnumerator PathLength()
// 	// {
// 	// 	for (int i = 0; i < points[i]; i++)
// 	// 	{
// 	// 		if (i == Waypoints.points.Length - 1)
// 	// 		{
// 	// 			return;
// 	// 		}
// 	// 		else
// 	// 		{
// 	// 				int j = i + 1;
// 	// 				float calculatePath = Vector3.Distance(Waypoints.points[i].position, Waypoints.points[j].position);
// 	// 				totalLength = calculatePath + totalLength;
// 	// 		}
// 	// 		Debug.Log("This is the path length " + totalLength);
// 	// 	}

// 	// }
// }
