using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEvents : MonoBehaviour
{
    public GameManager GM;

    public void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void StartReload()
    {
        GM.reloading = true;
    }
    void EndReload()
    {
        GM.Reload();
    }
}
