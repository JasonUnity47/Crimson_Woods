using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // Declaration

    // Class (What is Wave?)
    [System.Serializable] // In order to see and acess the class member in the Inspector.
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public enum SpawnState { SPAWNNING, WAITING, COUNTING };

    public Wave[] waves;
    public int nextWave = 0;

    public Transform[] spawnPos;

    public float timeBtwWaves = 5f;
    private float waveCountDown;

    private float timeBtwSearch = 1f;
    private float searchCountDown;

    public bool isEnd = false;

    private SpawnState state = SpawnState.COUNTING;

    // Script Reference
    private BuffSystem buffSystem;

    private void Start()
    {
        // Error Check
        if (spawnPos.Length == 0)
        {
            Debug.LogError("NO SPAWN POINT AVAILABLE.");
        }

        // Get References
        buffSystem = GetComponent<BuffSystem>();

        // Initialize the wave timer.
        waveCountDown = timeBtwWaves;

        // Initialize the search timer.
        searchCountDown = timeBtwSearch;
    }

    private void Update()
    {
        if (!isEnd)
        {
            if (state == SpawnState.WAITING)
            {
                if (!EnemyIsAlive())
                {
                    // Begin a new wave.
                    WaveCompleted();
                }

                else
                {
                    // Waiting until players kill all the enemies in the wave.
                    return;
                }
            }


            if (waveCountDown <= 0)
            {
                if (state != SpawnState.SPAWNNING)
                {
                    // Enter spawning state.
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }

            else
            {
                waveCountDown -= Time.deltaTime;
            }
        }
    }

    bool EnemyIsAlive()
    {
        // Check whether there is any enemy is alive in the game.
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0)
        {
            searchCountDown = timeBtwSearch;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    void WaveCompleted()
    {
        Debug.Log("WAVE COMPLETED!");

        state = SpawnState.COUNTING;
        waveCountDown = timeBtwWaves;

        // If the current wave is not a boss wave then allows the player to choose a buff after finished the current wave.
        if (nextWave != 4 && nextWave != 5 && nextWave != 10 && nextWave != 11)
        {
            buffSystem.buffPanel.SetActive(true);
        }

        if (nextWave + 1 > waves.Length - 1)
        {
            // Game End Section.
            Debug.Log("ALL WAVES COMPLETED!");
            isEnd = true;
        }

        else
        {
            // Continue spawn the next wave.
            nextWave++;
        }

        return;
    }

    void SpawnEnemy(Transform _enemy)
    {
        // Clone enemy = Spawn enemy.
        // Random spawn at random spawn point.
        Transform sp = spawnPos[Random.Range(0, spawnPos.Length)];

        Instantiate(_enemy, sp.position, Quaternion.identity);

        return;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNNING; // Start spawning enemy.

        // Spawn enemy based on the count of enemy in the wave.
        for (int i = 0; i < _wave.count; i++)
        {
            // Spawn enemy.
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate); // Waiting for the next spawn.
        }

        state = SpawnState.WAITING; // Waiting players to finish the current wave.

        yield break;
    }
}
