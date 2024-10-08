using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbitingProjectileMovement : MonoBehaviour
{
    [Header("Orbit Settings")]
    [Tooltip("The center point to orbit around, typically the player.")]
    public Transform orbitCenter;
    public GameObject temp;

    [Tooltip("Speed at which the projectile orbits around the center (degrees per second).")]
    public float orbitSpeed = 50f;

    [Tooltip("Radius of the orbit.")]
    public float orbitRadius = 5f;

    [Tooltip("Time in seconds before the projectile is destroyed automatically.")]
    public float lifetime = 10f;

    // Timer to track lifetime
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //getting the Glumbo in the scene to that way the origin follows it
        temp = GameObject.Find("/glumbo/glumboBody/attackOrigin");
        orbitCenter = temp.transform;
        if (orbitCenter == null)
        {
            Debug.LogError("orbitingProjectileMovement: Orbit Center not assigned.");
            Destroy(gameObject);
            return;
        }

        Vector3 initialPosition = orbitCenter.position + orbitCenter.right * orbitRadius;
        transform.position = initialPosition;
        transform.LookAt(orbitCenter);
    }

    // Update is called once per frame
    void Update()
    {
        if (orbitCenter != null)
        {
            // Rotate around the orbitCenter
            transform.RotateAround(orbitCenter.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }

        // Increment timer and destroy after lifetime
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    /*void OnCollisionEnter(Collision collision)
    {
        //check to see if we collide with something that needs a reaction, if not it just destroys itself so you can't spam projectiles and have them linger in the world
        if (collision.gameObject.CompareTag("enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }*/
}
