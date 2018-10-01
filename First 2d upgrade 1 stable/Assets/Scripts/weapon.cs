using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour {
    private AudioStuff.AudioManager audioManager;

    public float fireRate = 0;
    public int Damage = 10;
    public LayerMask ToHit;

    public Transform MuzzleFlash;
    public Transform hitPrefab;
    public Transform BulletEffect;
    float timeToSpawnEffect=0;
    public float EffectRate = 10;
    float timeToFire = 0;
    Transform firePoint;
    //camera shake
    public float camShakeAmt = 0.05f;
    public float length = 0.1f;
    CameraShake camShake;
	// Use this for initialization
	void Awake () {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firepoint cited, asshole");
        }
	}

    private void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null)
        {
            Debug.LogError("No camerashake script");
        }
    }

    // Update is called once per frame
    void Update () {
        if (fireRate == 0)
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time>timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    void Shoot()
    {
        Vector2 mouseAt = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointAt = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointAt,mouseAt-firePointAt,100,ToHit);
        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;
            if (hit.collider == null)
            {
                hitPos = (mouseAt - firePointAt) * 100;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            BullEffect(hitPos,hitNormal);
            timeToSpawnEffect = Time.time + 1 / EffectRate;
        }
        Debug.DrawLine(firePointAt, mouseAt);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointAt, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Damage);
                //Debug.Log("You hit " + hit.collider.name + " causing " + Damage + " damage.");
            }
        }
    }
    void BullEffect(Vector3 hitPosition,Vector3 hitNormal)
    {
        
        
        Transform trail = Instantiate(BulletEffect, firePoint.position, firePoint.rotation)as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            //SET POSITION
            lr.SetPosition(0,firePoint.position);
            lr.SetPosition(1, hitPosition);
        }

        Destroy(trail.gameObject, 0.02f);

        if(hitNormal!=new Vector3(9999, 9999, 9999))
        {
            Transform hit = Instantiate(hitPrefab, hitPosition, Quaternion.FromToRotation(Vector3.right, hitNormal))as Transform;
            Destroy(hit.gameObject, 1f);
        }
        
        Transform MuzzleClone = (Transform)Instantiate(MuzzleFlash, firePoint.position, firePoint.rotation);
        float size = Random.Range(1f, 2.0f);
        MuzzleClone.localScale = new Vector3(size, size, size);
        Destroy (MuzzleClone.gameObject,0.02f);
        //shake camera
        camShake.Shake(camShakeAmt, length);
        audioManager = AudioStuff.AudioManager.instance;      
        audioManager.PlaySound("GunSound");
    }

    
}
