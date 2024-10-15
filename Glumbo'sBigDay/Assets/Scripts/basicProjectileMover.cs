using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectileMover : MonoBehaviour
{
    public RigidbodyExtensions projectile; // Assign via Inspector or dynamically
    private float speed = 40f;

    void Start()
    {
        if (projectile == null)
        {
            // Attempt to find the RigidbodyExtensions component on the same GameObject
            projectile = GetComponent<RigidbodyExtensions>();
            if (projectile == null)
            {
                Debug.LogError("RigidbodyExtensions component not found on this GameObject.");
            }
        }

        // Configure projectile properties if needed
        projectile.MaxDistance = 30f;
        projectile.LaunchForce = speed;
    }

    void Update()
    {
        // Example: Launch the projectile when the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && projectile != null && !projectile.IsLaunched)
        {
            Vector3 launchDirection = transform.forward; // Launch in the forward direction
            projectile.Launch(launchDirection, projectile.LaunchForce);
        }

        // Destroy the projectile after 1 second if it hasn't returned yet
        // Note: This is already handled in RigidbodyExtensions via DestroyAfterTime()
        // So, this line can be removed unless additional conditions are needed
        // Destroy(this.gameObject, 1.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Collision handling is managed in RigidbodyExtensions
        // If additional collision logic is needed, implement here
    }

    // Intended to return projectile - handled in RigidbodyExtensions
    // Removed reverseProjectile method as it's not functional and managed in RigidbodyExtensions
}