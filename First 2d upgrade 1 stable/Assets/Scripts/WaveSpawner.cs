using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    private AudioStuff.AudioManager audioManager;

    [System.Serializable]
	public class Wave
    {
        public string name;
        public Transform enemy;
        
        public int count;
        public float rate;
    }
    public Transform[] spawnPoints;
    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave+1; }
    }

    public float timeBetweenWaves = 5f;
    private float waveCountdown=3f;

    public float WaveCountdown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }

    private void Start()
    {
        waveCountdown = timeBetweenWaves;

    }

    private void Update()
    {

        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        //begin new round
        Debug.Log("Wave Complete");
        
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        /* instead of this if
         * a random wave selection variable can
         * be used.
         */
        //nextWave = Random.Range(0, waves.Length);
        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All waves done");
            
            return;
        }

        nextWave++;
        audioManager = AudioStuff.AudioManager.instance;
        audioManager.PlaySound("WaveComplete");
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        //spawn

        for(int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points");
        }
        //spawn enemy 
        Debug.Log("Spawning enemy: "+_enemy.name);
        Transform _sPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sPoint.transform.position, _sPoint.transform.rotation);
    }
}
