using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {

	public AudioSource audioSource;
	public AudioClip[] audioClipArray;
	public Waypoints waypoints;
	public static int EnemiesAlive = 0;
	public int enemiesAlive;
	public static int BossAlive = 0;
	public static int loops = 0;
	public static int bossloops = 0;

	//Array to set the enemies used in the level
	public Wave[] waves;
	//Array to set the bosses used in the level
	public Boss[] bosses;
	//Array for Random/bonus encounters
	public int randomChance;
	public float spawnTimer = 0f;
	private float spawnCountdownTimer, nMTimer;
	private bool bonusWave, nMIncoming;
	private int randomIndex = 0;
	public Wave[] randoms;

	public GameObject enemy;
	public GameObject boss;
	public Transform spawnPointOne, spawnPointTwo;
	public GameObject spawnLocationTwo;
	private int path;
	[HideInInspector] public Transform spawnPoint;

	//Setting counter in Inspector and private for displaying in the UI
	public float timeBetweenWaves = 1f;
	public float countdown = 1f;

	//Set the variables used to calculate the waves and show in the UI
	public int waveIndex = 0;
	public int waveIndexDisplay = 0;
	public bool waveComplete = false;
	public int counter = 0;
	public int ecounter = 0;
	//Set the variables used to calculate the boss info and show in the UI
	public int bossIndex = 0;
	public int bossIndexDisplay = 0;
	public bool bossComplete = false;
	public int bosscounter = 0;
	public int bcounter = 0;

	//Need to test if this is needed or not - might have been combined
	//public int enemiesAlive;

	//Set the variables for the wave details + UI
	public static int CurrentWave;
	public int currentWave = 0;
	private int currentWaveDisplay = 0;
	private int totalWaves = 0;
	private int bossInterval = 0;

	//Text variables for UI
	public Text waveCountdownText;
	public Text currentWaveText;
	//Setting the game manager to handle win/lose
	public GameManager gameManager;
	//Setting to control the player profile?
//	public GFInit gfInit;

	void Start()
	{
		EnemiesAlive = 0;
		BossAlive = 0;
		//Counting how many waves there will be to apply the formula for when to spawn a boss
		StartCoroutine(EnemyCount());
		StartCoroutine(BossCount());
		totalWaves = waves.Length + bosses.Length;
		audioSource.clip = audioClipArray[0];
		audioSource.Play();
		spawnCountdownTimer = spawnTimer;
	}

	void Update ()
	{
		enemiesAlive = EnemiesAlive;
		//Have all the enemy waves been defeated?
		if (waveIndex == waves.Length)
		{
			waveComplete = true;
		}
		//Have all the boss waves been defeated?
		if (bossIndex == bosses.Length)
		{
			bossComplete = true;
		}
		if(bonusWave)
		{	
			waveCountdownText.text = string.Format("{0:00.00}", spawnCountdownTimer) + " NM incoming";
			if(spawnCountdownTimer <= 0)
			{
				bonusWave = false;
				waveCountdownText.text = " NM on it's way";
				nMIncoming = true;
				nMTimer = Random.Range(19, 31); // Maybe half of the start timer is easier to manage
			}
			
			spawnCountdownTimer -= Time.deltaTime;
		}
		if(nMIncoming)
		{
			if(nMTimer <0)
			{
				nMIncoming = false;
				waveCountdownText.text = " It's here";
				// Play different music  - maybe detect when defeated and change music back after
				StartCoroutine(BonusWave());
				// Can add feature that if you defeat [x] number of NMs in this level, you will fight a "stronger version of it"
				// Loop through [randomIndex = 0], then increase it when [x] achieved [++randomIndex;]
			}

			nMTimer -= Time.deltaTime;
		}

		//If any enemy is alive, don't proceed to the next code
		if (EnemiesAlive > 0 || BossAlive > 0)
		{
			return;
		}
		//Show "level won" (losing menu is configured under the other conditions of a loss (you lose all lives in the level etc..))
		if (waveComplete && bossComplete)
		{
			gameManager.WinLevel();
			this.enabled = false;
		}
		//If game is not complete; show/update the current wave
		else
		{
			currentWaveDisplay = currentWave + 1;
		}
		//When the counter is 0 start the wave according to whether it should be a boss or normal enemy wave
		if (countdown <= 0f)
		{
			if(currentWave <= 1)
			{
				StartCoroutine(waypoints.HighlightPath(1));
			}
			//Debug.Log("starting coroutine " + waveIndex + " waves length " + waves.Length);
			
			
			//This formula allows the level to be configured automatically and put the boss wave in a 'logical' place
			//E.G  The boss will not be first and should always be last.  If there are 15 waves and 3 bosses; it will spawn the boss after 5 enemy waves
			//If there are 20 waves and 2 bosses; it will spawn the boss after 10 enemy waves
			bossInterval = totalWaves / bosses.Length + 1;
			//Debug.Log("Total waves = " + totalWaves + ". Current wave: " + currentWave + ".  Enemy index = " + waveIndex + ". Boss interval = " + bossInterval + ".  Boss index = " + bossIndex + ".");
			//This function captures when bosses "should" actually spawn
			//E.G, Boss can't spawn on the first wave
			if (currentWave % bossInterval == 0 && currentWave != 0)
			{
				//These are the bosses "mid game"
				//Debug.Log("Boss time");
				StartCoroutine(BossCount());
				StartCoroutine(BossWave());
				countdown = timeBetweenWaves;
				++currentWave;
				currentWaveText.text = "Mini Boss ";
				audioSource.clip = audioClipArray[3];
				audioSource.Play();
				audioSource.Stop();
			}
			else if (waveComplete)
			{
				//This is the last boss once all enemy waves have been defeated
				StartCoroutine(BossCount());
				StartCoroutine(BossWave());
				countdown = timeBetweenWaves;
				//++currentWave;
				currentWaveText.text = "Final boss";
				audioSource.clip = audioClipArray[4];
				audioSource.Play();
				audioSource.Stop();
			}
			else
			{
				//If it's not a boss, it must be a normal wave
				StartCoroutine(EnemyCount());
				StartCoroutine(SpawnWave());
				if(randomChance > 0)
				{
					int rand = Random.Range(1, 100);

					if(randomChance >= rand)
					{
						bonusWave = true;
					}
				}
				countdown = timeBetweenWaves;
				++currentWave;
				currentWaveText.text = "Wave: " + currentWaveDisplay.ToString();
				audioSource.clip = audioClipArray[2];
				audioSource.Play();
			}
			CurrentWave = currentWave;
			waveCountdownText.text = " In progress";
			return;
		}
		//If none of the above, assume a level just ended and run the timer in the UI
		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
		if(currentWave > 0)
		{
			audioSource.clip = audioClipArray[1];
			if(!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
		waveCountdownText.text = string.Format("{0:00.00}", countdown);
	}

	//This function actually spawns the enemy according to what is set in the Inspector
	private IEnumerator SpawnWave()
	{
		Wave wave = waves[waveIndex];

		do
		{
			foreach (EnemyBlueprint enemy in wave.enemyWave)
			{
				if(enemy.enemySpawn == 2)
				{
					if(spawnLocationTwo.activeSelf == false)
					{
						spawnLocationTwo.SetActive(true);
						StartCoroutine(waypoints.HighlightPath(2));
					}
					spawnPoint = spawnPointTwo;
					path = enemy.enemySpawn;
				}
				else
				{
					spawnPoint = spawnPointOne;
					path = enemy.enemySpawn;
				}
				
				Refactor(enemy);
				
				yield return new WaitForSeconds(1.0f / enemy.enemyRate);
			}

			++loops;
		}
		while (counter > 0);

		++PlayerStats.Rounds;
		++waveIndex;
		yield return new WaitForSeconds(1.0f / wave.waveRate);

		void Refactor(EnemyBlueprint enemy)
		{
			if (enemy.enemyCount > 0)
			{
				SpawnEnemy(enemy.enemy);
				++EnemiesAlive;
				--enemy.enemyCount;
				--counter;
			}
		}
	}

	//This function actually spawns the boss according to what is set in the Inspector
	private IEnumerator BossWave()
	{
		Boss boss = bosses[bossIndex];
		//Debug.Log("In the boss loop");
		do
		{
			foreach (EnemyBlueprint enemy in boss.bossWave)
			{
				if(enemy.enemySpawn == 2)
				{
					if(spawnLocationTwo.activeSelf == false)
					{
						spawnLocationTwo.SetActive(true);
						StartCoroutine(waypoints.HighlightPath(2));
					}
					spawnPoint = spawnPointTwo;
					path = enemy.enemySpawn;
				}
				else
				{
					spawnPoint = spawnPointOne;
					path = enemy.enemySpawn;
				}
				Refactor(enemy);

				yield return new WaitForSeconds(1.0f / boss.bossRate);
			}

			++loops;
		}
		while (bosscounter > 0);

		++PlayerStats.Rounds;
		++bossIndex;
		yield return new WaitForSeconds(1.0f / boss.bossRate);

		void Refactor(EnemyBlueprint enemy)
		{
			if (enemy.enemyCount > 0)
			{
				SpawnEnemy(enemy.enemy);
				++BossAlive;
				--enemy.enemyCount;
				--bosscounter;
			}
		}
	}
	//This is used for bonus/random encounters, for "farmed" items
	private IEnumerator BonusWave()
	{
		Wave wave = randoms[randomIndex];

		do
		{
			foreach (EnemyBlueprint enemy in wave.enemyWave)
			{
				if(enemy.enemySpawn == 2)
				{
					if(spawnLocationTwo.activeSelf == false)
					{
						spawnLocationTwo.SetActive(true);
					}
					spawnPoint = spawnPointTwo;
					path = enemy.enemySpawn;
				}
				else
				{
					spawnPoint = spawnPointOne;
					path = enemy.enemySpawn;
				}
				++enemy.enemyCount;
				++counter;
				Refactor(enemy);
				
				yield return new WaitForSeconds(enemy.enemyRate);
			}

			++loops;
		}
		while (counter > 0);

		yield return new WaitForSeconds(1.0f / wave.waveRate);

		void Refactor(EnemyBlueprint enemy)
		{
			if (enemy.enemyCount > 0)
			{
				SpawnEnemy(enemy.enemy);
				++EnemiesAlive;
				--enemy.enemyCount;
				--counter;
			}
		}
		
	}
	//The literal spawn-into-the-game function, spawnPoint is set in the inspector
	public void SpawnEnemy (GameObject enemy)
	{
		Enemy.path = path;
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}

	//Function to count the enemies set in the Inspector
	IEnumerator EnemyCount ()
	{
		Wave wave = waves[waveIndex];

		for (int eb = 0; eb < 1; eb++)
		{
			loops = 0;
			counter = 0;
			foreach(EnemyBlueprint abcde in wave.enemyWave)
				 {
					 ecounter = abcde.enemyCount;
					 counter = counter + ecounter;
				 }
				 yield return counter;
		}
	}
	//Function to count the bosses set in the Inspector
	IEnumerator BossCount ()
	{
		Boss boss = bosses[bossIndex];

		for (int ebb = 0; ebb < 1; ebb++)
		{
			bossloops = 0;
			bosscounter = 0;
			foreach(EnemyBlueprint fghij in boss.bossWave)
				 {
					 bcounter = fghij.enemyCount;
					 bosscounter = bosscounter + bcounter;
				 }
				 yield return bosscounter;
		}
	}
}
