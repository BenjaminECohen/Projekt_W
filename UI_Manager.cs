using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    
    public Weapon weapon;
    public EntityHealth player;

    [System.Serializable]
    public class AmmoPanel
    {
        public Text maxAmmoText;
        public Text currAmmoText;
        public Image currAmmoAmount;
        public Image reloadAmount;
        public Text reserveAmmo;
    }
    public AmmoPanel ammoPanel;

    //For reload
    private float reloadTimer;
    private bool reloadMutex = false;

    [System.Serializable]
    public class HealthPanel
    {
        public Text healthAmount;
    }
    public HealthPanel healthPanel;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        ammoPanel.maxAmmoText.text = weapon.maxAmmo.ToString();
        ammoPanel.currAmmoText.text = weapon.currAmmo.ToString();
        ammoPanel.reserveAmmo.text = weapon.reserveAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        weaponManagement();
        playerHealthManagement();
    }


    private void weaponManagement()
    {
        ammoPanel.maxAmmoText.text = weapon.maxAmmo.ToString();
        ammoPanel.currAmmoText.text = weapon.currAmmo.ToString();
        ammoPanel.reserveAmmo.text = weapon.reserveAmmo.ToString();
        ammoPanel.currAmmoAmount.fillAmount = (float) weapon.currAmmo / (float) weapon.maxAmmo;

        if (weapon.isReloading)
        {
            if (!reloadMutex)
            {
                reloadMutex = true;
                reloadTimer = Time.time;
            }
            else
            {
                ammoPanel.reloadAmount.fillAmount = (Time.time - reloadTimer) / weapon.reloadTime;
                if (ammoPanel.reloadAmount.fillAmount == 1)
                {
                    ammoPanel.reloadAmount.fillAmount = 0;
                    weapon.reserveAmmo -= weapon.maxAmmo - weapon.currAmmo;
                    weapon.currAmmo = weapon.maxAmmo;
                    weapon.isReloading = false;
                }
            }
        }
        else
        {
            reloadMutex = false;
        }
    }


    private void playerHealthManagement()
    {
        healthPanel.healthAmount.text = player.health.ToString();
        if (player.health < 0)
        {
            player.health = 0;
        }
    }


    public void sceneChange(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool togglePanel(GameObject panel)
    {
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);
            return false;
        }
        else
        {
            panel.SetActive(true);
            return true;
        }
    }
}
