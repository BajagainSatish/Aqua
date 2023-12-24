using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int weaponDamage;
    public bool isPlayer1Projectile;
    public bool isArcherOrGunmanProjectile;//if archer or gunman projectile, damage only to ship men, negation means belongs to cannon or mortar.

    public Vector3 finalPos;

    private float projectileDistanceDisableThreshold;

    private void Awake()
    {
        projectileDistanceDisableThreshold = SetParameters.ProjectileDistanceDisableThreshold;
    }

    private void Update()
    {
        DeactivateProjectileIfReachFinalPosition();//prevent projectile bounce back effect
    }
    private bool VectorApproximatelyEqual(Vector3 a, Vector3 b, float threshold)
    {
        return Mathf.Abs(a.x - b.x) <= threshold
            && Mathf.Abs(a.y - b.y) <= threshold
            && Mathf.Abs(a.z - b.z) <= threshold;
    }
    private void DeactivateProjectileIfReachFinalPosition()
    {
        if (finalPos != null)
        {
            Vector3 currentPos = transform.position;
            if (VectorApproximatelyEqual(finalPos, currentPos, projectileDistanceDisableThreshold))
            {
                print("Projectile reached final position!!!");
                gameObject.SetActive(false);
            }
        }
    }
}
