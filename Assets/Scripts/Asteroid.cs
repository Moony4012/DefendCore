using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bullets))]
public class Asteroid : Enemy
{
    void Start()
    {
        base.Start();

        _startTime = Time.time;

        GameObject target = GameObject.FindObjectOfType<Planet>().Core;

        GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * (_bulletsSpeed * Random.Range(0.75f, 1.25f));
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-30, 30);
        GetComponent<Bullets>().SetDamage(_bulletsDamage);
        GetComponent<Bullets>().SetIgnoreTag(gameObject.tag);
        GetComponent<Bullets>().SetSender(gameObject);
    }
}
