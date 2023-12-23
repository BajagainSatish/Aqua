using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int weaponDamage;
    public bool isPlayer1Projectile;
    public bool isArcherOrGunmanProjectile;//if archer or gunman projectile, damage only to ship men, negation means belongs to cannon or mortar.
}
