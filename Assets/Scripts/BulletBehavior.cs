using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public enum BulletType { PlayerBullet };
    public BulletType bulletType;
    public Vector3 target;
    public LineRenderer line;
    public GameObject effect;
    public float lifeTime;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            effect.transform.parent = null;
            Destroy(gameObject);
        }

        switch (bulletType)
        {
            case BulletType.PlayerBullet:
                line.SetPositions(new Vector3[] { transform.position, target });
                break;
        }
    }
}