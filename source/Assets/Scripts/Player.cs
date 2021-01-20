using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private AudioStuff.AudioManager audioManager;
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    public float fallToDeath = -20f;
    [SerializeField]
    private StatusIndicator statusIndicator;

    private void Start()
    {
        stats.Init();
        if (statusIndicator == null)
        {
            Debug.LogError("Player Status Indicator not working");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth,stats.maxHealth);
        }
    }

    public PlayerStats stats = new PlayerStats();
    
    public void Update()
    {
        if (transform.position.y <= fallToDeath)
        {
            DamagePlayer(1000);
        }
    }

    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
        audioManager = AudioStuff.AudioManager.instance;
            
        if (stats.curHealth <= 0)
        {
            audioManager.PlaySound("PlayerDead");
            GameMaster.KillPlayer(this);
            Debug.Log("No player on the properties panel lol");
            return;
        }

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        audioManager.PlaySound("PlayerDamage");
    }
}
