using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectileBehavior : MonoBehaviour
{
    private float speed;
    private baselineScore playerScore = new baselineScore();
    private bool scoreControl = true;
    void Start()
    {
        speed = 15f;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (scoreControl)
            {
                playerScore.addScore(-100);
                scoreControl = false;
            }
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject, 3.0f);
        }
        scoreControl = true;
    }
}
