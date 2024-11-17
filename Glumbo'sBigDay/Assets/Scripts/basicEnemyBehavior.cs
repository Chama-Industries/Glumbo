using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemyscript : MonoBehaviour
{
    public GameObject player;
    protected double distanceFromPlayer;
    private NavMeshAgent enemy;

    //variables related to the enemy attack
    public GameObject bullet;
    public Transform bulletOrigin;
    private int wait = 0;
    private bool hasFired = false;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
    }

    void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer < 25.0f)
        {
            if (distanceFromPlayer < 10.0f && wait >= 100 && !hasFired)
            {
                shoot();
            }
            pursuit();
        }
    }

    void FixedUpdate()
    {
        if (wait == 100 && hasFired)
        {
            wait = 0;
            hasFired = false;
        }
        else
        {
            wait++;
        }
    }

    void shoot()
    {
        GameObject g = Instantiate(bullet, bulletOrigin.position, bulletOrigin.rotation);
        hasFired = true;
        Destroy(g, 4.0f);
    }

    void pursuit()
    {
        enemy.SetDestination(player.transform.position);
    }
}