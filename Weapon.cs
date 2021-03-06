﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    public Transform bulletStartZone;
    private AudioSource audio;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    public bool reloadTimeSoundBased = false;

    public int maxAmmo = 60;
    [HideInInspector]
    public int reserveAmmo = 240;
    [HideInInspector]
    public int currAmmo;
    public float reloadTime = 3.0f;
    [HideInInspector]
    public bool isReloading = false;
    [HideInInspector]
    public bool isFiring = false;

    [System.Serializable]
    public class BulletBehavior
    {
        public float bulletsPerSecond = 10;
        public float bulletForce = 1000;
    }

    public BulletBehavior bulletBehavior;
    
    
    public Queue<GameObject> bulletsQueue = new Queue<GameObject>();


    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        currAmmo = maxAmmo;
        audio = GetComponentInChildren<AudioSource>();
        audio.clip = reloadSound;
        if (reloadTimeSoundBased)
        {
            reloadTime = reloadSound.length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && currAmmo != maxAmmo && !isReloading)
        {
            //RELOADING HAS BEEN MOVED TO BE MANAGED IN THE UI_MANAGER FOR NOW
            Debug.Log("Reloading");
            isReloading = true;
            StartCoroutine("Reload");
        }
        if (Input.GetKey(KeyCode.Mouse0) && currAmmo > 0 && !isReloading && !isFiring)
        {
            isFiring = true;
            StartCoroutine("Fire");
        }

    }

    public IEnumerator Fire()
    {
        //Instantiate bullet
        GameObject b = createBullet();
        b.transform.right = this.transform.right;
        b.GetComponent<Rigidbody>().AddForce(this.transform.right * bulletBehavior.bulletForce);
        audio.PlayOneShot(shootSound);
        currAmmo--;
        yield return new WaitForSeconds(fireRateConverter(bulletBehavior.bulletsPerSecond));
        isFiring = false;
        yield break;
    }

    //Temporary Event: Will be replaced by a animation event to reload
    public IEnumerator Reload()
    {
        audio.clip = reloadSound;
        audio.Play();
        yield return new WaitForSeconds(reloadTime);
        reserveAmmo -= maxAmmo - currAmmo; //Take away appropriate amount of reserve ammo
        currAmmo = maxAmmo; //Refill ammo
        //isReloading = false;
        yield break;
    }

    private float fireRateConverter(float bps)
    {
        return (1.0f / bps);
    }

    private GameObject createBullet()
    {
        int bulletsQueued = bulletsQueue.Count;
        GameObject b; 
        if (bulletsQueued != 0)
        {
            b = bulletsQueue.Dequeue();
            Transform btran = b.transform;
            btran.position = bulletStartZone.position;
            btran.rotation = Quaternion.identity;
            btran.gameObject.SetActive(true);
            return b;
        }
        else
        {
            b = Instantiate(bullet, bulletStartZone.position, Quaternion.identity);
            b.GetComponent<Bullet>().weaponQueue = bulletsQueue;
            return b;
        }

    }

}
