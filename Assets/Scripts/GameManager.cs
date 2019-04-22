using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int ammo;
    public int clip;
    public int clipSize;
    public bool reloading;
    public TextMeshProUGUI remainingAmmo;
    public Animator gunAnimator;
    public AnimationClip reloadAnim;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        gunAnimator.SetBool("Reload", false); //Ensures animation does not play more than onceS
        remainingAmmo.text = ammo.ToString();
        if ((Input.GetKeyDown(KeyCode.R) || clip == 0) && CanReload())
            gunAnimator.SetBool("Reload", true);
    }

    public void Reload()
    {
        if (clip < clipSize) //If there are bullets missing from the clip
        {
            if (ammo - (clipSize - clip) >= 0) //See if we have enough ammo to reload the clip
            {
                ammo -= clipSize - clip;
                clip += clipSize - clip;
                gunAnimator.SetBool("Reload", true);
            }
            else if(ammo > 0) //Fill the clip with the rest of the ammo
            {
                clip += ammo;
                ammo = 0;
                gunAnimator.SetBool("Reload", true);
            }
        }
        reloading = false;
    }
    public bool CanReload()
    {
        //See if there are bullets missing from the clip, and if there is any ammo left
        return clip < clipSize && ammo > 0;
    }
}
