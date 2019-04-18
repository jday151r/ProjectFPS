using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int ammo;
    public int clip;
    public int clipSize;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
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
        }
        else
        {
            clip += ammo;
            ammo = 0;
        }
    }
}
