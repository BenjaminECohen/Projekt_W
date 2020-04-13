using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Health
{
    
    //Deal damage to an object
    public static bool dealDamage(GameObject obj, int damageValue)
    {
        if(obj.TryGetComponent(out EntityHealth health))
        {
            obj.GetComponent<EntityHealth>().health -= damageValue;
            //If the obj is an enemy
            if (obj.tag == "Enemy")
            {
                //Destroy the object if the health is sub or equal to 0
                if (obj.GetComponent<EntityHealth>().health <= 0)
                {
                    GameObject.Destroy(obj);
                }
                

            }
            return true;
        }
        else
        {
            Debug.Log("There is no health script on this object");
            return false;
        }
        
        
    }

    public static void checkHealthOverflow(GameObject obj)
    {
        EntityHealth objHealth = obj.GetComponent<EntityHealth>();
        if (objHealth.health > objHealth.maxHealth)
        {
            objHealth.health = objHealth.maxHealth;
        }
    }
}
