using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public int enemyKillCount=0;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3.5f;
    public Transform spawnPrefab;
    public string spawnSoundName;

    [SerializeField]
    private GameObject gameOverUI;

    //cache(?)
    private AudioStuff.AudioManager audioManager;

    public CameraShake cameraShake;
    public GameObject enemyDeathParticles;
    
    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("No camera shaker found");
        }
        _remainingLives = maxLives;
        //caching
        audioManager = AudioStuff.AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audio manager found in scene");
        }
        
        audioManager.PlaySound("Signature");
    }

    void Update()
    {
        if ((Input.GetKeyDown("space")))
        { 
            audioManager.PlaySound("Jump");
        }
    }

    public IEnumerator _RespawnPlayer()
    {
        //"TODO: Add fancy particle system and sounds");
        audioManager = AudioStuff.AudioManager.instance;
        audioManager.PlaySound(spawnSoundName);
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject ss = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject as GameObject;
        Destroy(ss, 3f);
    }



    public static void KillPlayer(Player playa)
    {
        AudioStuff.AudioManager.instance.PlaySound("PlayerDead");
        _remainingLives--;
        Destroy(playa.gameObject);
        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer()); 
        }
    }

    public void EndGame()
    {
        Debug.Log("Game over!");
        audioManager.PlaySound("GameOver");
        gameOverUI.SetActive(true);

    }

    public static void KillEnemy(Enemy enemy)
    {
        
        gm._KillEnemy(enemy);
        //gm.StartCoroutine(gm.Respawn());
    }
    public void _KillEnemy(Enemy _enemy)
    {
        enemyKillCount++;
        cameraShake.Shake(_enemy.shakeAmt,_enemy.shakeLength);
        Destroy(_enemy.gameObject);
        GameObject _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position,Quaternion.identity).gameObject;
        Destroy(_clone, 5f);
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;
    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public Transform eSpawnPoint;
    public float spawnDelay = 3.5f;
    public Transform spawnPrefab;
    public Transform eSpawnPrefab;
    public Transform eRandom;
    void Reposition()
    {
        Vector3 tempPos = eRandom.transform.position;
        float offsetX = Random.Range(-20, 20);
        float offsetY = Random.Range(0,5);
        tempPos.x += offsetX;
        tempPos.y += offsetY;
        eSpawnPoint.transform.position = tempPos;
    } 

    public IEnumerator Respawn()
    {
        //"TODO: Add fancy particle system and sounds");
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject ss = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject as GameObject;
        Destroy(ss, 3f);
    }

    public IEnumerator eRespawn()
    {
        //"TODO: Add fancy particle system and sounds");
        //GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay/3);
        Reposition();
        Instantiate(enemyPrefab, eSpawnPoint.position, eSpawnPoint.rotation);
        GameObject ss = Instantiate(eSpawnPrefab, eSpawnPoint.position, eSpawnPoint.rotation).gameObject as GameObject;
        Destroy(ss, 3f);
    }

    public static void KillPlayer(Player playa)
    {
        Destroy(playa.gameObject);
        gm.StartCoroutine(gm.Respawn());
    }

    public static void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        gm.StartCoroutine(gm.eRespawn());
    }

}
*/
