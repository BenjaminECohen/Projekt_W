using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    [System.Serializable]
    public class Respawn
    {
        public Transform respawnPoint;
        public GameObject respawnPrefab;
        [HideInInspector]
        public GameObject tempRespawnPoint;
    }
    public Respawn respawn;

    [HideInInspector]
    public bool freezePlayer;

    private EntityHealth playerHealth;
    private UI_Manager uim;

    // Start is called before the first frame update
    void Start()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (respawn.respawnPoint == null)
        {
            respawn.tempRespawnPoint = Instantiate(respawn.respawnPrefab, player.transform.position, Quaternion.identity);
            respawn.respawnPoint = respawn.tempRespawnPoint.transform;
        }
        
        playerHealth = player.GetComponent<EntityHealth>();
        uim = GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.health <= 0)
        {
            RespawnMethod();
        }

        //NonGameAction Input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            freezePlayer = uim.togglePanel(uim.pauseMenu);
            
        }
    }


    public void RespawnMethod()
    {
        player.transform.position = respawn.respawnPoint.position;
        player.transform.rotation = respawn.respawnPoint.rotation;
        playerHealth.health = playerHealth.maxHealth;
    }
}
