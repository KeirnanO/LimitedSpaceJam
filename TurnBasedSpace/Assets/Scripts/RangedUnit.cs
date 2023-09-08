using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : Entity
{
    public int damage;

    public Bullet bullet;
    public Transform bulletSpawnPoint;

    public void StartFire()
    {
        GetComponent<Animator>().SetTrigger("Shoot");
    }

    public void Fire()
    {
        var newBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        newBullet.damage = damage;
    }
}
