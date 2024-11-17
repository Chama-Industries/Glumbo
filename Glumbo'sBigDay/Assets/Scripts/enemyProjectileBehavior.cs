using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectileBehavior : MonoBehaviour
{
    private float speed;
    void Start()
    {
        speed = 15f;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "player")
        {
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject, 3.0f);
        }

    }
}
