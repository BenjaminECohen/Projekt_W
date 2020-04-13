using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Queue<GameObject> weaponQueue;
    public float killDistance;
    private Vector3 originPosition;
    public int dmgToPlayer = 5;
    public int dmgToEnemy = 10;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("bulletLife");
    }

    private void Update()
    {
        if (Vector3.Distance(this.gameObject.transform.position, originPosition) >= killDistance)
        {
            addToQueue();
        }
    }

    private IEnumerator bulletLife()
    {
        yield return new WaitForSeconds(2.0f);
        if (this.gameObject)
        {
            Destroy(this.gameObject);
        }
        yield break;
    }

    //When the bullet hits something
    private void OnCollisionEnter(Collision collision)
    {
        GameObject entity = collision.gameObject;
        //FIXME: make it so the damage can be changed based on the weapon
        if (entity.tag == "Player")
        {
            Health.dealDamage(entity, dmgToPlayer);
        }
        else if (entity.tag == "Enemy")
        {
            Health.dealDamage(entity, dmgToEnemy);
        }

        //Destroy(this.gameObject);
        
        //Add bullet to weapon queue
        if (weaponQueue != null)
        {
            addToQueue();
        }
        else
        {
            Destroy(this.gameObject);
        }
        

    }

    private void addToQueue()
    {
        weaponQueue.Enqueue(this.gameObject);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
