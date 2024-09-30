using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicProjectileMover : MonoBehaviour
{
    private float speed;
    void Start()
    {
        //how fast the projectile will move
        speed = 15f;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        //check to see if we collide with something that needs a reaction, if not it just destroys itself so you can't spam projectiles and have them linger in the world
        if (collision.gameObject.tag == "enemy")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject, 4.0f);
        }

    }
}
