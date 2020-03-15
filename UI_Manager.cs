using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    public Weapon weapon;

    [System.Serializable]
    public class AmmoPanel
    {
        public Text maxAmmoText;
        public Text currAmmoText;
        public Image currAmmoAmount;
        public Image reloadAmount;
    }
    public AmmoPanel ammoPanel;

    //For reload
    private float reloadTimer;
    private bool reloadMutex = false;


    // Start is called before the first frame update
    void Start()
    {
        ammoPanel.maxAmmoText.text = weapon.maxAmmo.ToString();
        ammoPanel.currAmmoText.text = weapon.currAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        weaponManagement();
    }


    private void weaponManagement()
    {
        ammoPanel.maxAmmoText.text = weapon.maxAmmo.ToString();
        ammoPanel.currAmmoText.text = weapon.currAmmo.ToString();
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
                }
            }
        }
        else
        {
            reloadMutex = false;
        }
    }
}
