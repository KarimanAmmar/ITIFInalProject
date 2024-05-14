using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
	[SerializeField] private List<EnemyPool> enemyPools;
	[SerializeField] private List<WaveData> wavesData;
	[SerializeField] private int currentWaveIndex = 0;
	[SerializeField] private Transform playerTransform;
	private int totalEnemiesInWave = 0;
	private bool spawningInProgress = false;

	private void Start()
	{
		totalEnemiesInWave = wavesData[currentWaveIndex].CalculateTotalEnemies();
		StartCoroutine(SpawnWave(currentWaveIndex));
	}

	private IEnumerator SpawnWave(int currentWaveIndex)
	{
		spawningInProgress = true;

		foreach (NumberOfEnemies enemyData in wavesData[currentWaveIndex].TypeOfEnemies)
		{
			int randomIndex = Random.Range(0, wavesData[currentWaveIndex].spawnPoints.Count);
			Vector3 spawnPosition = wavesData[currentWaveIndex].spawnPoints[randomIndex].position;
			yield return StartCoroutine(SpawnEnemies(enemyData, spawnPosition));
		}

		spawningInProgress = false;
	}

	private IEnumerator SpawnEnemies(NumberOfEnemies enemyData, Vector3 spawnPosition)
	{
		if ((int)enemyData.enemyType < 0 || (int)enemyData.enemyType >= enemyPools.Count)
		{
			Debug.LogError("Invalid enemy type index: " + (int)enemyData.enemyType);
			yield break;
		}

		for (int i = 0; i < enemyData.numberOfEnemy; i++)
		{
			enemyPools[(int)enemyData.enemyType].ActivateEnemy(spawnPosition);
			yield return new WaitForSeconds(enemyData.delayBetweenSpawns);
		}
	}

	public void EnemyKill()
	{
		totalEnemiesInWave--;

		if (totalEnemiesInWave <= 0 && !spawningInProgress)
		{
			WaveData waveData = wavesData[currentWaveIndex];
			waveData.isWaveCompleted = true;
			wavesData[currentWaveIndex] = waveData;
			CheckWaveCompletion();
		}
	}

	private void CheckWaveCompletion()
	{
		if (wavesData[currentWaveIndex].isWaveCompleted)
		{
			currentWaveIndex++;

			if (currentWaveIndex < wavesData.Count)
			{
				totalEnemiesInWave = wavesData[currentWaveIndex].CalculateTotalEnemies();
				StartCoroutine(SpawnWave(currentWaveIndex));
			}
			else
			{
				currentWaveIndex = 0; // Loop back to the first wave
				totalEnemiesInWave = wavesData[currentWaveIndex].CalculateTotalEnemies();
				StartCoroutine(SpawnWave(currentWaveIndex));
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			int randomEnemyPoolIndex = Random.Range(0, enemyPools.Count);
			enemyPools[randomEnemyPoolIndex].DisableRandomEnemy();
		}
	}
}