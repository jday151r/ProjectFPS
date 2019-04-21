using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public enum BulletType { PlayerBullet };
    public BulletType bulletType;
    public Vector3 target;
    public LineRenderer line;
    //public TrailRenderer trail;
    public float bulletLerp;

    void Awake()
    {
        //trail.SetPosition(0, transform.position);
    }

    void Update()
    {
        switch (bulletType)
        {
            case BulletType.PlayerBullet:
                //trail.SetPositions(new Vector3[] { transform.position, target });
                //transform.position = Vector3.Lerp(transform.position, target, bulletLerp);
                line.SetPositions(new Vector3[] { transform.position, target });
                break;
        }
    }
}