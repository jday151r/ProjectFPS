using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public enum BulletType { PlayerBullet };
    public BulletType bulletType;
    public Vector3 target;
    public float bulletLerp;
    
    void Update()
    {
        switch (bulletType)
        {
            case BulletType.PlayerBullet:
                transform.position = Vector3.Lerp(transform.position, target, bulletLerp);
                break;
        }
    }
}