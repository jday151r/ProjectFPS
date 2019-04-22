using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int ammo;
    public int clip;
    public int clipSize;
    public TextMeshProUGUI remainingAmmo;
    public Animator gunAnimator;
    public AnimationClip reloadAnim;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        gunAnimator.SetBool("Reload", false);
        remainingAmmo.text = ammo.ToString();
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
    }

    void Reload()
    {
        if (ammo - (clipSize - clip) >= 0)
        {
            ammo -= clipSize - clip;
            clip += clipSize - clip;
            gunAnimator.SetBool("Reload", true);
        }
        else
        {
            clip += ammo;
            ammo = 0;
            gunAnimator.SetBool("Reload", true);
        }
    }
}
