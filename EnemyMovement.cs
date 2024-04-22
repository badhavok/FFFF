// private Transform[] path;

// private void Start()
// {
//   path = Waypoints.GetPath(Enemy.path);
// }

// // waypoints
// public Transform[] GetPath(int index)
// {
//   if (index == 1) { return path1; }
  
//   return path2;
// }

using UnityEngine;
using System.Collections;

//This is the script used to control all the movement code/functions
[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

	public Transform target;
	public int wavepointIndex = 0;
	private bool runBack = false;
	private bool goLeft;
	private float finalCountdown;
	//private bool aboveGround;
	public float wormMove;
	private bool wormStop;
	private bool iDle;

	private Enemy enemy;
	private Animator anim;

	public float pathDistance, timeToDestination, remainingPathDistance, endPathCounter;

	public float endPathTime = 8;

	private float randomNumber, randomNumberTwo, lastNumber, lastNumberTwo;
	private int maxAttempts = 10;
	public int walkingPath;

	void Start()
	{
		//This is used when the enemy is "duplicated" (I.E doesn't start at the beginning of the path)
		enemy = GetComponent<Enemy>();
		anim = gameObject.GetComponentInChildren<Animator>();
		walkingPath = Enemy.path;
		//Debug.Log("Enemy " + enemy.name + " Path " + walkingPath);
		//Waypoints waypoints = new Waypoints();
		if (enemy.fromDropship)
		{
			if(walkingPath == 2)
			{
				target = Waypoints.pathPoints2[wavepointIndex];
				pathDistance = Waypoints.totalLength2;
			}
			else
			{
				target = Waypoints.pathPoints1[wavepointIndex];
				pathDistance = Waypoints.totalLength1;
			}
			transform.LookAt(target);
		}
		else if (!enemy.fromDropship)
		{
			if(walkingPath == 2)
			{
				target = Waypoints.pathPoints2[0];
				pathDistance = Waypoints.totalLength2;
			}
			else
			{
				target = Waypoints.pathPoints1[0];
				pathDistance = Waypoints.totalLength1;
			}
			transform.LookAt(target);
		}
		NewRandomNumber();
		finalCountdown = randomNumber;
		timeToDestination = pathDistance / enemy.speed;
		// essentially starts a countdown to the target.
		// if speed * someTime = PathDistance then someTime = pathDistance/ speed
		remainingPathDistance = pathDistance;
		//starts at max, goes to 0
	}

	void NewRandomNumber()
	{
		for(int i=0; randomNumber == lastNumber && i < maxAttempts; i++)
		{
			randomNumber = Random.Range(0, 4);
		}

		lastNumber = randomNumber;
	}
	void NewRandomNumberTwo()
	{
		for(int i=0; randomNumberTwo == lastNumberTwo && i < maxAttempts; i++)
		{
			randomNumberTwo = Random.Range(10, 15);
		}

		lastNumberTwo = randomNumberTwo;
	}

	void Update()
	{
		if (enemy.isDead)
		{
			return;
		}
		if (enemy.stopEnemy)
		{
		
		}
		else if (enemy.speedEnemy)
		{

		}
		else if (enemy.castingEnemy)
		{

		}
		else if	(enemy.slowEnemy)
		{
			timeToDestination -= Time.deltaTime / enemy.speed;
			remainingPathDistance -= enemy.speed * Time.deltaTime;
		}
		else
		{
			timeToDestination -= Time.deltaTime;
			remainingPathDistance -= enemy.speed * Time.deltaTime;
		}
		if (enemy.fearEnemy)
		{
			timeToDestination += Time.deltaTime / enemy.speed;
			remainingPathDistance += enemy.speed * Time.deltaTime;
		}

		//Debug.Log("My speed is: " + enemy.speed + "... + Time to destination " + timeToDestination + ". My remaining distance is " + remainingPathDistance);
		Vector3 dir = target.position - transform.position;
		transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);
		//Debug.Log("I'm travelling at a speed of " + enemy.speed + " .");

		if (enemy.isFlying && enemy.enemyStats.isHovercraft)
		{
			transform.Translate(Vector3.up * Time.deltaTime / 2, Space.World);
		}
		//"isSpider" is used for any small creatures to make them more animated and 'walk' across the path more freely
		//Might expand this with a variable so normal units can also walk a bit more freely, but less so than larger equivalents to keep them within the viewable path
		if (enemy.isSpider)
		{
			//Which direction am I moving?
			if (goLeft)
			{
				transform.Translate(Vector3.left * Time.deltaTime * 2);
			}
			else
			{
				transform.Translate(Vector3.right * Time.deltaTime * 2);
			}
			//The counter to control how long the enemy is moving to one side
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

		if(enemy.isWorm)
		{
			if(wormMove <= 0)
			{
				this.gameObject.tag = "Fallen";
				NewRandomNumberTwo();
				//Debug.Log("My Random number is " + randomNumberTwo + " ... wow");
				wormMove += randomNumberTwo;
				enemy.Stop(randomNumberTwo / 1.75f);
			}
			else if(wormMove < 10 && wormMove > 8 )
			{
				//Debug.Log("I'm in wormMove ++");
				//aboveGround = true;
				this.gameObject.tag = "Enemy";
				anim.SetBool("Up", true);
				anim.SetBool("Down", false);
				anim.SetBool("Static", true);
				wormMove -= Time.deltaTime;
			}
			else if(wormMove < 8 && wormMove > 6)
			{
				this.gameObject.tag = "Enemy";
				anim.SetBool("Up", true);
				anim.SetBool("Down", false);
				anim.SetBool("Static", true);
				//Debug.Log("Setting Idle status");
				wormMove -= Time.deltaTime;
			}
			else if(wormMove < 6 && wormMove > 3)
			{
				//aboveGround = false;
				this.gameObject.tag = "Fallen";
				//Debug.Log("Setting aboveGround to false");
				wormMove -= Time.deltaTime;
				anim.SetBool("Up", false);
				anim.SetBool("Down", true);
				anim.SetBool("Static", false);
			}
			else
			{
				//Debug.Log("I'm in the else");
				wormMove -= Time.deltaTime;
			}
		}
		//Do I need to run backwards?
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
	//Function to get the last waypoint when running backwards
	void GetLastWaypoint()
	{	
		--wavepointIndex;

		if(walkingPath == 2)
		{
			target = Waypoints.pathPoints2[wavepointIndex];
		}
		else
		{
			target = Waypoints.pathPoints1[wavepointIndex];
		}
		runBack = true;
	}
	//Function for moving forwards or detecting the enemy is at the end of the path
	void GetNextWaypoint()
	{
		++wavepointIndex;

		if(walkingPath == 2)
		{
			if (wavepointIndex >= Waypoints.pathPoints2.Length)
			{
				EndPath();
				return;
			}
			target = Waypoints.pathPoints2[wavepointIndex];
			//Debug.Log("I'm path 2 & " + gameObject.name + " - " + walkingPath + " " + target);
		}
		else
		{
			if (wavepointIndex >= Waypoints.pathPoints1.Length)
			{
				EndPath();
				return;
			}
			
			target = Waypoints.pathPoints1[wavepointIndex];
			//Debug.Log("I'm path 1 & " + walkingPath + " " + target);
		}
		runBack = false;
	}
	//When enemy is at the end path, lose a player life & despawn
	void EndPath()
	{
		if(endPathCounter < 0)
		{
			--PlayerStats.Lives;
			endPathCounter = endPathTime;
		}
		else
		{
			endPathCounter -= Time.deltaTime;
		}
	}

}
