using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public int maxHealth = 100;


    void Start()
    {
        health = maxHealth;
    }


}
