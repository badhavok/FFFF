using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {

	public static int EnemiesAlive = 0;
	public static int BossAlive = 0;
	public static int loops = 0;
	public static int bossloops = 0;

	public Wave[] waves;
	public Boss[] bosses;

	public GameObject enemy;
	public GameObject boss;

	public Transform spawnPoint;

	public float timeBetweenWaves = 1f;
	private float countdown = 1f;

	public int waveIndex = 0;
	public int waveIndexDisplay = 0;
	public bool waveComplete = false;
	public int counter = 0;
	public int ecounter = 0;

	public int bossIndex = 0;
	public int bossIndexDisplay = 0;
	public bool bossComplete = false;
	public int bosscounter = 0;
	public int bcounter = 0;

	public static int CurrentWave;
	public int currentWave = 0;
	private int currentWaveDisplay = 0;
	private int totalWaves = 0;
	private int bossInterval = 0;

	public Text waveCountdownText;
	public Text currentWaveText;

	public GameManager gameManager;
//	public GFInit gfInit;

	void Start()
	{
		EnemiesAlive = 0;
		BossAlive = 0;
		StartCoroutine(EnemyCount());
		StartCoroutine(BossCount());
		totalWaves = waves.Length + bosses.Length;
	}

	void Update ()
	{
		if (waveIndex == waves.Length)
		{
			waveComplete = true;
		}

		if (bossIndex == bosses.Length)
		{
			bossComplete = true;
		}

		if (EnemiesAlive > 0 || BossAlive > 0)
		{
			return;
		}

		if (waveComplete && bossComplete)
		{
			gameManager.WinLevel();
			this.enabled = false;
		}
		else
		{
			currentWaveDisplay = currentWave + 1;
		}

		if (countdown <= 0f)
		{
			Debug.Log("starting coroutine " + waveIndex + " waves length " + waves.Length);
			StartCoroutine(EnemyCount());
			StartCoroutine(BossCount());
			bossInterval = totalWaves / bosses.Length + 1;

			Debug.Log("Total waves = " + totalWaves + ". Current wave: " + currentWave + ".  Enemy index = " + waveIndex + ". Boss interval = " + bossInterval + ".  Boss index = " + bossIndex + ".");

			if (currentWave % bossInterval == 0 && currentWave != 0)
			{
				Debug.Log("Boss time");
				StartCoroutine(BossWave());
				countdown = timeBetweenWaves;
				++currentWave;
				currentWaveText.text = "Mini Boss ";
			}
			else if (waveComplete)
			{
				StartCoroutine(BossWave());
				countdown = timeBetweenWaves;
				++currentWave;
				currentWaveText.text = "Final boss";
			}
			else
			{
				StartCoroutine(SpawnWave());
				countdown = timeBetweenWaves;
				++currentWave;
				currentWaveText.text = "Wave: " + currentWaveDisplay.ToString();
			}
			CurrentWave = currentWave;
			return;
		}

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

		waveCountdownText.text = string.Format("{0:00.00}", countdown);
	}

	private IEnumerator SpawnWave()
	{
		Wave wave = waves[waveIndex];

		do
		{
			foreach (EnemyBlueprint enemy in wave.enemyWave)
			{
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

	private IEnumerator BossWave()
	{
		Boss boss = bosses[bossIndex];

		do
		{
			foreach (EnemyBlueprint enemy in boss.bossWave)
			{
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
				++EnemiesAlive;
				--enemy.enemyCount;
				--bosscounter;
			}
		}
	}

	public void SpawnEnemy (GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}

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
