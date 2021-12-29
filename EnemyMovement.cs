using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

	public Transform target;
	public int wavepointIndex = 0;
	private bool runBack = false;
	private bool goLeft;
	private float finalCountdown;

	private Enemy enemy;

	void Start()
	{
		enemy = GetComponent<Enemy>();
		if (enemy.fromDropship)
		{
			target = Waypoints.points[wavepointIndex];
			transform.LookAt(target);
		}
		else if (!enemy.fromDropship)
		{
			target = Waypoints.points[0];
			transform.LookAt(target);
		}
		var rand = Random.Range(0, 4);
		finalCountdown = rand;
	}

	void Update()
	{
		Vector3 dir = target.position - transform.position;
		transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);
		
		if (enemy.isFlying && enemy.enemyStats.isHovercraft)
		{
			transform.Translate(Vector3.up * Time.deltaTime / 2, Space.World);
		}
		if (enemy.isSpider)
		{
			if (goLeft)
			{
				transform.Translate(Vector3.left * Time.deltaTime * 2);
			}
			else
			{
				transform.Translate(Vector3.right * Time.deltaTime * 2);
			}
			
			if (finalCountdown <= 0)
			{
				goLeft = true;
				finalCountdown += 3;
			}
			else if (finalCountdown < 1.5)
			{
				finalCountdown -= Time.deltaTime;
				goLeft = false;
			}
			else
			{
				finalCountdown -= Time.deltaTime;
			}
		}
		
		if (enemy.fearEnemy)
		{	
			if (!runBack)
			{
				GetLastWaypoint();
				transform.LookAt(target);
				//Debug.Log("Towards : " + target + "");
			}
			else
			{
				if (Vector3.Distance(transform.position, target.position) <= 0.4f)
				{
					//Debug.Log("ONWARDSSS");
					GetLastWaypoint();
					transform.LookAt(target);
				}
			}
		}
		else
		{
			if (runBack)
			{
				GetNextWaypoint();
				transform.LookAt(target);
				//Debug.Log("Towards : " + target + "");
			}
			else
			{
				if (Vector3.Distance(transform.position, target.position) <= 0.4f)
				{
					//Debug.Log("ONWARDSSS");
					GetNextWaypoint();
					transform.LookAt(target);
				}
			}
		}	
	}
	
	
	void GetLastWaypoint()
	{
		--wavepointIndex;
		target = Waypoints.points[wavepointIndex];
		runBack = true;
	}

	void GetNextWaypoint()
	{
		if (wavepointIndex >= Waypoints.points.Length - 1)
		{
			EndPath();
			return;
		}

		++wavepointIndex;
		target = Waypoints.points[wavepointIndex];
		runBack = false;
	}
	
	void EndPath()
	{
		--PlayerStats.Lives;
		--WaveSpawner.EnemiesAlive;
		Destroy(gameObject);
	}

}
