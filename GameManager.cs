using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    [System.Serializable]
    public class Respawn
    {
        public Vector3 respawnPos;
        public Quaternion respawnRot; 
    }
    public Respawn respawn;

    // Start is called before the first frame update
    void Start()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (respawn.respawnPos == null)
        {
            respawn.respawnPos = player.transform.position;
        }
        if (respawn.respawnRot == null)
        {
            respawn.respawnRot = player.transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RespawnMethod();
        }
    }


    public void RespawnMethod()
    {
        player.transform.position = respawn.respawnPos;
        player.transform.rotation = respawn.respawnRot;
    }
}
