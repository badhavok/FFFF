using UnityEngine;
using System.Collections;

//This is the script used to control all the movement code/functions
[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

	private Enemy enemy;
	private EnemyDots dots;
	public Transform target;
	public int wavepointIndex = 0;
	private bool runBack = false;
	private bool goLeft;
	private float finalCountdown;
	//private bool aboveGround;
	public float wormMove;
	private bool wormStop;
	private string stop = "Stop";
	private bool iDle;
	public bool endPath, attackBase = false;

	private Animator anim;

	public float pathDistance, timeToDestination, remainingPathDistance, endPathCounter, attackCounter;

	public float endPathTime = 6;

	private float randomNumber, randomNumberTwo, lastNumber, lastNumberTwo;
	private int maxAttempts = 10;
	public int walkingPath;

	void Start()
	{
		//This is used when the enemy is "duplicated" (I.E doesn't start at the beginning of the path)
		enemy = GetComponent<Enemy>();
		dots = enemy.GetComponent<EnemyDots>();
		anim = gameObject.GetComponentInChildren<Animator>();
		walkingPath = Enemy.path;
		Waypoints waypoints = FindObjectOfType<Waypoints>();
		//Debug.Log("Enemy " + enemy.name + " Path " + walkingPath);
		//Waypoints waypoints = new Waypoints();
		if (enemy.fromDropship)
		{
			if(endPath)
			{
				return;
			}
			if(walkingPath == 2)
			{
				waypoints.PathLength(2);
				target = Waypoints.pathPoints2[wavepointIndex];
				pathDistance = waypoints.totalLength2;
			}
			else
			{
				waypoints.PathLength(1);
				target = Waypoints.pathPoints1[wavepointIndex];
				pathDistance = waypoints.totalLength1;
			}
			transform.LookAt(target);
		}
		else if (!enemy.fromDropship)
		{
			if(walkingPath == 2)
			{
				waypoints.PathLength(2);
				target = Waypoints.pathPoints2[0];
				pathDistance = waypoints.totalLength2;
			}
			else
			{
				waypoints.PathLength(1);
				target = Waypoints.pathPoints1[0];
				pathDistance = waypoints.totalLength1;
			}
			transform.LookAt(target);
		}
		NewRandomNumber();
		finalCountdown = randomNumber;
		wormMove = randomNumber;
		timeToDestination = pathDistance / enemy.speed;
		// essentially starts a countdown to the target.
		// if speed * someTime = PathDistance then someTime = pathDistance/ speed
		remainingPathDistance = pathDistance;
		//starts at max, goes to 0
	}
	//Generates numbers to make enemies take different paths
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
			randomNumberTwo = Random.Range(10, 14);
		}

		lastNumberTwo = randomNumberTwo;
	}

	void Update()
	{
		if (enemy.isDead)
		{
			enabled = false;
			return;
		}
		if (endPath)
		{
			if (enemy.isWorm)
			{
				this.gameObject.tag = "Enemy";
				anim.SetBool("Up", true);
				anim.SetBool("Down", false);
				anim.SetBool("Move", true);
				Debug.Log("End path worm");
			}
			if (attackBase)
			{
				attackCounter -= Time.deltaTime;

				if(attackCounter < 0)
				{
					--PlayerStats.Lives;
					attackBase = false;
				}
			}
			EndPath();
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
				transform.Translate(Vector3.left * Time.deltaTime * 1.5f);
			}
			else
			{
				transform.Translate(Vector3.right * Time.deltaTime * 1.5f);
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
				dots.DotEffect(stop, randomNumberTwo / 1.55f, 0);
			}
			else if(wormMove < 12 && wormMove > 8 )
			{
				//Debug.Log("I'm in wormMove ++");
				//aboveGround = true;
				this.gameObject.tag = "Enemy";
				anim.SetBool("Up", true);
				anim.SetBool("Down", false);
				anim.SetBool("Move", false);
				wormMove -= Time.deltaTime;
			}
			else if(wormMove < 8 && wormMove > 6)
			{
				this.gameObject.tag = "Enemy";
				anim.SetBool("Up", true);
				anim.SetBool("Down", false);
				anim.SetBool("Move", false);
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
				anim.SetBool("Move", true);
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
		endPath = true;
		if(enemy.isRandom)
		{
			enemy.Die();
			enemy.noDrop = true;
			return;
		}
		anim.SetBool("Move", false);
		
		if(endPathCounter < 0)
		{
			attackBase = true;
			attackCounter = 1;
			anim.SetBool("Attack", true);
			anim.SetBool("Static", false);
			endPathCounter = endPathTime;
		}
		else if (endPathCounter > 5)
		{
			endPathCounter -= Time.deltaTime;
		}
		else
		{
			endPathCounter -= Time.deltaTime;
			anim.SetBool("Attack", false);
			anim.SetBool("Static", true);
		}
	}

}
