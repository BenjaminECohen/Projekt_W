using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform bulletStartZone;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFiring)
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
        yield return new WaitForSeconds(fireRateConverter(bulletBehavior.bulletsPerSecond));
        isFiring = false;
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
            b.name = "Enemy Bullet";
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
