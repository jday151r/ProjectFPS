using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI remainingAmmo;
    public GunEvents gun;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //Ensures animations do not play more than once
        remainingAmmo.text = gun.ammo.ToString();
    }
}
