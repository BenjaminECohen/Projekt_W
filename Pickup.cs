using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update

    
    public enum PickupType
    {
        Health,
        Ammo
    }
    public PickupType pickupType;

    public int value;

    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision on Pickup");
        if (collision.gameObject.tag == "Player")
        {
            if (pickupType == PickupType.Ammo)
            {
                collision.gameObject.GetComponentInChildren<Weapon>().reserveAmmo += value;
                Destroy(this.gameObject);
            }
            else
            {
                if (collision.gameObject.GetComponent<EntityHealth>().health != collision.gameObject.GetComponent<EntityHealth>().maxHealth)
                {
                    collision.gameObject.GetComponent<EntityHealth>().health += value;
                    Health.checkHealthOverflow(collision.gameObject);
                    Destroy(this.gameObject);
                }           
            }
            
            
        }
    }
}
