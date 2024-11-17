using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    private float speed;

    // booleans to dictate projectile behavior, can be changed using Method Calls 
    private bool stopAttack = false;
    private bool doesOrbit = false;

    // Variables used to calcuate the distance between the Player and the Projectile
    private Vector3 initialPosition;
    private float differenceBetweenPositions;

    //rigidbody
    Rigidbody rb;

    //reference to allow us to know the location of the player without needing it assigned in the editor (see Start for the code)
    public GameObject player;
    //reference to the player's score
    private baselineScore playerScore = new baselineScore();

    // Timer to track lifetime, used if Destroy doesn't have a 2nd argument
    private float timer = 0f;
    void Start()
    {
        //getting the Glumbo in the scene
        player = GameObject.Find("glumbo");
        //how fast the projectile will move
        speed = 40f;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        initialPosition = transform.position;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        stopProjectile();
    }

    void OnCollisionEnter(Collision collision)
    {
        //logic to make sure that we dont allow the player to get hit by their own attack
        if(collision.gameObject.tag == "player")
        {
            Destroy(this.gameObject);
        }

        //check to see if we collide with something that needs a reaction, if not it just destroys itself so you can't spam projectiles and have them linger in the world
        if (collision.gameObject.tag == "enemy")
        {
            playerScore.addScore(100);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject, 0.1f);
        }

    }

    //this makes the projectile return to the player (unused currently, but thats what it does)
    void reverseProjectileDirection()
    {
        //logic to allow us to be able to stop the player's Attack, in this case it reverses the direction of it
        if (stopAttack)
        {
            rb.velocity = -transform.forward * speed;
            stopAttack = false;
        }
    }

    void stopProjectile()
    {
        //logic to see when the player's attack travels a certain distance
        differenceBetweenPositions = Vector3.Distance(transform.position, initialPosition);
        if (differenceBetweenPositions > 5 && !doesOrbit)
        {
            stopAttack = !stopAttack;
            //stops the projectile from moving
            rb.velocity = transform.forward * 0;
            initialPosition = transform.position;
            Destroy(this.gameObject, 0.1f);
        }
        else if (doesOrbit)
        {
            orbitPlayer();
        }
    }

    void orbitPlayer()
    {
        if (player != null)
        {
            rb.velocity = transform.forward * 0;
            // Rotate around the player
            transform.RotateAround(player.transform.position, Vector3.up, speed * 3.0f * Time.deltaTime);
        }

        // Increment timer and destroy after X seconds
        timer += Time.deltaTime;
        if (timer >= 5.0f)
        {
            doesOrbit = false;
            Destroy(this.gameObject);
        }
    }

    public void activateOrbit()
    {
        doesOrbit = true;
    }
}
